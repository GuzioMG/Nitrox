using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.GameLogic.Players;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using NitroxModel.Logger;

namespace NitroxServer.ConsoleCommands
{
    internal class OpCommand : Command
    {
        private readonly PlayerData playerData;

        public OpCommand(PlayerData playerData) : base("op", Perms.CONSOLE, "<name>", "Sets an user as admin.")
        {
            this.playerData = playerData;
        }

        public override void RunCommand(string[] args, Optional<Player> player)
        {
            string playerName = args[0];
            string message;

            if(playerData.UpdatePlayerPermissions(playerName, Perms.ADMIN))
            {
                message = "Updated " + playerName + " permissions to admin";
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
