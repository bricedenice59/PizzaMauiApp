namespace PizzaMauiApp.Helpers.API;

public class ApiException<T> : ApiResponse<T>
{
    public string? ExceptionMessage { get; set; }
    public string? StackTrace { get; set; }
}