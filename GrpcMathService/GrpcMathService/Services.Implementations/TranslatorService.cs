using Grpc.Core;
using GrpcMathService.Services.Abstractions;

namespace GrpcMathService.Services
{
    internal sealed class TranslatorService : Translator.TranslatorBase
    {
        private readonly ILogger<TranslatorService> _logger;
        private readonly IListResultReplyService _service;
        public TranslatorService(ILogger<TranslatorService> logger, IListResultReplyService service)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }
        /// <summary>
        /// получаем сообщение через gRPC от микросервиса -клиента ConsoleClientApp с массивом цифр,которые ввёл пользователь, и в ответ отправляем результат вычислений
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ListResultReply> Translate(RequestInput request, ServerCallContext context)
        {
            ListResultReply resultList = new();
            try
            {
                resultList = _service.GetListResults(request.Numbers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Path: {ex.StackTrace}  Time:{DateTime.Now.ToLongTimeString()} " + $"Detail :{ex.Message}");
            }

            return Task.FromResult(resultList);
        }
    }
}