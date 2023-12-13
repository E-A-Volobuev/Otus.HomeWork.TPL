using AutoMapper;
using GrpcWithDbService.Entities;
using Service.Contracts;

namespace GrpcWithDbService.Mapping;

/// <summary>
/// преобразование дто в сущость базы данных
/// </summary>
public class CalcResultEntityMappingProfile : Profile
{
    public CalcResultEntityMappingProfile() 
    {
        CreateMap<CalcResultEntity, SumResultDto>();
        CreateMap<SumResultDto, CalcResultEntity>().ForMember(target => target.Id, source => source.Ignore());
    }
}
