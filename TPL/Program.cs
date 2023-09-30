// See https://aka.ms/new-console-template for more information
using Task_;

Console.WriteLine("Hello, World!");

AsyncSemaphore s_asyncSemaphore = new AsyncSemaphore();


TaskControl control1 = new TaskControl(true);
Task task1 = Task.Run(async () => {
    try
    { 
        Console.WriteLine("Enter task1");
        using(await s_asyncSemaphore.WaitAsync())
        {
            Console.WriteLine("Enter task1 lock");
            while (true)
            {
                Console.WriteLine("task1");
                await Task.Delay(1000, control1.CancelSource.Token);
            }
        }
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("OperationCanceledException");
    }
    catch (Exception ex)
    {
        Console.WriteLine("task1.catch");
    }
    finally
    {
        Console.WriteLine("task1.Release");
    }
    Console.WriteLine("Exit task1");
}, control1.CancelSource.Token);

Thread.Sleep(1000);

TaskControl control2 = new TaskControl(true);
Task task2 = Task.Run(async () => {
    Console.WriteLine("Enter task2");
    //await asyncLock.WaitAsync();
    using (await s_asyncSemaphore.WaitAsync())
    {
        Console.WriteLine("Enter task2 lock");
    }
    //asyncLock.Release();
    Console.WriteLine("Exit task2");
}, control2.CancelSource.Token);


if (Console.ReadKey().Key == ConsoleKey.Spacebar)
{
    Console.WriteLine("taks1.cancel");
    control1.Cancel();
}

Console.ReadKey();