namespace PizzaMauiApp.Models;

public class Pizza : BaseModel
{
    public required string Name { get; set; }
    public required string Image { get; set; }
    public required double Price { get; set; }
    
    public required string Description { get; set; }
    
    public required string Ingredients { get; set; }
}