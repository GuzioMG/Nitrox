using NitroxModel.Logger;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.GameLogic;
using System.Collections.Generic;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;

namespace NitroxServer.ConsoleCommands
{
    internal class ListCommand : Command
    {
        private readonly PlayerManager playerManager;

        public ListCommand(PlayerManager playerManager) : base("list", Perms.PLAYER, "", "Gives a list of online players.", new string[] {"online", "players", "whois" })
        {
            this.playerManager = playerManager;
        }

        public override void RunCommand(string[] args, Optional<Player> player)
        {
            List<Player> players = playerManager.GetPlayers();

            string playerList = "List Command Result: " + string.Join(", ", players);

            if(players.Count == 0)
            {
                playerList += "No Players Online";
            }

            Log.Info(playerList);
            ChatManager.SendServerMessageIfPlayerIsPresent(player, playerList);
        }

        public override bool VerifyArgs(string[] args)
        {
            return args.Length == 0;
        }
    }
}
