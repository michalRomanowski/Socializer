using Socializer.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Socializer.BlazorHybrid.UIModels;

internal class CreateUserUI : CreateUserDto
{
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}
