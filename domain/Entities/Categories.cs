using AuctionSystem.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryDescription { get; set; } = string.Empty;
        
    public ICollection<Project> Projects { get; set; } = new List<Project>();

    }