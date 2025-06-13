using Socializer.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Socializer.BlazorHybrid.UIModels;

internal class LoginViewModel
{
    [Required]
    public string Username { get; set; }

    [Required, Password]
    public string Password { get; set; }
}
