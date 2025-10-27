using Microsoft.EntityFrameworkCore;
using Mz.Healthcare.Api.Models.Entities;

namespace Mz.Healthcare.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<PatientEntity> Patients { get; set; }
}