using System.Text.RegularExpressions;

namespace PizzaMauiApp.Helpers.ValidationRules;

public class PasswordRule<T> : IValidationRule<T>
{
    //regexlib.com
    private readonly Regex _regex = new(@"(?=^.{10,60}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$");

    public string? ValidationMessage { get; set; }

    public bool Check(T? value) =>
        value is not null  && _regex.IsMatch(value as string ?? string.Empty);
}