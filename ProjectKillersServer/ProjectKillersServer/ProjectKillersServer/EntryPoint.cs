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

namespace SwiftKernelServerProject {
    public class EntryPoint {
        private static ServerUpdater updater;
        public static SwiftKernelServer Server = null;

        public static List<Client> Clients = new List<Client>();
        public static TestMission Mission = new TestMission();

        //PHYSICS
        public static Physics Physics;
        public static PhysicsSolver PhysicsSolver;
        public static CollisionHandler CollisionHandler;

        private static void Main(string[] args) {
            updater = new ServerUpdater();

            CollisionHandler = new CollisionHandler();

            PhysicsSolver = new PhysicsSolver();
            PhysicsSolver.OnAdd += CollisionHandler.OnCollide;

            Physics = new Physics(-1100, -1100, 1100, 1100, 0, 0, false);
            Physics.SetSolver(PhysicsSolver);

            Mission = new TestMission();
            Mission.Objects = new Dictionary<string, BaseMissionObject>();

            updater.OnUpdate += Mission.Update;
            updater.OnUpdate += SyncMissionHandler.Update;
            updater.OnUpdate += Physics.Update;

            TestObject testObj1 = new TestObject(new Vector3K(2f, 5f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.64f, 0.64f, 0.64f), new Vector3K(0f, 0f, 0f));
            TestObject testObj2 = new TestObject(new Vector3K(2f, 4f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.64f, 0.64f, 0.64f), new Vector3K(0f, 0f, 0f));
            TestObject testObj3 = new TestObject(new Vector3K(2f, 3f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.64f, 0.64f, 0.64f), new Vector3K(0f, 0f, 0f));
            TestObject testObj4 = new TestObject(new Vector3K(2f, 2f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.64f, 0.64f, 0.64f), new Vector3K(0f, 0f, 0f));
            TestObject testObj5 = new TestObject(new Vector3K(2f, 1f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.64f, 0.64f, 0.64f), new Vector3K(0f, 0f, 0f));
            TestObject testObj6 = new TestObject(new Vector3K(2f, 0f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.64f, 0.64f, 0.64f), new Vector3K(0f, 0f, 0f));
            TestObject testObj7 = new TestObject(new Vector3K(2f, -1f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.64f, 0.64f, 0.64f), new Vector3K(0f, 0f, 0f));
            TestObject testObj8 = new TestObject(new Vector3K(2f, -2f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.64f, 0.64f, 0.64f), new Vector3K(0f, 0f, 0f));
            TestObject testObj9 = new TestObject(new Vector3K(2f, -3f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.64f, 0.64f, 0.64f), new Vector3K(0f, 0f, 0f));
            TestObject testObj10 = new TestObject(new Vector3K(2f, -4f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.64f, 0.64f, 0.64f), new Vector3K(0f, 0f, 0f));

            Mission.AddObject(testObj1, Physics.World);
            Mission.AddObject(testObj2, Physics.World);
            Mission.AddObject(testObj3, Physics.World);
            Mission.AddObject(testObj4, Physics.World);
            Mission.AddObject(testObj5, Physics.World);
            Mission.AddObject(testObj6, Physics.World);
            Mission.AddObject(testObj7, Physics.World);
            Mission.AddObject(testObj8, Physics.World);
            Mission.AddObject(testObj9, Physics.World);
            Mission.AddObject(testObj10, Physics.World);

            Server = new SwiftKernelServer();
            Server.Setup(6000, "pkillers");

            Server.OnPeerConnected += Server_OnPeerConnected;
            Server.OnPeerDisconnected += Server_OnPeerDisconnected;
            Server.OnRequestReceived += Server_OnRequestReceived;

            Server.Start();
        }

        private static void Server_OnPeerConnected(NetPeer peer) {
            Console.WriteLine("Peer connected with ip {0}", peer.EndPoint.Host);

            Clients.Add(new Client(peer));
        }

        private static void Server_OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
            Console.WriteLine("Peer disconnected with ip {0}", peer.EndPoint.Host);

            Client client = Clients.Find(x => x.Peer == peer);

            lock (Mission.DynamicObjectsLock) {
                foreach (string k in client.ControlledObjects.Keys) {
                    if (Mission.DynamicObjects.ContainsKey(k)) Mission.DynamicObjects[k].Destroy();
                }
            }

            LeaveMissionHandler.DoHandle(client, "GameManagerHandleLeaveMission");

            Clients.Remove(client);
        }

        private static void Server_OnRequestReceived(NetPeer peer, byte[] data, string networkID) {
            Client client = Clients.Find(x => x.Peer == peer);

            if(client == null) return;

            NetData ndata = Utils.FromBytesJSON<NetData>(data);

            switch (ndata.Type) {
                case RequestTypes.EnterInMission: EnterInMissionHandler.DoHandle(ndata, client, networkID); break;
                case RequestTypes.SyncPlayer: SyncPlayerHandler.DoHandle(ndata, client, networkID); break;
                case RequestTypes.Shoot: ShootHandler.DoHandle(ndata, client, networkID); break;
                case RequestTypes.InteractObject: InteractObjectHandler.DoHandle(ndata, client, networkID); break;
            }
        }

        #region Public API
         
        public static void SendResponse(List<Client> clients, byte[] data, string networkID = "") {
            foreach(Client c in clients) {
                Server.SendResponse(c.Peer, data, networkID);
            }
        }

        public static void SendEvent(List<Client> clients, byte[] data, string networkID = "") {
            foreach(Client c in clients) {
                Server.SendEvent(c.Peer, data, networkID);
            }
        }

        #endregion
    }
}
