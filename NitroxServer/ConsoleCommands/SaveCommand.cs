using NitroxServer.ConsoleCommands.Abstract;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using System.Configuration; //TODO: Make save settings presisent

namespace NitroxServer.ConsoleCommands
{
    internal class SaveCommand : Command
    {

        Server server = Server.Instance;

        public SaveCommand() : base("save", Perms.ADMIN, "[<config>|now|<helpCmd>]", "Saves, shows currrent save configuration or sets save configuration.")
        {
        }

        public override void RunCommand(string[] args, Optional<Player> player)
        {
            server.Save();
        }

        public override bool VerifyArgs(string[] args)
        {
            if(args.Length == 0)
            {
                return true;
            }
            else if (args.Length == 1 && (args[0] == "now" || args[0] == "?" || args[0] == "info" || args[0] == "help" || args[0] == "h" || args[0] == "q" || args[0] == "never" || args[0] == "exit" || args[0] == "auto" || args[0] == "normal" || args[0] == "disableAutosaveOnNoPlayers:true" || args[0] == "disableAutosaveOnNoPlayers:false"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
