using System.Net.Mail;
using System.Text.RegularExpressions;

namespace PizzaMauiApp.Helpers.ValidationRules;

public class StringNotEmptyRule<T> : IValidationRule<T>
{
    public string? ValidationMessage { get; set; }

    public bool Check(T? value) => 
        value != null && 
        value is string && 
        !string.IsNullOrEmpty(value.ToString());
}