namespace Mz.Healthcare.Api.Models.DTOs;

public class CreatePatientDto
{
    public required string NHSNumber { get; set; }
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string GPPractice { get; set; }
}