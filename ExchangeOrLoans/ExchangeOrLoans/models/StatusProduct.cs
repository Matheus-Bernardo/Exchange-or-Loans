namespace ExchangeOrLoans.models;

public class StatusProduct
{
    public required int Id { get; set; }
    public required string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
}