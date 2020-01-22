using NitroxModel.DataStructures.Util;
using NitroxModel.Packets;

namespace NitroxServer
{
    class ChatManager
    {
        public static void SendServerMessageIfPlayerIsPresent(Optional<Player> player, string message)
        {
            if (player.IsPresent())
            {
                player.Get().SendPacket(new ChatMessage(ChatMessage.SERVER_ID, message));
            }
        }
    }
}
