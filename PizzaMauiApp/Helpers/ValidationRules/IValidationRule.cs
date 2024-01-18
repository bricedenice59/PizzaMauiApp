namespace PizzaMauiApp.Helpers.ValidationRules;

public interface IValidationRule<T>
{
    string? ValidationMessage { get; set; }
    bool Check(T? value);
}