using Microsoft.AspNetCore.Mvc;
using Mz.Healthcare.Api.Models.DTOs;
using Mz.Healthcare.Api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mz.Healthcare.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController(IPatientService patientService) : ControllerBase
{
    // GET: api/<PatientController>
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
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IEnumerable<PatientDto>> GetAllAsync(
        string? name,
        string? sortBy,
        bool ascending,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        return await patientService.GetAllAsync(name, sortBy, ascending, pageNumber, pageSize, cancellationToken);
    }

    // GET api/<PatientController>/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PatientDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var patient = await patientService.GetByIdAsync(id, cancellationToken);
        if (patient is null)
            return NotFound();

        return Ok(patient);
    }

    // POST api/<PatientController>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PatientDto>> Post(CreatePatientDto dto, CancellationToken cancellationToken)
    {
        var patient = await patientService.CreateAsync(dto, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }


    // PUT api/<PatientController>/5
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, UpdatePatientDto dto, CancellationToken cancellationToken)
    {
        await patientService.UpdateAsync(id, dto, cancellationToken);

        return NoContent();
    }

    // DELETE api/<PatientController>/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PatientDto>> Delete(int id, CancellationToken cancellationToken)
    {
        var patient = await patientService.DeleteAsync(id, cancellationToken);
        if (patient is null)
            return NotFound();

        return Ok(patient);
    }
}