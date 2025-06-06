using System;
using System.Threading;

namespace test3
{
    public class ComputationController
    {
        private readonly ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private int counter = 0;

        public event Action<int>? OnStep;

        public void Start()
        {
            Thread thread = new Thread(() =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    pauseEvent.Wait();
                    OnStep?.Invoke(counter++);
                    Thread.Sleep(100);
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public void TogglePause()
        {
            if (pauseEvent.IsSet)
                pauseEvent.Reset();
            else
                pauseEvent.Set();
        }

        public void Stop()
        {
            cts.Cancel();
            pauseEvent.Set();
        }

        public int Counter => counter;
    }

    class Program
    {
        static void Main()
        {
            var engine = new ComputationController();
            engine.OnStep += (x) => Console.WriteLine($"Time: {x}");
            engine.Start();

            if (Console.IsInputRedirected)
            {
                Console.WriteLine("Non-interactive mode detected. Running for 10 steps.");
                Thread.Sleep(1000);
                engine.Stop();
                return;
            }

            Console.WriteLine("Space for pause/resume. ESC for exit.");

            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Spacebar)
                {
                    engine.TogglePause();
                    Console.WriteLine("Switched.");
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    engine.Stop();
                    Console.WriteLine("Program stopped.");
                    break;
                }
            }
        }
    }
}
