using Common;
using ProjectKillersCommon;
using ProjectKillersServer;
using ProjectKillersServer.Events;
using ProjectKillersServer.RequestHandlers;
using SwiftKernel.Core;
using SwiftKernelCommon.Core;
using System;
using System.Collections.Generic;
using ProjectKillersCommon.Data;
using ProjectKillersServer.Controllers;

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

        public static List<ClientController> ClientControllers = new List<ClientController>();
        public static List<RoomController> RoomControllers = new List<RoomController>();

        public static List<Room> GetRooms() {
            List<Room> rooms = new List<Room>();
            foreach(RoomController r in RoomControllers) {
                rooms.Add(r.Room);
            }
            return rooms;
        }

        public static List<Room> GetRooms(List<RoomController> roomControllers) {
            List<Room> rooms = new List<Room>();
            foreach (RoomController r in roomControllers) {
                rooms.Add(r.Room);
            }
            return rooms;
        }

        public static List<Client> GetClients() {
            List<Client> clients = new List<Client>();
            foreach (ClientController c in ClientControllers) {
                clients.Add(c.Client);
            }
            return clients;
        }

        public static List<Client> GetClients(List<ClientController> clientControllers) {
            List<Client> clients = new List<Client>();
            foreach (ClientController c in clientControllers) {
                clients.Add(c.Client);
            }
            return clients;
        }

        private static void Main(string[] args) {
            Updater = new ServerUpdater();
            Updater.OnUpdate += SyncMissionHandler.Update;

            SKServer = new SwiftKernelServer();
            SKServer.Setup(6000, "pkillers");

            SKServer.OnPeerConnected += Server_OnPeerConnected;
            SKServer.OnPeerDisconnected += Server_OnPeerDisconnected;
            SKServer.OnRequestReceived += Server_OnRequestReceived;

            RoomControllers.Add(new RoomController(new Room("Test Room", null)));

            SKServer.Start();
        }

        private static void Server_OnPeerConnected(NetPeer peer) {
            Console.WriteLine("Peer connected with ip {0}", peer.EndPoint.Host);

            ClientControllers.Add(new ClientController(new Client(), peer));

            lock (ClientsLock) {
                ClientControllers.Add(new ClientController(new Client(), peer));
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
            ClientController client = null;

            lock (ClientsLock) {
                client = ClientControllers.Find(x => x.Peer == peer);
            }

            if(client == null) return;

            NetDataRequest ndata = Utils.FromBytesJSON<NetDataRequest>(data);

            if(string.IsNullOrEmpty(client.Client.Nickname)) {
                switch (ndata.Type) {
                    case RequestTypes.Login: LoginHandler.DoHandle(ndata, client, networkID); break;
                }
            } else {
                switch (ndata.Type) {
                    case RequestTypes.EnterInMission: EnterInMissionHandler.DoHandle(ndata, client, networkID); break;
                    case RequestTypes.SyncPlayer: SyncPlayerHandler.DoHandle(ndata, client, networkID); break;
                    case RequestTypes.Shoot: ShootHandler.DoHandle(ndata, client, networkID); break;
                    case RequestTypes.GetRooms: GetRoomsHandler.DoHandle(ndata, client, networkID); break;
                    case RequestTypes.CreateRoom: CreateRoomHandler.DoHandle(ndata, client, networkID); break;
                    case RequestTypes.EnterInRoom: EnterInRoomHandler.DoHandle(ndata, client, networkID); break;
                    case RequestTypes.StartMission: StartMissionHandler.DoHandle(ndata, client, networkID); break;
                    case RequestTypes.LeaveRoom: LeaveRoomHandler.DoHandle(ndata, client, networkID); break;
                    case RequestTypes.SyncRoom: ProjectKillersServer.RequestHandlers.SyncRoomHandler.DoHandle(ndata, client, networkID); break;
                }
            }
        }

        #region Public API
         
        public static void SendResponse(List<ClientController> clients, byte[] data, string networkID = "") {
            foreach(ClientController c in clients) {
                SKServer.SendResponse(c.Peer, data, networkID);
            }
        }

        public static void SendEvent(List<ClientController> clients, byte[] data, string networkID = "") {
            foreach(ClientController c in clients) {
                SKServer.SendEvent(c.Peer, data, networkID);
            }
        }

        public static void SendResponse(ClientController client, byte[] data, string networkID = "") {
            SKServer.SendResponse(client.Peer, data, networkID);
        }

        public static void SendEvent(ClientController client, byte[] data, string networkID = "") {
            SKServer.SendEvent(client.Peer, data, networkID);
        }

        public static RoomController GetClientRoom(ClientController client) {
            foreach(RoomController r in RoomControllers) {
                if (r.Clients.Contains(client)) {
                    return r;
                }
            }
            throw new Exception("no found client room");
        }

        #endregion
    }
}
