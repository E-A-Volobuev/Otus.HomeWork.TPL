using ConsoleClientApp.Extensions.Constants;
using ConsoleClientApp.Extensions.Exceptions;
using ConsoleClientApp.Services.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Services.Extensions;

namespace ConsoleClientApp;

internal sealed class StartApp : BackgroundService
{
    private readonly IConsoleActionService _consoleAction;
    private readonly ICalculationResultService _calculationResultService;
    private readonly IGrpcService _grpcService;
    private readonly ILogger<StartApp> _logger;
    private readonly IApacheKafkaProducerService _kafkaService;
    public StartApp(IConsoleActionService consoleAction, IGrpcService grpcService,
                    ICalculationResultService calculationResultService,
                    ILogger<StartApp> logger, IApacheKafkaProducerService kafkaService)
    {
        _consoleAction = consoleAction ?? throw new ArgumentNullException(nameof(consoleAction));
        _grpcService = grpcService ?? throw new ArgumentNullException(nameof(grpcService));
        _calculationResultService = calculationResultService ?? throw new ArgumentNullException(nameof(calculationResultService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _kafkaService = kafkaService ?? throw new ArgumentNullException(nameof(kafkaService));
    }
    /// <summary>
    /// запуск всех процессов
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //await _kafkaService.ProduceStartAsync();
        _consoleAction.PrintMessage(ConsoleConstants.AppManual);
        _consoleAction.PrintMessage(string.Empty);
        while (true)
        {
            _consoleAction.PrintMessage(ConsoleConstants.Menu);
            _consoleAction.PrintMessage(ConsoleConstants.SelectAction);
            try
            {
                await MenuHelperAsync(GetByteNumber());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Path: {ex.StackTrace}  Time:{DateTime.Now.ToLongTimeString()} " +
                                 $"Detail :{ex.Message}");
                if (ex is ConsoleInputException consoleEx)
                    _consoleAction.PrintMessage(consoleEx.Message);
            }
        }
    }
    /// <summary>
    /// вывод значений главного меню
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="ConsoleInputException"></exception>
    private async Task MenuHelperAsync(byte number)
    {
        switch (number)
        {
            case 1:
                await StartGameAsync();
                break;
            case 2:
                await GetHistoryAsync();
                break;
            case 3:
                Environment.Exit(0);
                break;
            default:
                throw new ConsoleInputException(ExceptionConstants.ConsoleInputExceptionMessage);
        }
    }
    /// <summary>
    /// запуск получения суммы элементов массива, который введёт пользователь
    /// </summary>
    /// <returns></returns>
    private async Task StartGameAsync()
    {
        _consoleAction.PrintMessage(ConsoleConstants.StartProcess);
        List<int> arrayNums = new();
        bool isNotQuit = true; //пользователь не вышел из ввода цифр
        bool isEndInputNums = false; //пользователь закончил ввод цифр
        while (isNotQuit)
        {
            _consoleAction.PrintMessage(ConsoleConstants.InputNumber);
            try
            {
                string textInput = GetNumberInput(ref isNotQuit, ref isEndInputNums);
                isNotQuit=await GameHelperAsync(isEndInputNums, textInput, arrayNums);
            }
            catch (Exception ex)
            {
                if (ex is ConsoleInputException consoleEx)
                    _consoleAction.PrintMessage(consoleEx.Message);
            }
        }
    }
    /// <summary>
    /// получаем историю измерений замеров времени
    /// </summary>
    /// <returns></returns>
    private async Task GetHistoryAsync()
    {
        ListResultReply responceList = await _grpcService.GetDbActionResultsAsync(GrpcConstants.GetAllDbAction);
        if (responceList.Results.Count == 0)
            _consoleAction.PrintMessage(ConsoleConstants.TryAgain);

        List<SumResultDto> dtoList = _calculationResultService.ConvertCalcResult(responceList);
        _consoleAction.PrintMessage(ConsoleConstants.PrintHistory);
        _consoleAction.PrintMessage(ConsoleConstants.BorderHistory);
        PrintCalcResult(dtoList);
        _consoleAction.PrintMessage(ConsoleConstants.BorderHistory);
    }
    /// <summary>
    /// получаем результат суммы элементов массива с микросервиса GrpcMathService через gRPC
    /// </summary>
    /// <param name="isEndInputNums"></param>
    /// <param name="textInput"></param>
    /// <param name="arrayNums"></param>
    /// <returns></returns>
    private async Task<bool> GameHelperAsync(bool isEndInputNums, string textInput, List<int> arrayNums)
    {
        if (isEndInputNums)
        {
            ListResultReply responceList = await _grpcService.SendListResultsAsync(arrayNums);
            if(responceList.Results.Count==0)
            {
                _consoleAction.PrintMessage(ConsoleConstants.TryAgain);
                return false;
            }
            List<SumResultDto> dtoList = _calculationResultService.ConvertCalcResult(responceList);
            PrintCalcResult(dtoList);
            _consoleAction.PrintMessage(ConsoleConstants.SaveResultCalc);
            _consoleAction.PrintMessage(ConsoleConstants.SelectAction);
            return await SaveHelperAsync(GetByteNumber(), dtoList);
        }
        int number = GetIntNumber(textInput);
        arrayNums.Add(number);

        return true;


    }
    /// <summary>
    /// Сохраняем значения,полученные из микросервиса GrpcMathService через gRPC в микросервис GrpcWithDbService через ApacheKafka
    /// </summary>
    /// <param name="number"></param>
    /// <param name="dtoList"></param>
    /// <returns></returns>
    /// <exception cref="ConsoleInputException"></exception>
    private async Task<bool> SaveHelperAsync(byte number, List<SumResultDto> dtoList)
    {
        bool flag = false;
        switch (number)
        {
            case 1:
                await _kafkaService.ProduceStartAsync(dtoList);
                _consoleAction.PrintMessage(ConsoleConstants.SaveComplited);
                break;
            case 2:
                break;
            default:
                throw new ConsoleInputException(ExceptionConstants.ConsoleInputExceptionMessage);
        }

        return flag;
    }

    /// <summary>
    /// проверяем продолжать ли ввод чисел от пользователя
    /// </summary>
    /// <param name="isQuit"></param>
    /// <param name="isEndInputNums"></param>
    /// <returns></returns>
    private string GetNumberInput(ref bool isQuit, ref bool isEndInputNums)
    {
        string textInput = _consoleAction.GetConsoleInput();
        if (textInput.ToLower() == ConsoleConstants.SymbolForExit)
            isQuit = false;
        if (textInput.ToLower() == ConsoleConstants.SymbolForEndInput)
            isEndInputNums = true;

        return textInput;
    }
    /// <summary>
    /// получаем действие пользователя с консоли
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ConsoleInputException"></exception>
    private byte GetByteNumber()
    {
        bool flag = byte.TryParse(_consoleAction.GetConsoleInput(), out var number);
        if (!flag)
            throw new ConsoleInputException(ExceptionConstants.ConsoleInputExceptionMessage);

        return number;
    }
    /// <summary>
    /// получаем число (для отправки массива со всеми числами на сервер) с консоли
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ConsoleInputException"></exception>
    private int GetIntNumber(string textInput)
    {
        bool isNumber = Int32.TryParse(textInput, out var number);
        if (!isNumber)
            throw new ConsoleInputException(ExceptionConstants.ConsoleInputExceptionMessage);

        return number;
    }
    private void PrintCalcResult(List<SumResultDto> result)
    {
        foreach (var el in result)
            _consoleAction.PrintMessage(el.Description);
    }
}
