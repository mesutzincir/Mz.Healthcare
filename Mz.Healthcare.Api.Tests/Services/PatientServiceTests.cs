using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Mz.Healthcare.Api.Data;
using Mz.Healthcare.Api.Models.DTOs;
using Mz.Healthcare.Api.Models.Entities;
using Mz.Healthcare.Api.Services;

namespace Mz.Healthcare.Api.Tests.Services;

[TestFixture]
public class PatientServiceTests
{
    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // isolate per test
            .Options;

        _dbContext = new AppDbContext(options);
        _loggerMock = new Mock<ILogger<PatientService>>();
        _service = new PatientService(_dbContext, _loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    private AppDbContext _dbContext = null!;
    private Mock<ILogger<PatientService>> _loggerMock = null!;
    private PatientService _service = null!;

    [Test]
    public async Task GetByIdAsync_ShouldReturnPatient_WhenExists()
    {
        // Arrange
        var patient = new PatientEntity
        {
            Id = 1,
            Name = "John Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            GPPractice = "Clinic A",
            NHSNumber = "12345"
        };
        await _dbContext.Patients.AddAsync(patient);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("John Doe");
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Act
        var result = await _service.GetByIdAsync(10, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnPagedAndSortedResults()
    {
        // Arrange
        var patients = new List<PatientEntity>
        {
            new()
            {
                Id = 1, NHSNumber = "NHS001", Name = "John Doe", DateOfBirth = new DateTime(1980, 1, 1),
                GPPractice = "Central GP"
            },
            new()
            {
                Id = 2, NHSNumber = "NHS002", Name = "Jane Smith", DateOfBirth = new DateTime(1990, 2, 2),
                GPPractice = "West GP"
            },
            new()
            {
                Id = 3, NHSNumber = "NHS003", Name = "Michael Brown", DateOfBirth = new DateTime(1975, 3, 3),
                GPPractice = "East GP"
            }
        };
        await _dbContext.Patients.AddRangeAsync(patients);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetAllAsync(
            null,
            "name",
            true,
            1,
            2,
            CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Jane Smith");
        result.Last().Name.Should().Be("John Doe");

        // get next page
        result = await _service.GetAllAsync(
            null,
            "name",
            true,
            2,
            2,
            CancellationToken.None);

        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Michael Brown");
    }

    [Test]
    public async Task CreateAsync_ShouldAddPatientAndReturnDto()
    {
        // Arrange
        var dto = new CreatePatientDto
        {
            Name = "Alice",
            NHSNumber = "9876",
            GPPractice = "Health Center",
            DateOfBirth = new DateTime(1995, 5, 10)
        };

        // Act
        var result = await _service.CreateAsync(dto, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be(dto.Name);
        result.NHSNumber.Should().Be(dto.NHSNumber);
        result.GPPractice.Should().Be(dto.GPPractice);
        result.DateOfBirth.Should().Be(dto.DateOfBirth);
    }

    [Test]
    public async Task UpdateAsync_ShouldModifyPatient_WhenExists()
    {
        // Arrange
        var patient = new PatientEntity
        {
            Id = 1,
            Name = "Original",
            NHSNumber = "111",
            GPPractice = "Old Clinic",
            DateOfBirth = new DateTime(1990, 1, 1),
            IsActive = true
        };
        await _dbContext.Patients.AddAsync(patient);
        await _dbContext.SaveChangesAsync();

        var updateDto = new UpdatePatientDto
        {
            Name = "Updated",
            NHSNumber = "222",
            GPPractice = "New Clinic",
            DateOfBirth = new DateTime(1989, 12, 31),
            IsActive = false
        };

        // Act
        await _service.UpdateAsync(patient.Id, updateDto, CancellationToken.None);

        // Assert
        var updated = await _dbContext.Patients.FindAsync(1);
        updated.Should().BeEquivalentTo(updateDto);
    }

    [Test]
    public async Task UpdateAsync_ShouldDoNothing_WhenPatientNotFound()
    {
        // Arrange
        var dto = new UpdatePatientDto
        {
            Name = "DoesNotExist",
            NHSNumber = "000",
            GPPractice = "Nowhere",
            DateOfBirth = new DateTime(1989, 12, 31),
            IsActive = false
        };

        // Act
        await _service.UpdateAsync(999, dto, CancellationToken.None);

        // Assert
        (await _dbContext.Patients.CountAsync()).Should().Be(0);
    }

    [Test]
    public async Task DeleteAsync_ShouldRemovePatientAndReturnDto()
    {
        // Arrange
        var patient = new PatientEntity
        {
            Id = 10,
            Name = "ToDelete",
            NHSNumber = "D123",
            GPPractice = "Clinic",
            DateOfBirth = new DateTime(1991, 1, 1)
        };
        await _dbContext.Patients.AddAsync(patient);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.DeleteAsync(10, CancellationToken.None);

        // Assert
        result.Name.Should().Be(patient.Name);
        result.NHSNumber.Should().Be(patient.NHSNumber);
        result.GPPractice.Should().Be(patient.GPPractice);
        result.DateOfBirth.Should().Be(patient.DateOfBirth);

        var count = await _dbContext.Patients.CountAsync();
        count.Should().Be(0);

        _loggerMock.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("ToDelete")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    public async Task DeleteAsync_ShouldReturnNull_WhenPatientNotFound()
    {
        // Act
        var result = await _service.DeleteAsync(123, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _loggerMock.VerifyNoOtherCalls();
    }
}