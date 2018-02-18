using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Missions;
using ProjectKillersCommon.Data.Objects;
using ProjectKillersServer;
using ProjectKillersServer.Events;
using ProjectKillersServer.RequestHandlers;
using SwiftKernel.Core;
using SwiftKernelCommon.Core;
using System;
using System.Collections.Generic;
using ProjectKillersServer.Physics;
using ProjectKillersCommon.Data;

namespace SwiftKernelServerProject {
    public class Server {
        private static object clientsLock = new object();
        public static object ClientsLock {
            get {
                return clientsLock;
            }
        }

        public static ServerUpdater Updater { get; private set; }
        public static SwiftKernelServer SKServer { get; private set; }

        public static List<Client> Clients = new List<Client>();
        public static List<Room> Rooms = new List<Room>();

        //PHYSICS
        public static Physics Physics { get; private set; }
        public static PhysicsSolver PhysicsSolver { get; private set; }
        public static CollisionHandler CollisionHandler { get; private set; }

        private static void Main(string[] args) {
            Updater = new ServerUpdater();

            CollisionHandler = new CollisionHandler();

            PhysicsSolver = new PhysicsSolver();
            PhysicsSolver.OnAdd += CollisionHandler.OnCollide;

            Physics = new Physics(-1100, -1100, 1100, 1100, 0, 0, false);
            Physics.SetSolver(PhysicsSolver);

            Updater.OnUpdate += SyncMissionHandler.Update;
            Updater.OnUpdate += Physics.Update;

            SKServer = new SwiftKernelServer();
            SKServer.Setup(6000, "pkillers");

            SKServer.OnPeerConnected += Server_OnPeerConnected;
            SKServer.OnPeerDisconnected += Server_OnPeerDisconnected;
            SKServer.OnRequestReceived += Server_OnRequestReceived;

            Rooms.Add(new Room("Test Room", null));

            SKServer.Start();
        }

        private static void Server_OnPeerConnected(NetPeer peer) {
            Console.WriteLine("Peer connected with ip {0}", peer.EndPoint.Host);

            Clients.Add(new Client(peer));

            lock (ClientsLock) {
                Clients.Add(new Client(peer));
            }
        }

        private static void Server_OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
            //Console.WriteLine("Peer disconnected with ip {0}", peer.EndPoint.Host);

            //Client client = null;

            //lock (ClientsLock) {
            //    client = Clients.Find(x => x.Peer == peer);
            //}

            //lock (Mission.Locker) {
            //    foreach (string k in client.ControlledObjects.Keys) {
            //        if (Mission.DynamicObjects.ContainsKey(k)) Mission.DynamicObjects[k].Destroy();
            //    }
            //}

            //LeaveMissionHandler.DoHandle(client, "EventGameManagerHandleLeaveMission");

            //lock (ClientsLock) {
            //    Clients.Remove(client);
            //}
        }

        private static void Server_OnRequestReceived(NetPeer peer, byte[] data, string networkID) {
            Client client = null;

            lock (ClientsLock) {
                client = Clients.Find(x => x.Peer == peer);
            }

            if(client == null) return;

            NetData ndata = Utils.FromBytesJSON<NetData>(data);

            switch (ndata.Type) {
                case RequestTypes.EnterInMission: EnterInMissionHandler.DoHandle(ndata, client, networkID); break;
                case RequestTypes.SyncPlayer: SyncPlayerHandler.DoHandle(ndata, client, networkID); break;
                case RequestTypes.Shoot: ShootHandler.DoHandle(ndata, client, networkID); break;
                case RequestTypes.InteractObject: InteractObjectHandler.DoHandle(ndata, client, networkID); break;
                case RequestTypes.GetRooms: GetRoomsHandler.DoHandle(ndata, client, networkID); break;
                case RequestTypes.CreateRoom: CreateRoomHandler.DoHandle(ndata, client, networkID); break;
                case RequestTypes.EnterInRoom: EnterInRoomHandler.DoHandle(ndata, client, networkID); break;
                case RequestTypes.StartMission: StartMissionHandler.DoHandle(ndata, client, networkID); break;
            }
        }

        #region Public API
         
        public static void SendResponse(List<Client> clients, byte[] data, string networkID = "") {
            foreach(Client c in clients) {
                SKServer.SendResponse(c.Peer, data, networkID);
            }
        }

        public static void SendEvent(List<Client> clients, byte[] data, string networkID = "") {
            foreach(Client c in clients) {
                SKServer.SendEvent(c.Peer, data, networkID);
            }
        }

        public static void SendResponse(Client client, byte[] data, string networkID = "") {
            SKServer.SendResponse(client.Peer, data, networkID);
        }

        public static void SendEvent(Client client, byte[] data, string networkID = "") {
            SKServer.SendEvent(client.Peer, data, networkID);
        }

        #endregion
    }
}
