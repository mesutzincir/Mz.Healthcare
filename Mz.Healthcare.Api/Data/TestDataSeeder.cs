using Mz.Healthcare.Api.Models.Entities;

namespace Mz.Healthcare.Api.Data;

public static class TestDataSeeder
{
    /// <summary>
    ///     seed database with test data. Data was created by ChatGpt.
    /// </summary>
    /// <param name="dbContext"></param>
    public static void Seed(AppDbContext dbContext)
    {
        if (dbContext.Patients.Any()) return; // Already seeded

        var patients = new List<PatientEntity>
        {
            new()
            {
                NHSNumber = "NHS001", Name = "John Doe", DateOfBirth = new DateTime(1980, 1, 1),
                GPPractice = "Central GP"
            },
            new()
            {
                NHSNumber = "NHS002", Name = "Jane Smith", DateOfBirth = new DateTime(1990, 2, 2),
                GPPractice = "West GP"
            },
            new()
            {
                NHSNumber = "NHS003", Name = "Michael Brown", DateOfBirth = new DateTime(1975, 3, 3),
                GPPractice = "East GP"
            },
            new()
            {
                NHSNumber = "NHS004", Name = "Emily Johnson", DateOfBirth = new DateTime(1985, 4, 4),
                GPPractice = "North GP"
            },
            new()
            {
                NHSNumber = "NHS005", Name = "Daniel Williams", DateOfBirth = new DateTime(1995, 5, 5),
                GPPractice = "Central GP"
            },
            new()
            {
                NHSNumber = "NHS006", Name = "Sophia Jones", DateOfBirth = new DateTime(2000, 6, 6),
                GPPractice = "West GP"
            },
            new()
            {
                NHSNumber = "NHS007", Name = "David Miller", DateOfBirth = new DateTime(1970, 7, 7),
                GPPractice = "East GP"
            },
            new()
            {
                NHSNumber = "NHS008", Name = "Olivia Davis", DateOfBirth = new DateTime(1992, 8, 8),
                GPPractice = "North GP"
            },
            new()
            {
                NHSNumber = "NHS009", Name = "James Garcia", DateOfBirth = new DateTime(1988, 9, 9),
                GPPractice = "Central GP"
            },
            new()
            {
                NHSNumber = "NHS010", Name = "Emma Martinez", DateOfBirth = new DateTime(1998, 10, 10),
                GPPractice = "West GP"
            }
        };

        dbContext.Patients.AddRange(patients);
        dbContext.SaveChanges();
    }
}