using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using NitroxModel.Logger;

namespace NitroxServer
{
    public static class IpLogger
    {
        public static string FailedToResolveExternalIpMessage = "Could not get your external IP. Go to whatismyipaddress.com (IP data provider for this mod) and figure it out yourself.";
        public static string FailedToResolveIPsMessage = "Unable to resolve IP Addresses... you are on your own. (You're on Mac / Linux (Under Wine, not Mono), are you?)";


        [Obsolete]
        public static void PrintServerIps()
        {
            PrintServerIps(false);
        }

        public static void PrintServerIps(bool errorable)
        {
            try
            {
                NetworkInterface[] allInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface eachInterface in allInterfaces)
                {
                    PrintIfHamachi(eachInterface);
                    PrintIfLan(eachInterface);
                }

                PrintIfExternal();
            }
            catch (Exception ex)
            {
                if (errorable)
                {
                    Log.Warn(FailedToResolveExternalIpMessage);  // Printing it only because there is a possible solution to an average user. Totally useless otherwise.
                    Log.Error(FailedToResolveIPsMessage + "!! >Error details: ", ex);
                    return;
                }
                // This is technically an error, but will scare most users into thinking the server is not working.
                // Generally, this can happen on Mac / Wine due to issues fetching networking interfaces.  Simply
                // ignore as this is not a big deal.  They can look these up themselves.
                Log.Info(FailedToResolveIPsMessage);  //Look up :-)
                Log.Info(FailedToResolveExternalIpMessage);
            }
        }

        private static void PrintIfHamachi(NetworkInterface @interface)
        {
            if (@interface.Name != "Hamachi")
            {
                return;
            }

            var ips = @interface.GetIPProperties().UnicastAddresses
                .Select(address => address.Address.ToString())
                .Where(address => !address.ToString().Contains("fe80::"));
            Log.Info("If using Hamachi, use this IP: " + string.Join(" or ", ips));
        }

        private static void PrintIfLan(NetworkInterface @interface)
        {
            if (@interface.GetIPProperties().GatewayAddresses.Count == 0)
            {
                return;
            }

            foreach (UnicastIPAddressInformation eachIp in @interface.GetIPProperties().UnicastAddresses)
            {
                string[] splitIpParts = eachIp.Address.ToString().Split('.');
                int secondPart = 0;
                if (splitIpParts.Length > 1)
                {
                    int.TryParse(splitIpParts[1], out secondPart);
                }

                if (splitIpParts[0] == "10" || splitIpParts[0] == "192" && splitIpParts[1] == "168" || splitIpParts[0] == "172" && secondPart > 15 && secondPart < 32) //To get if IP is private
                {
                    Log.Info("If playing on LAN, use this IP: " + eachIp.Address);
                }
            }
        }

        private static void PrintIfExternal()
        {
            using (Ping ping = new Ping())
            {
                ping.PingCompleted += PingOnPingCompleted;
                ping.SendAsync("8.8.8.8", 1000, null);
            }
        }

        private static void PingOnPingCompleted(object sender, PingCompletedEventArgs e)
        {
            if (e.Reply == null || e.Reply.Status != IPStatus.Success)
            {
                return;
            }

            using (WebClient client = new WebClient())
            {
                client.DownloadStringCompleted += ExternalIpStringDownloadCompleted;
                client.DownloadStringAsync(new Uri("http://ipv4bot.whatismyipaddress.com/")); // from https://stackoverflow.com/questions/3253701/get-public-external-ip-address answer by user_v
            }
        }

        private static void ExternalIpStringDownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                Log.Info($"If using port forwarding, use this IP: {e.Result}");
            }
            else
            {
                Log.Warn(FailedToResolveExternalIpMessage);
            }
        }
    }
}
