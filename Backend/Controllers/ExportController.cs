using Backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ExportController : ControllerBase
{
    private readonly AppDbContext _context;

    public ExportController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("selections")]
    public async Task<IActionResult> ExportSelections([FromQuery] int queueId, [FromQuery] string format = "csv")
    {
        var queue = await _context.Queues
            .Include(q => q.Project)
            .FirstOrDefaultAsync(q => q.Id == queueId);

        if (queue == null)
        {
            return NotFound(new { message = "队列不存在" });
        }

        var selections = await _context.SelectionRecords
            .Include(s => s.User)
            .Include(s => s.SelectedImage)
            .Where(s => s.QueueId == queueId)
            .OrderBy(s => s.ImageGroup)
            .ThenBy(s => s.UserId)
            .ToListAsync();

        if (format.ToLower() == "csv")
        {
            var csv = new StringBuilder();
            csv.AppendLine("用户ID,用户名,图片组,选择的文件夹,选择的文件名,选择时间");

            foreach (var selection in selections)
            {
                csv.AppendLine($"{selection.UserId},{selection.User.Username},{selection.ImageGroup},{selection.SelectedImage.FolderName},{selection.SelectedImage.FileName},{selection.CreatedAt:yyyy-MM-dd HH:mm:ss}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            var fileName = $"selections_{queue.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            
            return File(bytes, "text/csv", fileName);
        }
        else if (format.ToLower() == "json")
        {
            var data = selections.Select(s => new
            {
                UserId = s.UserId,
                Username = s.User.Username,
                ImageGroup = s.ImageGroup,
                SelectedFolderName = s.SelectedImage.FolderName,
                SelectedFileName = s.SelectedImage.FileName,
                SelectedFilePath = s.SelectedImage.FilePath,
                CreatedAt = s.CreatedAt
            }).ToList();

            return Ok(data);
        }
        else
        {
            return BadRequest(new { message = "不支持的导出格式。支持的格式：csv, json" });
        }
    }

    [HttpGet("progress")]
    public async Task<IActionResult> ExportProgress([FromQuery] int? queueId = null, [FromQuery] string format = "csv")
    {
        var query = _context.UserProgresses
            .Include(p => p.Queue)
            .Include(p => p.User)
            .AsQueryable();

        if (queueId.HasValue)
        {
            query = query.Where(p => p.QueueId == queueId.Value);
        }

        var progressList = await query
            .OrderBy(p => p.QueueId)
            .ThenBy(p => p.UserId)
            .ToListAsync();

        if (format.ToLower() == "csv")
        {
            var csv = new StringBuilder();
            csv.AppendLine("队列ID,队列名称,用户ID,用户名,已完成,总计,进度百分比,最后更新");

            foreach (var progress in progressList)
            {
                var percentage = progress.TotalGroups > 0 ? (double)progress.CompletedGroups / progress.TotalGroups * 100 : 0;
                csv.AppendLine($"{progress.QueueId},{progress.Queue.Name},{progress.UserId},{progress.User.Username},{progress.CompletedGroups},{progress.TotalGroups},{percentage:F2}%,{progress.LastUpdated:yyyy-MM-dd HH:mm:ss}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            var fileName = $"progress_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            
            return File(bytes, "text/csv", fileName);
        }
        else if (format.ToLower() == "json")
        {
            var data = progressList.Select(p => new
            {
                QueueId = p.QueueId,
                QueueName = p.Queue.Name,
                UserId = p.UserId,
                Username = p.User.Username,
                CompletedGroups = p.CompletedGroups,
                TotalGroups = p.TotalGroups,
                ProgressPercentage = p.TotalGroups > 0 ? (double)p.CompletedGroups / p.TotalGroups * 100 : 0,
                LastUpdated = p.LastUpdated
            }).ToList();

            return Ok(data);
        }
        else
        {
            return BadRequest(new { message = "不支持的导出格式。支持的格式：csv, json" });
        }
    }
}

