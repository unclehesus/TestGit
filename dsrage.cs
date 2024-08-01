using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using RAGE;

namespace DiscordRageMPBot
{
    class Program : Events.Script
    {
        private DiscordSocketClient _client;

        public Program()
        {
            StartBotAsync().GetAwaiter().GetResult();
        }

        private async Task StartBotAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += LogAsync;
            _client.MessageReceived += MessageReceivedAsync;

            var token = "token"; 
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            Console.WriteLine("бот в ахуях включен!");

            Events.OnPlayerJoin += OnPlayerJoinHandler;
            Events.OnPlayerQuit += OnPlayerQuitHandler;

            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(SocketMessage message)
        {

            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            if (message.Content == "!players")
            {

                var players = RAGE.Elements.Entities.Players.All;
                string playerList = string.Join(", ", players.Select(p => p.Name));
                await message.Channel.SendMessageAsync($"Players online: {playerList}");
            }
        }

        private void OnPlayerJoinHandler(RAGE.Elements.Player player)
        {

            var channel = _client.GetChannel(ulong.Parse("id kanala")) as IMessageChannel;
            channel?.SendMessageAsync($"{player.Name} joined the server.");
        }

        private void OnPlayerQuitHandler(RAGE.Elements.Player player, string reason)
        {

            var channel = _client.GetChannel(ulong.Parse("id kanala")) as IMessageChannel;
            channel?.SendMessageAsync($"{player.Name} left the server. Reason: {reason}");
        }

        static void Main(string[] args)
        {
            new Program();
        }
    }
}
