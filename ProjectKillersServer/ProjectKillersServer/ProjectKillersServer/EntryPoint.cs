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

namespace SwiftKernelServerProject {
    public class EntryPoint {
        private static ServerUpdater updater;
        public static SwiftKernelServer Server = null;

        public static List<Client> Clients = new List<Client>();
        public static TestMission Mission = new TestMission();

        private static void Main(string[] args) {
            updater = new ServerUpdater();

            Mission = new TestMission();
            Mission.Objects = new Dictionary<string, BaseMissionObject>();

            TestObject testObj1 = new TestObject(new Vector3K(2f, 5f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(1f, 1f, 1f), new Vector3K(0f, 0f, 0f));
            testObj1.Mission = Mission;
            TestObject testObj2 = new TestObject(new Vector3K(2f, 4f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(1f, 1f, 1f), new Vector3K(0f, 0f, 0f));
            testObj2.Mission = Mission;
            TestObject testObj3 = new TestObject(new Vector3K(2f, 3f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(1f, 1f, 1f), new Vector3K(0f, 0f, 0f));
            testObj3.Mission = Mission;
            TestObject testObj4 = new TestObject(new Vector3K(2f, 2f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(1f, 1f, 1f), new Vector3K(0f, 0f, 0f));
            testObj4.Mission = Mission;
            TestObject testObj5 = new TestObject(new Vector3K(2f, 1f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(1f, 1f, 1f), new Vector3K(0f, 0f, 0f));
            testObj5.Mission = Mission;
            TestObject testObj6 = new TestObject(new Vector3K(2f, 0f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(1f, 1f, 1f), new Vector3K(0f, 0f, 0f));
            testObj6.Mission = Mission;
            TestObject testObj7 = new TestObject(new Vector3K(2f, -1f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(1f, 1f, 1f), new Vector3K(0f, 0f, 0f));
            testObj7.Mission = Mission;
            TestObject testObj8 = new TestObject(new Vector3K(2f, -2f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(1f, 1f, 1f), new Vector3K(0f, 0f, 0f));
            testObj8.Mission = Mission;
            TestObject testObj9 = new TestObject(new Vector3K(2f, -3f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(1f, 1f, 1f), new Vector3K(0f, 0f, 0f));
            testObj9.Mission = Mission;
            TestObject testObj10 = new TestObject(new Vector3K(2f, -4f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(1f, 1f, 1f), new Vector3K(0f, 0f, 0f));
            testObj10.Mission = Mission;

            Mission.Objects.Add(testObj1.ID, testObj1);
            Mission.Objects.Add(testObj2.ID, testObj2);
            Mission.Objects.Add(testObj3.ID, testObj3);
            Mission.Objects.Add(testObj4.ID, testObj4);
            Mission.Objects.Add(testObj5.ID, testObj5);
            Mission.Objects.Add(testObj6.ID, testObj6);
            Mission.Objects.Add(testObj7.ID, testObj7);
            Mission.Objects.Add(testObj8.ID, testObj8);
            Mission.Objects.Add(testObj9.ID, testObj9);
            Mission.Objects.Add(testObj10.ID, testObj10);

            updater.OnUpdate += Mission.Update;

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
                case RequestTypes.SyncMission: SyncMissionHandler.DoHandle(ndata, client, networkID); break;
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
