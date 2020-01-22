using System;
using NitroxModel.DataStructures.Util;
using NitroxModel.Logger;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.GameLogic;
using NitroxModel.DataStructures.GameLogic;
using NitroxServer.ConfigParser;

namespace NitroxServer.ConsoleCommands
{
    internal class ChangeAdminPasswordCommand : Command
    {
        private readonly ServerConfig serverConfig;

        public ChangeAdminPasswordCommand(PlayerManager playerManager, ServerConfig serverConfig) : base("changeadminpassword", Perms.ADMIN, "<password>", "Changes the admin password.")
        {
            this.serverConfig = serverConfig;
        }

        public override void RunCommand(string[] args, Optional<Player> player)
        {
            try
            {
                string playerName = player.IsPresent() ? player.Get().Name : "SERVER";
                ChangeAdminPassword(args[0], playerName);   
            }
            catch (Exception ex)
            {
                Log.Error("Error attempting to change an admin password to "+args[0]+": ", ex);
            }
        }

        public override bool VerifyArgs(string[] args)
        {
            return args.Length >= 1;
        }

        private void ChangeAdminPassword(string password, string name)
        {
            serverConfig.ChangeAdminPassword(password);
            Log.Warn($"Admin password changed to \"{password}\" by {name}");
        }
    }
}
