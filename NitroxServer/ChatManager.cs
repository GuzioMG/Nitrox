using NitroxModel.DataStructures.Util;
using NitroxModel.Packets;

namespace NitroxServer
{
    class ChatManager
    {
        public static void        SendPrebuildMessage(ChatMessage chatMessage, Player target)
        {
            target.SendPacket(chatMessage);
        }

        public static ChatMessage SendServerMessage  (Player player, string message)
        {
            ChatMessage chatMessage = messageBuilder(message, 0, true);
            SendPrebuildMessage(chatMessage, player);
            return chatMessage;
        }

        public static ChatMessage SendFakeMessage   (Player player, string message, ushort fakePlayerID)
        {
            ChatMessage chatMessage = messageBuilder(message, fakePlayerID, false);
            SendPrebuildMessage(chatMessage, player);
            return chatMessage;
        }



        public static ChatMessage SendServerMessageIfPlayerIsPresent(Optional<Player> player, string message)
        {
            ChatMessage chatMessage = null;
            if (player.IsPresent())
            {
                chatMessage = SendServerMessage(player.Get(), message);
            }
            return chatMessage;
        }



        public static ChatMessage messageBuilder(string message, ushort sender, bool asServer)
        {
            ChatMessage chatMessage;
            if(asServer){
                sender = ChatMessage.SERVER_ID;
            }
            chatMessage = new ChatMessage(sender, message);
            return chatMessage;
        }
    }
}
