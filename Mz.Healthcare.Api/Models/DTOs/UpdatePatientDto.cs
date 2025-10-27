namespace Mz.Healthcare.Api.Models.DTOs;

public class UpdatePatientDto : CreatePatientDto
{
    public bool IsActive { get; set; }
}