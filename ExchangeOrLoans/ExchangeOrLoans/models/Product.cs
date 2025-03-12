using System.ComponentModel.DataAnnotations;

namespace ExchangeOrLoans.models;

public class Product
{
    [Key]
    public  int IdProduct  { get; set; }
    public required string? Name { get; set; }
    public string? Description { get; set; }
    public required int IdUserSeller  { get; set; }
    public int? IdUserBuyer { get; set; }
    public required float? Price  { get; set; }
    public  int? IdStatusProduct { get; set; } = 1;
    public required float? QuantityStock  { get; set; }
    public string? ImageUrl { get; set; } = null;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateAt { get; set; } = DateTime.UtcNow;

}