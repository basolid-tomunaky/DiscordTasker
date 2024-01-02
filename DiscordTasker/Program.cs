using DiscordTasker.Extensions;

namespace DiscordTasker
{
    public class Program
    {
        private readonly Tasker tasker = new Tasker();
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private static void Main(string[] args)
        {
            new Program().Main();
        }

        private void Main()
        {
            Console.CancelKeyPress += OnCancelKeyPressed;
            tasker.StartAsync().Forget();

            try
            {
                var token = cancellationTokenSource.Token;
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Canceled.");
            }
        }

        private void OnCancelKeyPressed(object? sender, ConsoleCancelEventArgs e)
        {
            Console.CancelKeyPress -= OnCancelKeyPressed;
            e.Cancel = true;
            cancellationTokenSource.Cancel();
            tasker.OnExitAsync().Wait();
        }
    }
}