using PeterParker.Data.DTOs;
using PeterParker.Data.Models;

namespace PeterParker.DTOs;

public class UserLoginDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
