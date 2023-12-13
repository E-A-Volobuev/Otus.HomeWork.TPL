using Confluent.Kafka;
using Newtonsoft.Json;
using Service.Contracts;

namespace GrpcWithDbService.Infrastructure.ApacheKafKa;

internal sealed class ApacheKafkaConsumer
{
    private ConsumerConfig _consumerConfig;
    private IConsumer<string, string> _kafkaConsumer;

    public ApacheKafkaConsumer(IConfiguration configuration, string groupId, string topicName)
    {
        _consumerConfig = GetConsumerConfig(groupId, configuration);
        _kafkaConsumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();
        var topics = new List<string>() { topicName };
        _kafkaConsumer.Subscribe(topics);
    }
    /// <summary>
    /// запуск запроса к топику кафки для получения данных
    /// </summary>
    /// <param name="dtoList"></param>
    /// <returns></returns>
    public List<SumResultDto> Consume(List<SumResultDto> dtoList)
    {
        //задержка,чтобы уменьшить количество обращений к брокеру
        var consumeResult = _kafkaConsumer.Consume(TimeSpan.FromSeconds(10));
        if (consumeResult != null)
        {
            if(consumeResult.Message!= null)
            {
                dtoList=JsonConvert.DeserializeObject<List<SumResultDto>>(consumeResult.Message.Value);
            }
        }
        return dtoList;
    }

    private static ConsumerConfig GetConsumerConfig(string groupId, IConfiguration configuration)
    {
        KafkaSettings kafkaSettings = configuration.Get<AMQPSettings>().KafkaSettings;
        ConsumerConfig config = new ()
        {
            BootstrapServers = kafkaSettings.BootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };
        return config;
    }
}
