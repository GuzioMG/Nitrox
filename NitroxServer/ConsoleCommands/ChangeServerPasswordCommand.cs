using System;
using NitroxModel.DataStructures.Util;
using NitroxModel.Logger;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxModel.DataStructures.GameLogic;
using NitroxServer.ConfigParser;

namespace NitroxServer.ConsoleCommands
{
    internal class ChangeServerPasswordCommand : Command
    {
        private readonly ServerConfig serverConfig;

        public ChangeServerPasswordCommand(ServerConfig serverConfig) : base("changeserverpassword", Perms.ADMIN, "[<password>]", "Changes the server password. No arguments to clear the password.")
        {
            this.serverConfig = serverConfig;
        }

        public override void RunCommand(string[] args, Optional<Player> player)
        {
            try
            {
                string playerName = player.IsPresent() ? player.Get().Name : "SERVER";
                ChangeServerPassword(args.Length==0?"":args[0], playerName);
            }
            catch (Exception ex)
            {
                Log.Error("Error attempting to " + (args.Length == 0 ? "change " : "remove ") + "the server password" + (args.Length == 0 ? "" : (" to "+args[0])) + ": ", ex);
            }
        }

        public override bool VerifyArgs(string[] args)
        {
            return args.Length >= 0;
        }

        private void ChangeServerPassword(string password, string name)
        {
            serverConfig.ChangeServerPassword(password);
            Log.Warn($"Server password changed to \"{password}\" by {name}");
        }
    }
}
