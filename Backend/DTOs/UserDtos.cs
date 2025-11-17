using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class ApproveUserDto
{
    [Required(ErrorMessage = "用户ID是必需的")]
    public int UserId { get; set; }
}
