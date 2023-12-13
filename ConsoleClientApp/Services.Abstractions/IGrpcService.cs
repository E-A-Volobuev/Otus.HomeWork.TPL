namespace ConsoleClientApp.Services.Abstractions;

internal interface IGrpcService
{
    Task<ListResultReply> SendListResultsAsync(List<int> numbers);
    Task<ListResultReply> GetDbActionResultsAsync(string nameAction);
}
