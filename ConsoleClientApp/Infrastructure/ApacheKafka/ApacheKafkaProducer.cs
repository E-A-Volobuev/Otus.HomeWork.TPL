using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
namespace ConsoleClientApp.Infrastructure.ApacheKafka;

internal sealed class ApacheKafkaProducer
{
    private ProducerConfig _config;

    public ApacheKafkaProducer(IConfiguration configuration)
    {
        _config = GetKafkaProducerConfig(configuration);
    }
    /// <summary>
    /// пишем сообщение в топик кафки
    /// </summary>
    /// <param name="topicName"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task ProduceAsync(string topicName, string message)
    {
        using (var producer = new ProducerBuilder<string, string>(_config).Build())
        {
            await producer.ProduceAsync(topicName,
                new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = message
                });
        }
    }
    /// <summary>
    /// конфиги для подключения к кафке
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private ProducerConfig GetKafkaProducerConfig(IConfiguration configuration)
    {
        KafkaSettings kafkaSettings = configuration.Get<AMQPSettings>().KafkaSettings;
        ProducerConfig config = new ()
        {
            BootstrapServers = kafkaSettings.BootstrapServers,
        };
        return config;
    }
}
