using System.ComponentModel.DataAnnotations;
using PizzaMauiApp.Helpers.Validation;
using PizzaMauiApp.Helpers.ValidationRules;

namespace PizzaMauiApp.Models;

public partial class UserModel : ObservableObject
{
    [ObservableProperty] 
    private ValidatableObject<string> _email = new();
    
    [ObservableProperty]
    private ValidatableObject<string> _password = new();

    public void Init(bool isLogin)
    {
        Email = new ValidatableObject<string>();
        Password = new ValidatableObject<string>();
        AddValidations(isLogin);
    }
    
    private void AddValidations(bool isLogin)
    {
        Email.Validations.Add(new EmailRule<string> 
        { 
            ValidationMessage = "Email is not valid" 
        });

        //signup
        if (!isLogin)
        {
            Password.Validations.Add(new PasswordRule<string>
            { 
                ValidationMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 10 characters." 
            });
        }
        else
        {
            Password.Validations.Add(new StringNotEmptyRule<string>
            { 
                ValidationMessage = "Password must not be empty." 
            });
        }
    }
}