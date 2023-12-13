using GrpcWithDbService.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrpcWithDbService.Infrastructure.EntityFramework;

public class EFCoreDbContext : DbContext
{
    public DbSet<CalcResultEntity> CalcResultEntities { get; set; }

    public EFCoreDbContext(DbContextOptions<EFCoreDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
