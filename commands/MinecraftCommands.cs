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
        [RequireRoles(RoleCheckMode.MatchNames, "tonto")]
        public async Task GetIp(CommandContext ctx)
        {
            WebClient webClient = new WebClient();
            string publicIp = webClient.DownloadString("https://api.ipify.org");

            await ctx.Channel.SendMessageAsync($"La IP del server es: {publicIp}");
        }

        [Command("status")]
        public async Task CheckServerStatus(CommandContext ctx)
        {

            //ip externo 207.188.191.8
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
                statusMessage.AddField("Status", "Online :red_circle:", true);
                await ctx.Channel.SendMessageAsync(embed: statusMessage);
            }
            else
            {
                await ctx.Channel.SendMessageAsync("minestat not working properly");
            }

        }
    }
}
