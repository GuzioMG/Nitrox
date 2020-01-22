using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.GameLogic.Players;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using NitroxModel.Logger;

namespace NitroxServer.ConsoleCommands
{
    internal class DeopCommand : Command
    {
        private readonly PlayerData playerData;

        public DeopCommand(PlayerData playerData) : base("deop", Perms.CONSOLE, "<name>")
        {
            this.playerData = playerData;
        }

        public override void RunCommand(string[] args, Optional<Player> player)
        {
            string playerName = args[0];
            string message;

            if (playerData.UpdatePlayerPermissions(playerName, Perms.PLAYER))
            {
                message = "Updated " + playerName + " permissions to player";
            }
            else
            {
                message = "Could not update permissions on unknown player " + playerName;
            }

            Log.Info(message);
        }

        public override bool VerifyArgs(string[] args)
        {
            return args.Length == 1;
        }
    }
}
