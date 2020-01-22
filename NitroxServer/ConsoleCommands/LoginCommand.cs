using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.GameLogic.Players;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using NitroxModel.Logger;
using NitroxServer.ConfigParser;

namespace NitroxServer.ConsoleCommands
{
    internal class LoginCommand : Command
    {
        private readonly PlayerData playerData;
        private readonly ServerConfig serverConfig;

        public LoginCommand(PlayerData playerData, ServerConfig serverConfig) : base("login", Perms.PLAYER, "<password>", "Gives a player admin permissions, even if he/she doesn't have permissions. Requies acces key (admin <password>).", new string[] {"force", "admin"})
        {
            this.playerData = playerData;
            this.serverConfig = serverConfig;
        }

        public override void RunCommand(string[] args, Optional<Player> player)
        {
            string pass = args[0];
            string message;
            string playerName = player.Get().Name;
            bool correctPass = true;

            Log.Warn("Player " + playerName + " forced admin permissions on itself. Given password...");

            if (pass == serverConfig.AdminPassword)
            {
                if (playerData.UpdatePlayerPermissions(playerName, Perms.ADMIN))
                {
                    message = "Updated permissions to admin for " + playerName;
                    Log.Warn("...is correct. Make sure you trust this person, as it can now do almost anything.");
                }
                else
                {
                    message = "Could not update permissions " + playerName;
                    Log.Info("...is correct.");
                }
            }
            else
            {
                message = "Incorrect Password.";
                Log.Warn("...is incorrect. Beware, this might be a brute-force attack. ...Or just a typo :-)");
                correctPass = false;
            }
            Log.Info((!correctPass ? ("Could not update permissions " + playerName + ". "):"") + message);
            ChatManager.SendServerMessageIfPlayerIsPresent(player, message);
        }

        public override bool VerifyArgs(string[] args)
        {
            return args.Length == 1;
        }
    }
}
