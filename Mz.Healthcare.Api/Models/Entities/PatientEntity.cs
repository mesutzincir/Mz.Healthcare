using System.ComponentModel.DataAnnotations;

namespace Mz.Healthcare.Api.Models.Entities;

public class PatientEntity
{
    public int Id { get; set; }

    [Required, StringLength(20)]
    public required string NHSNumber { get; set; }
    
    [Required, StringLength(100)]
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string GPPractice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
}