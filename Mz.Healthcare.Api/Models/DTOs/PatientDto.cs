namespace Mz.Healthcare.Api.Models.DTOs;

public class PatientDto : CreatePatientDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}