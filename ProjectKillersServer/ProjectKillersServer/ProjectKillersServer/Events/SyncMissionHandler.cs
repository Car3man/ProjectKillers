using System;
using System.Collections.Generic;
using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Data.Missions;
using SwiftKernelServerProject;

namespace ProjectKillersServer.Events {
    public static class SyncMissionHandler {
        public static void Update(float deltaTime) {
            DoSync();
        }

        public static void DoSync () {
            lock (Server.ClientsLock) {
                for (int i = 0; i < Server.ClientControllers.Count; i++) {
                    if (Server.ClientControllers[i].MissionController == null) continue;

                    BaseMission mission = Server.ClientControllers[i].MissionController.Mission;

                    if (Server.ClientControllers[i] != null && Server.ClientControllers[i].Actualy) {
                        NetDataEvent allResponse = new NetDataEvent(EventTypes.SyncMission, new Dictionary<string, ObjectWrapper>() { { "mission", new ObjectWrapper<BaseMission>(mission) } });

                        lock(mission) {
                            Server.SendEvent(Server.ClientControllers[i], Utils.ToBytesJSON(allResponse), "EventGameManagerHandleSyncMission");
                        }

                        Server.ClientControllers[i].MissionFirstInited = true;
                    }
                }
            }
        }
    }
}
