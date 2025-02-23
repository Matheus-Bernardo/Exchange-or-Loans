using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeOrLoans.models;

public class UserEvaluation
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto incremento
    public int IdEvaluation { get; set; }
    [Required]
    public int IdUserReviewer { get; set; }
    [Required]
    public int IdUserRated { get; set; }
    [Required]
    public required int  EvaluatedScore { get; set; }
    public string Comments { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


}