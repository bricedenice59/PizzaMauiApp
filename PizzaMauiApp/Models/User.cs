using System.ComponentModel.DataAnnotations;
using PizzaMauiApp.Helpers.Validation;

namespace PizzaMauiApp.Models;

public partial class User : ObservableObject
{
    [ObservableProperty] 
    private ValidatableObject<string> _email = new();
    
    [ObservableProperty]
    private ValidatableObject<string> _password = new();
    
    public string? Token { get; set; }

    public void ResetValues()
    {
        Email.Value = null;
        Password.Value = null;
        Token = null;

        Email.IsValid = true;
        Password.IsValid = true;

        Email.Errors = Array.Empty<string>();
        Password.Errors = Array.Empty<string>();
    }
}