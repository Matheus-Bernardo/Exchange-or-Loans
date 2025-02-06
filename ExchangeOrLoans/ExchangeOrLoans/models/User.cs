using System.Text.Json.Serialization;

namespace ExchangeOrLoans.models;
public class User
{
    public int Id { get; set; }
    public   string? Username { get; set; }
    public   string Email { get; set; }
    public   string? FirstName { get; set; }
    public  string LastName { get; set; }
    [JsonIgnore]
    public  string? Password { get; set; }
    public  int? Score { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}