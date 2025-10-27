using Microsoft.EntityFrameworkCore;
using Mz.Healthcare.Api.Data;
using Mz.Healthcare.Api.Extensions;
using Mz.Healthcare.Api.Models.DTOs;
using Mz.Healthcare.Api.Models.Entities;

namespace Mz.Healthcare.Api.Services;

/// <summary>
///     inquiries and CRUD for patient
/// </summary>
/// <param name="dbContext"></param>
/// <param name="logger"></param>
public class PatientService(AppDbContext dbContext, ILogger<PatientService> logger) : IPatientService
{
    /// <inheritdoc cref="IPatientService.GetByIdAsync" />
    public async Task<PatientDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var patientEntity = await dbContext.Patients.FindAsync([id], cancellationToken);

        return patientEntity?.MapToPatientDto();
    }

    /// <inheritdoc cref="IPatientService.GetAllAsync" />
    public async Task<IEnumerable<PatientDto>> GetAllAsync(
        string? name,
        string? sortBy,
        bool ascending,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        const int defaultPageNumber = 1;
        const int defaultPageSize = 5;
        pageNumber = pageNumber <= 0 ? defaultPageNumber : pageNumber;
        pageSize = pageSize <= 0 ? defaultPageSize : pageSize;

        var query = dbContext.Patients.AsNoTracking().AsQueryable();

        // add search criteria
        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));

        // add sort section
        query = sortBy?.ToLower() switch
        {
            "id" => ascending ? query.OrderBy(p => p.Id) : query.OrderByDescending(p => p.Id),
            "name" => ascending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name),
            "dateofbirth" => ascending
                ? query.OrderBy(p => p.DateOfBirth)
                : query.OrderByDescending(p => p.DateOfBirth),
            "createdat" => ascending ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderBy(p => p.Id) // default sorting
        };

        // Pagination
        var skip = (pageNumber - 1) * pageSize;
        query = query.Skip(skip).Take(pageSize);

        // execute the query to get patient
        var entities = await query.ToListAsync(cancellationToken);

        var dtos = new List<PatientDto>();
        entities.ForEach(patientEntity => dtos.Add(patientEntity.MapToPatientDto()));

        return dtos;
    }

    /// <inheritdoc cref="IPatientService.CreateAsync" />
    public async Task<PatientDto> CreateAsync(CreatePatientDto dto, CancellationToken cancellationToken)
    {
        var patientEntity = new PatientEntity
        {
            NHSNumber = dto.NHSNumber,
            Name = dto.Name,
            DateOfBirth = dto.DateOfBirth,
            GPPractice = dto.GPPractice,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await dbContext.Patients.AddAsync(patientEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return patientEntity.MapToPatientDto();
    }

    /// <inheritdoc cref="IPatientService.UpdateAsync" />
    public async Task UpdateAsync(int id, UpdatePatientDto dto, CancellationToken cancellationToken)
    {
        var patientEntity = await dbContext.Patients.FindAsync([id], cancellationToken);
        if (patientEntity is not null)
        {
            patientEntity.UpdatedAt = DateTime.UtcNow;
            patientEntity.NHSNumber = dto.NHSNumber;
            patientEntity.Name = dto.Name;
            patientEntity.DateOfBirth = dto.DateOfBirth;
            patientEntity.GPPractice = dto.GPPractice;
            patientEntity.IsActive = dto.IsActive;

            await dbContext.SaveChangesAsync(cancellationToken);
        }
        // should DataNotFound exception be thrown or not? --> It is business decision and no need to throw exception this time.
    }

    /// <inheritdoc cref="IPatientService.DeleteAsync" />
    public async Task<PatientDto?> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var patientEntity = await dbContext.Patients.FindAsync([id], cancellationToken);
        if (patientEntity is null) return null;

        dbContext.Patients.Remove(patientEntity);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Patient id :{PatientId} - {PatientName} was removed.", patientEntity.Id,
            patientEntity.Name);

        return patientEntity.MapToPatientDto();
    }
}