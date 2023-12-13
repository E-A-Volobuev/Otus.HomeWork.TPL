using ConsoleClientApp.Extensions.Constants;
using ConsoleClientApp.Extensions.Exceptions;
using ConsoleClientApp.Services.Abstractions;
using Grpc.Net.Client;

namespace ConsoleClientApp.Services.Implementations;

internal sealed class GrpcService: IGrpcService
{
    /// <summary>
    /// отправляем массив чисел через grpc в микросервис GrpcMathService и в ответ получаем результат суммы чисел этого массива
    /// </summary>
    /// <param name="numbers"></param>
    /// <returns></returns>
    /// <exception cref="GrpcException"></exception>
    public async Task<ListResultReply> SendListResultsAsync(List<int> numbers)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(ConsoleConstants.UrlServerByMathAction);
            var client = new Translator.TranslatorClient(channel);

            RequestInput input = new RequestInput();
            input.Numbers.AddRange(numbers);

            return await client.TranslateAsync(input);
        }
        catch (Exception ex)
        {
            throw new GrpcException(ex.Message);
        }
    }
    /// <summary>
    /// отправляем название команды для базы данных в микросервис GrpcWithDbService и в ответ получаем историю измерений 
    /// </summary>
    /// <param name="nameAction"></param>
    /// <returns></returns>
    /// <exception cref="GrpcException"></exception>
    public async Task<ListResultReply> GetDbActionResultsAsync(string nameAction)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(ConsoleConstants.UrlServerByDb);
            var client = new Translator.TranslatorClient(channel);

            RequestMessage message = new RequestMessage();
            message.NameAction = nameAction;

            return await client.TransleateMessageToDbAsync(message);
        }
        catch (Exception ex)
        {
            throw new GrpcException(ex.Message);
        }
    }
}
