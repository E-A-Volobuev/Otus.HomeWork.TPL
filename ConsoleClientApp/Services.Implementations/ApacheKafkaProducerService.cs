using ConsoleClientApp.Extensions.Constants;
using ConsoleClientApp.Infrastructure.ApacheKafka;
using ConsoleClientApp.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Service.Contracts;

namespace ConsoleClientApp.Services.Implementations;

internal sealed class ApacheKafkaProducerService: IApacheKafkaProducerService
{
    /// <summary>
    /// пишем сообщение в топик кафки
    /// </summary>
    /// <param name="dtoList"></param>
    /// <returns></returns>
    public async Task ProduceStartAsync(List<SumResultDto> dtoList)
    {
        IConfiguration configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .Build();
        ApacheKafkaProducer producer = new ApacheKafkaProducer(configuration);
        await producer.ProduceAsync(KafkaConstants.TopicName, $"{GetJsonMessage(dtoList)}");
    }

    private string GetJsonMessage(List<SumResultDto> dtoList)
    {
        return JsonConvert.SerializeObject(dtoList, Formatting.Indented);
    }
}
