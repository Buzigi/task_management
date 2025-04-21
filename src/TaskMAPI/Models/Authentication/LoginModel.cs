using System;
using System.ComponentModel.DataAnnotations;

namespace TaskMAPI.Models.Authentication;

public class LoginModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
