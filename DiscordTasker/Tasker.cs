using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text;

namespace DiscordTasker
{
    public class Tasker
    {
        private DiscordSocketClient socketClient;
        private CommandService commandService;
        private IServiceProvider serviceProvider;

        public Tasker()
        {
            socketClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });

            socketClient.Log += Log;
            commandService = new CommandService();
            serviceProvider = new ServiceCollection().BuildServiceProvider();
        }

        public async Task StartAsync()
        {
            // 秘密ファイルからトークン取得
            string secret = File.ReadAllText("secret.json", Encoding.UTF8);
            JObject secretData = JObject.Parse(secret);
            string token = (string?)secretData["token"] ?? throw new InvalidCastException();

            // ログイン
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), serviceProvider);
            await socketClient.LoginAsync(TokenType.Bot, token);
            await socketClient.StartAsync();
        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        public Task OnExitAsync()
        {
            return socketClient.LogoutAsync();
        }
    }
}
