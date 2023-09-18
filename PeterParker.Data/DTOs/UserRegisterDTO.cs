using PeterParker.Data.DTOs;
using PeterParker.Data.Models;

namespace PeterParker.DTOs;

public class UserRegisterDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string HomeAddress { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

}
