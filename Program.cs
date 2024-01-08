using CliWrap;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MineBot.commands;
using MineBot.config;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MineBot
{
    internal class Program
    {
        private static DiscordClient Client { get; set; }
        private static CommandsNextExtension Commands { get; set; }

        static async Task Main(string[] args)
        {
            //Get bot token
            var jsonReader = new JSONReader();
            await jsonReader.ReadJson();

            //Create bot Configuration
            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = jsonReader.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };

            //Apply bot config
            Client = new DiscordClient(discordConfig);

            Client.Ready += Client_Ready;
            Client.ComponentInteractionCreated += ButtonPressed;

            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { jsonReader.Prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = true
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<MinecraftCommands>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task StopServer()
        {
            await Cli.Wrap("dash")
                            .WithArguments("stop")
                            .WithWorkingDirectory("/home/peru/minecraft")
                            .ExecuteAsync();
        }

        private static async Task ButtonPressed(DiscordClient sender, DSharpPlus.EventArgs.ComponentInteractionCreateEventArgs args)
        {
            switch (args.Interaction.Data.CustomId)
            {
                case "shutdown":
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent("Shutting down server..."));
                    await StopServer();
                    break;
                case "cancel":
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent("Server shutdown Canceled"));
                    break;

                default:
                    break;
            }
        }

        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {        
            return Task.CompletedTask;
        }
    }
}