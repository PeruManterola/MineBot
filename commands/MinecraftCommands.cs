using CliWrap;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MineStatLib;
using System.Net;

namespace MineBot.commands
{
    public class MinecraftCommands : BaseCommandModule
    {
        [Command("ip")]
        [RequireRoles(RoleCheckMode.MatchNames, "Minecrafters")]
        public async Task GetIp(CommandContext ctx)
        {
            WebClient webClient = new WebClient();
            string publicIp = webClient.DownloadString("https://api.ipify.org");

            await ctx.Channel.SendMessageAsync($"La IP del server es: {publicIp}");
        }

        [Command("status")]
        [RequireRoles(RoleCheckMode.MatchNames, "Minecrafters")]
        public async Task CheckServerStatus(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Checking Status...");
            MineStat ms = new MineStat("192.168.1.137", 25565);

            var statusMessage = new DiscordEmbedBuilder
            {
                Title = $"TitiriterozCraft Minecraft Server",
                Color = DiscordColor.SpringGreen
            };
            statusMessage.WithThumbnail("https://pbs.twimg.com/tweet_video_thumb/FkmuzFaXEAcRHv7.jpg");



            if (ms.ServerUp)
            {
                WebClient webClient = new WebClient();
                string publicIp = webClient.DownloadString("https://api.ipify.org");

                statusMessage.AddField("Status:", "Online :green_circle:", false);
                statusMessage.AddField("Members Online:", $"{ms.CurrentPlayers}/{ms.MaximumPlayers}", false);
                statusMessage.AddField("Version:", $"{ms.Version}", false);
                statusMessage.AddField("IP:", $"{publicIp}", false);
                await ctx.Channel.SendMessageAsync(embed: statusMessage);
            }
            else if (ms.ServerUp == false)
            {
                statusMessage.AddField("Status", "Offline :red_circle: :rage: ", true);
                await ctx.Channel.SendMessageAsync(embed: statusMessage);
            }
            else
            {
                await ctx.Channel.SendMessageAsync("minestat not working properly");
            }

        }

        [Command("stopserver")]
        [RequireRoles(RoleCheckMode.MatchNames, "Minecrafters")]
        public async Task StopServer(CommandContext ctx)
        {
            DiscordButtonComponent confirmButton = new DiscordButtonComponent(ButtonStyle.Danger, "shutdown", "SHUTDOWN");
            DiscordButtonComponent denyButton = new DiscordButtonComponent(ButtonStyle.Secondary, "cancel", "Cancel");


            var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithColor(DiscordColor.SpringGreen)
                .WithTitle("Are you sure you want to **SHUTDOWN** the server?")
                )
                .AddComponents(confirmButton, denyButton); 

            await ctx.Channel.SendMessageAsync(message);
            //await StopServer();
        }

        

        [Command("startServer")]
        [RequireRoles(RoleCheckMode.MatchNames, "Minecrafters")]
        public async Task StartServer(CommandContext ctx)
        {
            var statusMessage = new DiscordEmbedBuilder
            {
                Title = $"Starting Server...",
                Description = "This might take a few minutes, you can check if it's Online with the !status command.",
                Color = DiscordColor.SpringGreen
            };

            await ctx.Channel.SendMessageAsync(embed: statusMessage);

            await Cli.Wrap("dash")
               .WithArguments("server")
               .WithCredentials(creds => creds
                    .SetUserName("peru")
                    .SetPassword("pmfOzzy0488")
                    .LoadUserProfile()
                )
               .WithWorkingDirectory("/home/peru/minecraft")
               .ExecuteAsync();
        }
    }
}
