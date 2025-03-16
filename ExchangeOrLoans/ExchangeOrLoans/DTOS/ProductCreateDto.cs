using System.ComponentModel.DataAnnotations;
namespace ExchangeOrLoans.Dtos;

public class ProductCreateDto
{
    
    public string? Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public int IdUserSeller { get; set; }

    public int? IdUserBuyer { get; set; }

    [Required]
    public float Price { get; set; }

    public int? IdStatusProduct { get; set; } = 1;

    [Required]
    public float QuantityStock { get; set; }

    public IFormFile? Image { get; set; } 
}