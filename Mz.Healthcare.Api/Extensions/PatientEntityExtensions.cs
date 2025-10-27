using Mz.Healthcare.Api.Models.DTOs;
using Mz.Healthcare.Api.Models.Entities;

namespace Mz.Healthcare.Api.Extensions;

public static class PatientEntityExtensions
{
    /// <summary>
    ///     create PatientDto instance from patientEntity.
    ///     AutoMapper may be used for bigger project but this is ok for this project.
    /// </summary>
    /// <param name="patientEntity"></param>
    /// <returns>PatientDto instance</returns>
    public static PatientDto MapToPatientDto(this PatientEntity patientEntity)
    {
        return new PatientDto
        {
            NHSNumber = patientEntity.NHSNumber,
            Name = patientEntity.Name,
            DateOfBirth = patientEntity.DateOfBirth,
            GPPractice = patientEntity.GPPractice,
            Id = patientEntity.Id,
            CreatedAt = patientEntity.CreatedAt,
            UpdatedAt = patientEntity.UpdatedAt,
            IsActive = patientEntity.IsActive
        };
    }
}