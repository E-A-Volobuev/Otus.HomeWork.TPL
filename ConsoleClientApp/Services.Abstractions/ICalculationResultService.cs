using Service.Contracts;

namespace ConsoleClientApp.Services.Abstractions;

internal interface ICalculationResultService
{
     List<SumResultDto> ConvertCalcResult(ListResultReply reply);
}
