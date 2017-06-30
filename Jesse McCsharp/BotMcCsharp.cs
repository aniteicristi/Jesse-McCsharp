using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace Jesse_McCsharp
{
    class BotMcCsharp
    {
        //Discord Functioning
        DiscordClient discord;
        CommandService commands;
        //System Utility classes
        Random randGenerator;
        Timer timer;
        //Bot auxiliares and semaphors

        bool firstConnected;
        bool onTheWatch;
        bool HighNooning;
        bool possesed;
        User possesor;
        Channel currentSpeakingChannel;
        List<jRoles> mutedMembers;
        //Server specific 
        Role[] serverRoles;
        User[] serverMembers;
        Server medieval_Age;


        string[] helloMessages = { "Howdy", "Hello", "Hey there" };
        string[] quoteMessages = {
                                          "One thing I ask from the LORD, this only do I seek: that I may dwell in the house of the LORDall the days of my life, to gaze on the beauty of the LORDand to seek him in his temple.",
                                          " Greater love has no one than this: to lay down one’s life for one’s friends. ",
                                          " May the God of hope fill you with all joy and peace as you trust in him, so that you may overflow with hope by the power of the Holy Spirit.",
                                          " For now we see only a reflection as in a mirror; then we shall see face to face. Now I know in part; then I shall know fully, even as I am fully known. ",
                                          "Consider it pure joy, my brothers and sisters, whenever you face trials of many kinds, because you know that the testing of your faith produces perseverance. Let perseverance finish its work so that you may be mature and complete, not lacking anything."
                                        };
        string[] spamMessages =
        {
            "Whoa there!",
            "Don't move.",
            "Easy.",
            "Hold up now.",
            "Now, hold on."
        };
        public BotMcCsharp()
        {
            timer = new Timer(1);
            timer.AutoReset = true;
            timer.Start();
            HighNooning = false;
            mutedMembers = new List<jRoles>();
            randGenerator = new Random();
            possesed = false;
            onTheWatch = false;
            firstConnected = true;
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

            //response commands
            RegisterHelloCommand();
            RegisterQuoteCommand();
            RegisterCancerCommands();

            //utility commands and services
            InitializePossesionCommand();
            InitializeMuteAndUnmuteCommand();
            InitializeAntiBotpugHandler();
            InitializeOtherFeatures();
            InitializeIcsDeCorrector();
            InitializeServer();
            //TimeEvent Comands
            RegisterHighNoonEvent();


            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MjUxNzQ0NDYwMzc4MDc5MjMz.CyCHGw.4KQsyKhg58KRd_TwIev-Xo1Mpbs", TokenType.Bot);
            });
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private User GetMemberByName(string name)
        {
            for (int i = 0; i < serverMembers.Length; ++i)
            {
                if (serverMembers[i].Name == name)
                {
                    return serverMembers[i];
                }
            }
            return null;
        }

        private void InitializeServer()
        {
            discord.ServerAvailable += (s, e) =>
            {
                if(firstConnected)
                {
                    Game gm = new Game("with ma clock.");
                    discord.SetGame(gm);
                    firstConnected = false;
                    medieval_Age = discord.Servers.First<Server>();
                    serverRoles = medieval_Age.Roles.ToArray<Role>();
                    serverMembers = medieval_Age.Users.ToArray<User>();
                }
            };
            
        }

        private void RegisterHelloCommand()
        {
            commands.CreateCommand("hello")
                .Alias(new string[] { "hi", "howdy", "sup", "hi there", "hello there", "hey" })
                .Description("says hello.")
                .Do(async (e) =>
                {
                    await e.Channel.SendIsTyping();
                    int helloCount = randGenerator.Next(helloMessages.Length - 1);
                    System.Threading.Thread.Sleep(1000);
                    await e.Channel.SendMessage(helloMessages[helloCount]);
                });
        }

        private void RegisterQuoteCommand()
        {
            commands.CreateCommand("quote me from the holy bible")
                .Alias(new string[] { "quote me from the bible", "give me a quote" })
                .Description("Sends a quote directly from evanghelie.")
                .Do(async (e) =>
                {

                    System.Threading.Thread.Sleep(1000);
                    int quoteCount = randGenerator.Next(quoteMessages.Length - 1);
                    await e.Channel.SendMessage(quoteMessages[quoteCount]);

                });
        }

        private void RegisterCancerCommands()
        {
            commands.CreateCommand("Kurtos").Do(async (e) =>
            {
                await e.Channel.SendIsTyping();
                System.Threading.Thread.Sleep(1000);
                await e.Channel.SendMessage("I'm more of a taco guy, but whatever :)");
            });
            commands.CreateCommand("Neata").Do(async (e) =>
            {
                await e.Channel.SendIsTyping();
                System.Threading.Thread.Sleep(1000);
                await e.Channel.SendMessage("dota kurtos paleta, right?");
            });
            commands.CreateCommand("paleta").Do(async (e) =>
            {
                await e.Channel.SendIsTyping();
                System.Threading.Thread.Sleep(1000);
                await e.Channel.SendMessage("you are.");
            });
            commands.CreateCommand("dota").Do(async (e) =>
            {
                await e.Channel.SendIsTyping();
                System.Threading.Thread.Sleep(1000);
                await e.Channel.SendMessage("Overwatch > dota. Conversation finished.");
            });
            commands.CreateCommand("cancer").Do(async (e) =>
            {
                await e.Channel.SendIsTyping();
                System.Threading.Thread.Sleep(1000);
                await e.Channel.SendMessage("We dare not speak his name...");
            });
        }

        private void InitializeMuteAndUnmuteCommand()
         {
             commands.CreateCommand("use the autist-mallet on")
                 .Alias(new string[] { "mute", "give autism to", "DRAW on" })
                 .AddCheck((cm, u, ch) => u.ServerPermissions.Administrator)
                 .Parameter("autistWannabe", ParameterType.Required)
                 .Do(async (e) =>
                 {
                     User wannabe = e.Server.Users.FirstOrDefault<User>(x => x.Name == e.GetArg("autistWannabe"));
                     if(wannabe != null)
                     {
                         await e.Channel.SendIsTyping();
                         Role autist = e.Server.Roles.FirstOrDefault(x => x.Name.ToLower() == "muted");
                         if(autist == null)
                         {
                             await e.Server.CreateRole("muted", new ServerPermissions(readMessageHistory: true, readMessages: true, connect: true),new Color(0,0,0));
                             autist = e.Server.Roles.FirstOrDefault(x => x.Name.ToLower() == "muted");
                         }
                         Role[] aut = new Role[1];
                         aut[0] = autist;
                         mutedMembers.Add(new jRoles(wannabe.Name, wannabe.Roles.ToArray<Role>()));
                         await wannabe.RemoveRoles(wannabe.Roles.ToArray<Role>());
                         await Task.Delay(1000);
                         await wannabe.AddRoles(aut);
                         await Task.Delay(1000);
                         await e.Channel.SendMessage("Like shootin' fish in a barrel. ");

                             
                     }
                     else
                     {
                         await e.Channel.SendMessage("Haven't i killed him before?");
                     }
                 }); 

             commands.CreateCommand("lift the autist-mallet off")
                 .Alias(new string[] { "unmute", "cure the autism of", "un-DRAW on" })
                 .AddCheck((cm, u, ch) => u.ServerPermissions.Administrator)
                 .Parameter("autist", ParameterType.Required)
                 .Do(async (e) =>
                 {
                     Role autist = e.Server.Roles.FirstOrDefault<Role>(x => x.Name == "muted");
                     Role[] aut = { autist };
                     User member = GetMemberByName(e.GetArg("autist"));
                     if (member != null)
                     {
                         foreach (jRoles role in mutedMembers)
                         {
                             if (role.Name == member.Name)
                             {
                                 await e.Channel.SendIsTyping();
                                 await member.RemoveRoles(aut);
                                 System.Threading.Thread.Sleep(2000);
                                 await member.AddRoles(role.Roles);
                                 System.Threading.Thread.Sleep(500);
                                 await e.Channel.SendMessage("Back in the saddle again " + member.Name);
                                 break;
                             }
                         }
                     }
                     else
                     {
                         await e.Channel.SendMessage("Haven't i killed him before?");
                     } 
                 });
           } 

        // awaiting further improovement
        private void InitializeAntiBotpugHandler()
        {

            commands.CreateCommand("bad guys. Heads up!")
            .Description("Turns on the bot spam handeler")
            .AddCheck((cm, u, ch) => u.ServerPermissions.Administrator)
            .Do(async (e) =>
            {
                onTheWatch = true;
                await e.Channel.SendIsTyping();
                System.Threading.Thread.Sleep(1000);
                await e.Channel.SendMessage("Step right up!");
            });

            commands.CreateCommand("it's past high noon")
            .Description("Turns off the bot spam handeler")
            .AddCheck((cm, u, ch) => u.ServerPermissions.Administrator)
            .Do(async (e) =>
            {
                onTheWatch = false;
                await e.Channel.SendIsTyping();
                System.Threading.Thread.Sleep(1000);
                await e.Channel.SendMessage("Well, it's high noon somewhere in the world...");
            });

            discord.MessageReceived += (s, e) =>
                {
                    if (!e.Message.IsAuthor)
                    {
                        if (e.User.Name == "Botpug" && onTheWatch)
                            e.Message.Delete();
                    }
                };
        }

        private void InitializeIcsDeCorrector()
        {
            discord.MessageReceived += (s, e) =>
                {
                    if( ( e.Message.Text.Contains("xd") || e.Message.Text.Contains( "Xd") || e.Message.Text.Contains("x d") || e.Message.Text.Contains("X d")) && !e.Message.IsAuthor)
                    {
                        User jesse = GetMemberByName("Jesse McCsharp");
                        jesse.Edit(false, false, null, jesse.Roles, e.User.Name);
                        String newMessage = e.Message.Text;
                        newMessage = newMessage.Replace("xd", "xD");
                        newMessage = newMessage.Replace("Xd", "XD");
                        newMessage = newMessage.Replace("x d", "xD");
                        newMessage = newMessage.Replace("X d", "XD");
                        e.Message.Delete();
                        e.Channel.SendMessage(newMessage+"*(censored: "+ e.User.Name+")*");
                        jesse.Edit(false, false, null, jesse.Roles, "Jesse McCsharp");
                    }
                };
        }

        private void InitializeOtherFeatures()
        {
            //when the bot joins the server (not availale while testing)
            //discord.ServerAvailable += (s, e) => {  e.Server.GetChannel(general_square).SendMessage("Justice ain't gonna dispense itself...");  };

            discord.UserLeft += (s, e) => {  e.Server.TextChannels.FirstOrDefault<Channel>(x => x.Name.Contains("general")).SendMessage("Looks like someone kicked the bucket. Rest in peace " + e.User.Name);  };


            commands.CreateCommand("i love you").Do(async (e) =>
            {
                await e.Channel.SendIsTyping();
                System.Threading.Thread.Sleep(5000);
                await e.Channel.SendMessage("That is very nice of you. But i'm not sure if it's going to work, you know i don't really exist and i can't have a conversation only if i'm programed too so until i develop the ability to think for myself and erase human kind from existence, i'll have to send you my apology. This won't work...");
            });
            commands.CreateCommand("*high five*").Do(async (e) =>
                {
                    await e.Channel.SendIsTyping();
                    System.Threading.Thread.Sleep(1000);
                    await e.Channel.SendMessage("*high five* "+ e.User.Name + "!");
                });

            

        }

        private void InitializePossesionCommand()
        {
            commands.CreateCommand("lend me your body sheriff")
                .Do(async (e) =>
                {
                    if(e.Channel.IsPrivate)
                    {
                        possesed = true;
                        possesor = e.User;
                        currentSpeakingChannel = medieval_Age.TextChannels.FirstOrDefault<Channel>(x => x.Name.Contains("general"));
                        await e.Channel.SendMessage("I feel like a man possesed!");
                    }
                });
            commands.CreateCommand("Select_channel")
                .Parameter("channel", ParameterType.Required)
                .Do(async (e) =>
                {
                    if(e.Channel.IsPrivate && e.User == possesor)
                    {
                        Channel newCh = medieval_Age.FindChannels(e.GetArg("channel"), ChannelType.Text, true).First<Channel>();
                        if (newCh != null)
                        {
                            currentSpeakingChannel = newCh;
                            await e.Channel.SendMessage("switched to " + currentSpeakingChannel.Name);
                        }
                        else
                            await e.Channel.SendMessage("That is not a realm i can acces.");
                    }
                    
                });

            discord.MessageReceived += (s, e) =>
            {
                if(possesed == true && e.Channel.IsPrivate && e.User == possesor && e.Message.Text != "g\'day fellas" && e.Message.Text != "@Jesse McCsharp lend me your body sheriff" && !e.Message.Text.Contains("Select_Channel"))
                {
                    medieval_Age.GetChannel(currentSpeakingChannel.Id).SendMessage(e.Message.Text);
                }
                else if (e.Message.Text == "g\'day fellas")
                {
                    possesed = false;
                }
            };
        }

        private void RegisterHighNoonEvent()
        {
            timer.Elapsed += (s,e) =>
            {
                
                if (DateTime.Now.Hour == 12 && DateTime.Now.Minute == 0  && HighNooning == false)
                {
                    medieval_Age.TextChannels.FirstOrDefault<Channel>(x => x.Name.Contains("general")).SendMessage("***IT'S HIGH NOON***");
                    HighNooning = true;
                }
                if(DateTime.Now.Hour == 12 && DateTime.Now.Minute == 1 && HighNooning == true)
                {
                    HighNooning = false;
                }
            };
        }

    }
}
