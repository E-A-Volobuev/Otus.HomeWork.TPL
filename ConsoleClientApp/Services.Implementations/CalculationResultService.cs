using ConsoleClientApp.Extensions.Constants;
using ConsoleClientApp.Extensions.Exceptions;
using ConsoleClientApp.Services.Abstractions;
using Service.Contracts;

namespace ConsoleClientApp.Services.Implementations;

internal sealed class CalculationResultService: ICalculationResultService
{
    /// <summary>
    /// конвертируем объект , который автоматически создался через protobuf файл grpc в dto
    /// </summary>
    /// <param name="reply"></param>
    /// <returns></returns>
    /// <exception cref="GrpcException"></exception>
    public List<SumResultDto> ConvertCalcResult(ListResultReply reply)
    {
        List<SumResultDto> dtoList = new();
        if (reply.Results.Count > 0)
        {
            foreach (var el in reply.Results)
            {
                if (el != null)
                    dtoList.Add(GetDtoByResponceObj(el));
            }
        }
        else
            throw new GrpcException(ConsoleConstants.RepeatMessage);

        return dtoList;
    }
    private SumResultDto GetDtoByResponceObj(ResultReply reply)
    {
        SumResultDto dto = new()
        {
            Sum = reply.Sum,
            CalculationType = (CalculationType)reply.CalculationType,
            CurrentDateTime = reply.CurrentDateTime.ToDateTime(),
            Duration = reply.Duration.ToTimeSpan(),
        };
        dto.Description = GetDescription(dto);

        return dto;
    }
    private string GetDescription(SumResultDto dto)
    {
       return string.Concat(ConsoleConstants.CalculationType, dto.CalculationType.DisplayName(), "\n",
                            ConsoleConstants.SumElementsByArray, dto.Sum, "\n",
                            ConsoleConstants.DurationCalc, dto.Duration.ToString(), "\n",
                            ConsoleConstants.StartDateTime, dto.CurrentDateTime.ToString("d"), "\n");
    }
}
