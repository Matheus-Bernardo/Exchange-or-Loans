namespace ExchangeOrLoans.models;
public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required int Score { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}