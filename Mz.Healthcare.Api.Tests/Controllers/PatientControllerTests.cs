using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Mz.Healthcare.Api.Controllers;
using Mz.Healthcare.Api.Models.DTOs;
using Mz.Healthcare.Api.Services;

namespace Mz.Healthcare.Api.Tests.Controllers;

public class PatientControllerTests
{
    private PatientController _controller;
    private Mock<IPatientService> _patientServiceMock;

    [SetUp]
    public void Setup()
    {
        _patientServiceMock = new Mock<IPatientService>();
        _controller = new PatientController(_patientServiceMock.Object);
    }

    [Test]
    public async Task GetById_ShouldReturnPatient_WhenFound()
    {
        // Arrange
        var expectedPatient = new PatientDto
        {
            NHSNumber = "NHSNumber",
            Name = "Mesut",
            DateOfBirth = new DateTime(1980, 1, 1),
            GPPractice = "GPPractice",
            Id = 10,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _patientServiceMock.Setup(s => s.GetByIdAsync(expectedPatient.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPatient);

        // Act
        var result = await _controller.GetById(expectedPatient.Id, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        var okResult = result.Result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult!.Value.Should().BeEquivalentTo(expectedPatient);
    }

    [Test]
    public async Task GetById_ShouldReturnNotFound_WhenPatientDoesNotExist()
    {
        // Arrange
        const int patientId = 10;
        _patientServiceMock.Setup(s => s.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PatientDto?)null);

        // Act
        var result = await _controller.GetById(patientId, CancellationToken.None);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnPatients()
    {
        // Arrange
        var expectedPatients = new List<PatientDto>
        {
            new()
            {
                NHSNumber = "NHSNumber1",
                Name = "Mesut1",
                DateOfBirth = new DateTime(1980, 1, 1),
                GPPractice = "GPPractice",
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new()
            {
                NHSNumber = "NHSNumber2",
                Name = "Mesut2",
                DateOfBirth = new DateTime(1982, 1, 1),
                GPPractice = "GPPractice",
                Id = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            }
        };

        _patientServiceMock
            .Setup(s => s.GetAllAsync("mes", "name", true, 1, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPatients);

        // Act
        var result = await _controller.GetAllAsync("mes", "name", true, 1, 5, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeEquivalentTo(expectedPatients);
        _patientServiceMock.Verify(s => s.GetAllAsync("mes", "name", true, 1, 5, It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Test]
    public async Task Post_ShouldReturnCreatedPatient()
    {
        // Arrange
        var createDto = new CreatePatientDto
        {
            NHSNumber = "NHS001",
            Name = "Mesut Z",
            DateOfBirth = new DateTime(1980, 1, 1),
            GPPractice = "Central GP"
        };
        var createdPatient = new PatientDto
        {
            Id = 1,
            NHSNumber = "NHS001",
            Name = "Mesut Z",
            DateOfBirth = new DateTime(1980, 1, 1),
            GPPractice = "Central GP"
        };

        _patientServiceMock
            .Setup(s => s.CreateAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdPatient);

        // Act
        var result = await _controller.Post(createDto, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        var createdAt = result.Result as CreatedAtActionResult;
        createdAt.Should().NotBeNull();
        createdAt.Value.Should().BeEquivalentTo(createdPatient);
        createdAt.StatusCode.Should().Be(201);
    }

    [Test]
    public async Task Update_ShouldReturnNoContent()
    {
        // Arrange
        const int patientId = 1;
        var updateDto = new UpdatePatientDto
        {
            NHSNumber = "NHS001",
            Name = "Mesut Z",
            DateOfBirth = new DateTime(1980, 1, 1),
            GPPractice = "Central GP",
            IsActive = true
        };

        _patientServiceMock.Setup(s => s.UpdateAsync(patientId, updateDto, It.IsAny<CancellationToken>()));

        // Act
        var result = await _controller.Update(patientId, updateDto, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<NoContentResult>();
        ((NoContentResult)result).StatusCode.Should().Be(204);
        _patientServiceMock.Verify(s => s.UpdateAsync(1, updateDto, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Delete_ShouldReturnOk_WhenPatientExists()
    {
        // Arrange
        const int patientId = 1;
        var deletedPatient = new PatientDto
        {
            Id = patientId,
            NHSNumber = "NHS001",
            Name = "Mesut Z",
            DateOfBirth = new DateTime(1980, 1, 1),
            GPPractice = "Central GP"
        };
        _patientServiceMock.Setup(s => s.DeleteAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(deletedPatient);

        // Act
        var result = await _controller.Delete(patientId, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(deletedPatient);
        okResult.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task Delete_ShouldReturnNotFound_WhenPatientDoesNotExist()
    {
        // Arrange
        const int patientId = 111;
        _patientServiceMock.Setup(s => s.DeleteAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PatientDto?)null);

        // Act
        var result = await _controller.Delete(patientId, CancellationToken.None);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }
}