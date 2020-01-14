using NitroxModel.DataStructures.Util;
using NitroxModel.DataStructures.GameLogic;
using NitroxServer.ConsoleCommands.Abstract;
using static NitroxModel.Logger.Log;

namespace NitroxServer.ConsoleCommands
{
    class IpCommand : Command
    {
        public IpCommand() : base("ip", Perms.CONSOLE, "", "Show IPs of the server.", new string[] {"ipconfig", "getip", "whatismyip-dot-com", "ips"})
        {
        }

        public override void RunCommand(string[] args, Optional<Player> player)
        {
            Info("Printing IP...");
            IpLogger.PrintServerIps();
            Info("IPs printed. If nothing got displayed, wait a few seconds. If it still doesn't seem to work (or thrown an exception), figure IPs yourself. It's not that hard. Even the lowliest of cogs can do it!"); //I have no idea, what/who the heck is "the lowliest of cogs", but I've seen this easteregg couple times in this mod.
        }

        public override bool VerifyArgs(string[] args)
        {
            return (args.Length == 0);
        }
    }
}
