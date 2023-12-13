using GrpcWithDbService.Entities;
using GrpcWithDbService.Infrastructure.EntityFramework;
using GrpcWithDbService.Service.Abstractions;

namespace GrpcWithDbService.Infrastructure.Repositories;

/// <summary>
/// репозиторий для crud операций объектов, получаемых с микросервиса клиента ConsoleClientApp
/// </summary>
public class CalcResultRepository: EFGenericRepository<CalcResultEntity>,ICalcResultRepository
{
    public CalcResultRepository(EFCoreDbContext context) : base(context) {}
}
