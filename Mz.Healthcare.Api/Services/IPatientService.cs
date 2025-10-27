using Mz.Healthcare.Api.Models.DTOs;

namespace Mz.Healthcare.Api.Services;

public interface IPatientService
{
    /// <summary>
    ///     get patient by id
    /// </summary>
    /// <param name="id">patient id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Patient record.</returns>
    Task<PatientDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     search patient by name.
    /// </summary>
    /// <param name="name">patient name to search</param>
    /// <param name="sortBy" example="name or id, or dateofbirth or createdat">fields name to sort</param>
    /// <param name="ascending">sort direction, true:asc, false: desc</param>
    /// <param name="pageNumber" example="1 or 2">page number, default 1</param>
    /// <param name="pageSize" example="5 or 10 or 20">max record count in page, default 5</param>
    /// <param name="cancellationToken"></param>
    /// <returns>patients which is found by name</returns>
    public Task<IEnumerable<PatientDto>> GetAllAsync(
        string? name,
        string? sortBy,
        bool ascending,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    /// <summary>
    ///     add new patient
    /// </summary>
    /// <param name="dto">CreatePatientDto instance with new patient data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>new patient data with id</returns>
    Task<PatientDto> CreateAsync(CreatePatientDto dto, CancellationToken cancellationToken);

    /// <summary>
    /// </summary>
    /// <param name="id">patient id</param>
    /// <param name="dto">UpdatePatientDto instance with new values</param>
    /// <param name="cancellationToken"></param>
    Task UpdateAsync(int id, UpdatePatientDto dto, CancellationToken cancellationToken);

    /// <summary>
    ///     delete patient from db by id and return deleted record.
    /// </summary>
    /// <param name="id">patient id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>removed patient data</returns>
    Task<PatientDto?> DeleteAsync(int id, CancellationToken cancellationToken);
}