using ProjectKillersCommon;
using ProjectKillersCommon.Data.Objects;
using ProjectKillersServer.Controllers.Objects;
using SwiftKernelCommon.Core;
using System.Collections.Generic;

namespace ProjectKillersServer.Controllers {
    public class ClientController {
        public Client Client;
        public NetPeer Peer;
        public bool Actualy = false;
        public bool MissionFirstInited = false;
        public BaseMissionController MissionController;

        public Dictionary<string, BaseMissionObjectController> ControlledObjects = new Dictionary<string, BaseMissionObjectController>();

        public ClientController(Client client, NetPeer peer) {
            Client = client;
            Peer = peer;
        }

        public PlayerObjectController CurrentPlayer {
            get {
                foreach (string key in ControlledObjects.Keys) {
                    if (ControlledObjects[key] is PlayerObjectController) {
                        return ControlledObjects[key] as PlayerObjectController;
                    }
                }
                return null;
            }
        }

        public void SetMissionController(BaseMissionController missionController) {
            MissionFirstInited = false;
            MissionController = missionController;
        }
    }
}
