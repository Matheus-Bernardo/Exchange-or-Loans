using System.ComponentModel.DataAnnotations;

namespace ExchangeOrLoans.models;

public class Product
{
    [Key]
    public required int IdProduct  { get; set; }
    public required int IdUserSeller  { get; set; }
    public required int IdUserBuyer { get; set; }
    public required float Price  { get; set; }
    public required int IdStatusProduct  { get; set; }
    public required float QuantityStock  { get; set; }
    public string ImageUrl  { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

}