using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
namespace Jesse_McCsharp
{
    class Bob
    {
        DiscordClient discord;
        CommandService commands;
        public Bob()
        {

            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });
            discord.UsingCommands(x =>
            {
                x.PrefixChar = '~';
                x.AllowMentionPrefix = true;
                x.HelpMode = HelpMode.Private;

            });
            commands = discord.GetService<CommandService>();
            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MzAyNzkyNzc4MTM4NTE3NTA1.C9OujA.fOcHe8-nRm8Nsu1y-UuKr4o_v-c", TokenType.Bot);
            });
        }
        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

    }
}
