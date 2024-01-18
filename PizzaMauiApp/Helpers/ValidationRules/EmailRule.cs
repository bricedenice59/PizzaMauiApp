using System.Net.Mail;
using System.Text.RegularExpressions;

namespace PizzaMauiApp.Helpers.ValidationRules;

public class EmailRule<T> : IValidationRule<T>
{
    public string? ValidationMessage { get; set; }

    public bool Check(T? value)
    {
        string? emailAddress = null;
        if (value is not null)
            emailAddress = value as string;

        if(string.IsNullOrEmpty(emailAddress))
            return false;
     
        try
        {
            var address = new MailAddress(emailAddress);
            return string.IsNullOrEmpty(address.DisplayName);
        }
        catch (FormatException)
        {
            // address is invalid
        }

        return false;
    }
}