using ProjectKillersCommon.Classes;
using ProtoBuf;
using SwiftKernelCommon.Core;
using System;
using System.Collections.Generic;
using ProjectKillersCommon.Data.Objects;
using ProjectKillersCommon.Data.Missions;

namespace ProjectKillersCommon {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class Client {
        public NetPeer Peer;
        public string ID;
        public bool Actualy = false;
        public bool MissionFirstInited = false;
        public BaseMission Mission;

        public Dictionary<string, BaseMissionObject> ControlledObjects = new Dictionary<string, BaseMissionObject>();

        public PlayerObject CurrentPlayer {
            get {
                foreach (string key in ControlledObjects.Keys) {
                    if (ControlledObjects[key] is PlayerObject) {
                        return ControlledObjects[key] as PlayerObject;
                    }
                }
                return null;
            }
        }

        public Client(NetPeer peer, string id) {
            Peer = peer;
            ID = id;
            Actualy = false;
        }

        public Client(NetPeer peer) {
            Peer = peer;
        }

        public void SetMission(BaseMission mission) {
            MissionFirstInited = false;
            Mission = mission;
        }
    }
}
