using Service.Contracts;

namespace ConsoleClientApp.Services.Abstractions;

internal interface IApacheKafkaProducerService
{
    Task ProduceStartAsync(List<SumResultDto> dtoList);
}
