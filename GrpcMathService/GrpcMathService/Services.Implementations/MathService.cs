using GrpcMathService.Infrastructure.Extensions;
using GrpcMathService.Services.Abstractions;
using System.Collections.Concurrent;

namespace GrpcMathService.Services.Implementations;

internal sealed class MathService: IMathService
{
    /// <summary>
    /// вычисляем сумму элементов массива в зависимости от типа вычисления
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="typeCalc"></param>
    /// <returns></returns>
    public int GetSumByCollection(IEnumerable<int> collection, TypeCalc typeCalc)
    {
        int sum = 0;
        return typeCalc switch
        {
            TypeCalc.None => GetSumCollection(collection, sum),
            TypeCalc.ParallelLocker => GetSumCollectionWithLocker(collection, sum),
            TypeCalc.ParallelInterlocked => GetSumCollectionWithInterlocked(collection, sum),
            TypeCalc.Thread => GetSumCollectionWithThreads(collection, sum),
            _ => GetSumCollectionByParallelLINQ(collection, sum)
        };
    }
    /// <summary>
    /// обычное последовательное вычисление суммы элементов коллекции
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="sum"></param>
    /// <returns></returns>
    private int GetSumCollection(IEnumerable<int> collection,int sum)
    {
        foreach (var item in collection)
            sum += item;
        return sum;
    }

    /// <summary>
    /// вычисление суммы элементов коллекции через Parallel LINQ
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="sum"></param>
    /// <returns></returns>
    private int GetSumCollectionByParallelLINQ(IEnumerable<int> collection, int sum)
    {
        collection.AsParallel().ForAll(x => sum += x);
        return sum;
    }

    /// <summary>
    /// вычисление суммы элементов коллекции через TPL + locker
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="sum"></param>
    /// <returns></returns>
    private int GetSumCollectionWithLocker(IEnumerable<int> collection, int sum)
    {
        object locker = new();
        Parallel.ForEach(collection, x =>
        {
          lock(locker)
                sum+= x;
        });
        return sum;
    }

    /// <summary>
    /// вычисление суммы элементов коллекции через TPL + interlocked
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="sum"></param>
    /// <returns></returns>
    private int GetSumCollectionWithInterlocked(IEnumerable<int> collection, int sum)
    {
        Parallel.ForEach(collection, x =>Interlocked.Add(ref sum, x));
        return sum;
    }

    /// <summary>
    /// вычисление суммы элементов коллекции через Thread c использованием ConcurrentBag
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="sum"></param>
    /// <returns></returns>
    private int GetSumCollectionWithThreads(IEnumerable<int> collection, int sum)
    {
        ///наполняем ConcurrentBag значениями из массива с клиента
        ConcurrentBag<int> conBag = GetConcurrentBag(collection);
        // достаем значения из ConcurrentBag и считаем их сумму
        List<Thread> bagConsumeTasks = new List<Thread>();
        while (!conBag.IsEmpty)
        {
            Thread thread = new Thread(() =>
            {
                int item;
                if (conBag.TryTake(out item))
                    Interlocked.Add(ref sum, item);
            });
            thread.Start();
            bagConsumeTasks.Add(thread);
        }
        bagConsumeTasks.ForEach(x => x.Join());
        return sum;
    }
    /// <summary>
    /// наполняем ConcurrentBag значениями из массива с клиента
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    private ConcurrentBag<int> GetConcurrentBag(IEnumerable<int> collection)
    {
        ConcurrentBag<int> bag = new ConcurrentBag<int>();
        List<Thread> bagAddTasks = new List<Thread>();
        foreach (var item in collection)
        {
            Thread thread = new Thread(() => bag.Add(item));
            thread.Start();
            bagAddTasks.Add(thread);
        }
        bagAddTasks.ForEach(x => x.Join());
        return bag;
    }
}
