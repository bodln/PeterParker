using PeterParker.Data.Models;

namespace PeterParker.DTOs;

public class UserDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<string> Vehicles { get; set; } = new List<string>();
}
