using NitroxModel.DataStructures.Util;
using NitroxModel.DataStructures.GameLogic;
using NitroxServer.ConsoleCommands.Abstract;

namespace NitroxServer.ConsoleCommands
{
    class IpCommand : Command
    {
        public IpCommand() : base("ip", Perms.CONSOLE, "", "Show IPs of the server.", new string[] {"ipconfig", "getip", "whatismyipdotcom"})
        {

        }

        public override void RunCommand(string[] args, Optional<Player> player)
        {
            throw new System.NotImplementedException();
        }

        public override bool VerifyArgs(string[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}
