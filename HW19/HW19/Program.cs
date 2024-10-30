class Progarm
{
	private static Mutex mutex = new();
	private static ManualResetEvent manualResetEvent = new(false);

	static void Main(string[] args)
	{
		Thread ownerThread = new(() =>
		{
			Console.WriteLine($"Gas station is open!");
			manualResetEvent.Set();
		});

		for (int i = 0; i < 5; i++)
		{
			Thread thread = new(() =>
			{
				Console.WriteLine($"Client no.{Thread.CurrentThread.ManagedThreadId} waiting for gas station to open!");
				manualResetEvent.WaitOne();
				UseGasStation();
			});			
            thread.Start();
		}

		ownerThread.Start();

		Console.ReadKey();
	}

	static void UseGasStation()
	{
		mutex.WaitOne();
        Console.WriteLine($"Client no.{Thread.CurrentThread.ManagedThreadId} started using gas station!");
		Thread.Sleep(4000);
        Console.WriteLine($"Client no.{Thread.CurrentThread.ManagedThreadId} vacated the gas station!");
		mutex.ReleaseMutex();
    }
}