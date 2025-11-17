using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectsController(AppDbContext context)
    {
        _context = context;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim!);
    }

    private bool IsAdmin()
    {
        return User.IsInRole("Admin");
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetProjects()
    {
        var projects = await _context.Projects
            .Include(p => p.CreatedBy)
            .Include(p => p.Queues)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedById = p.CreatedById,
                CreatedByUsername = p.CreatedBy.Username,
                CreatedAt = p.CreatedAt,
                QueueCount = p.Queues.Count
            })
            .ToListAsync();

        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(int id)
    {
        var project = await _context.Projects
            .Include(p => p.CreatedBy)
            .Include(p => p.Queues)
            .Where(p => p.Id == id)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedById = p.CreatedById,
                CreatedByUsername = p.CreatedBy.Username,
                CreatedAt = p.CreatedAt,
                QueueCount = p.Queues.Count
            })
            .FirstOrDefaultAsync();

        if (project == null)
        {
            return NotFound(new { message = "项目不存在" });
        }

        return Ok(project);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] CreateProjectDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetUserId();

        var project = new Project
        {
            Name = createDto.Name,
            Description = createDto.Description,
            CreatedById = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        var projectDto = await _context.Projects
            .Include(p => p.CreatedBy)
            .Where(p => p.Id == project.Id)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedById = p.CreatedById,
                CreatedByUsername = p.CreatedBy.Username,
                CreatedAt = p.CreatedAt,
                QueueCount = 0
            })
            .FirstAsync();

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, projectDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProjectDto>> UpdateProject(int id, [FromBody] UpdateProjectDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound(new { message = "项目不存在" });
        }

        project.Name = updateDto.Name;
        project.Description = updateDto.Description;

        await _context.SaveChangesAsync();

        var projectDto = await _context.Projects
            .Include(p => p.CreatedBy)
            .Include(p => p.Queues)
            .Where(p => p.Id == id)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedById = p.CreatedById,
                CreatedByUsername = p.CreatedBy.Username,
                CreatedAt = p.CreatedAt,
                QueueCount = p.Queues.Count
            })
            .FirstAsync();

        return Ok(projectDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound(new { message = "项目不存在" });
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

