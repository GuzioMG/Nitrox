using System.Timers;
using System;
using NitroxModel.Logger;
using NitroxServer.Serialization.World;
using NitroxServer.ConfigParser;
using System.Configuration;
using NitroxServer.GameLogic;

namespace NitroxServer
{
    public class Server
    {
        private readonly Timer saveTimer;
        public bool Autosaving;
        public bool DisableAutosaveOnNoPlayers = true;
        private Communication.NetworkingLayer.NitroxServer server;
        private readonly World world;
        private readonly WorldPersistence worldPersistence;
        public bool IsRunning { get; private set; }
        private bool IsSaving;
        public static Server Instance { get; private set; }

        public Server(WorldPersistence worldPersistence, World world, ServerConfig serverConfig, Communication.NetworkingLayer.NitroxServer server)
        {
            if (ConfigurationManager.AppSettings.Count == 0)
            {
                Log.Warn("Nitrox Server Cant Read Config File.");
            }
            Instance = this;
            this.worldPersistence = worldPersistence;
            this.world = world;
            this.server = server;
            
            saveTimer = new Timer();
            saveTimer.Interval = serverConfig.SaveInterval;
            saveTimer.AutoReset = true;
            saveTimer.Elapsed += delegate { Save(); };
        }

        public void Save()
        {
            if (IsSaving)
            {
                return;
            }
            IsSaving = true;
            worldPersistence.Save(world);
            IsSaving = false;
            if (PlayerManager.Instance != null)
            {
                if(Autosaving && PlayerManager.Instance.GetPlayers().Count == 0 && DisableAutosaveOnNoPlayers)
                {
                    Log.Warn("No players online. Autosave disabled! It won't be enabled automatically!!!"); // TODO: Make it auto-enable, when a player joins.
                    DisablePeriodicSaving();
                    Console.Beep(900, 5000); //5-second beep to warn an owner (if he/she uses computer at the moment) //TODO: Make it possibe to toggle this off.
                }
                else if (PlayerManager.Instance.GetPlayers().Count == 0 && !Autosaving)
                {
                    Log.Info("Autosave still disabled due to lack of players! It won't be enabled automatically!!!");
                }
            }
        }

        public void Start()
        {
            IsRunning = true;
            IpLogger.PrintServerIps();
            server.Start();
            Log.Info("Nitrox Server Started");
            EnablePeriodicSaving();
        }

        public void Stop()
        {
            Log.Info("Nitrox Server Stopping...");
            DisablePeriodicSaving();
            Save();
            server.Stop();
            Log.Info("Nitrox Server Stopped");
            IsRunning = false;
        }

        private void EnablePeriodicSaving()
        {
            Log.Info("Enabling periodic saving...");
            Autosaving = true;
            saveTimer.Start();
        }

        private void DisablePeriodicSaving()
        {
            saveTimer.Stop();
            Autosaving = false;
            Log.Info("Periodic saving disabled.");
        }
    }
}
