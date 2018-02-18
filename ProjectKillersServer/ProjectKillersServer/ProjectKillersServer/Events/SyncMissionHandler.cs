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
                for (int i = 0; i < Server.Clients.Count; i++) {
                    if (Server.Clients[i].Mission == null) continue;

                    BaseMission mission = Server.Clients[i].Mission;
                    BaseMission missionChanges = mission.GetMissionChanges();

                    if (Server.Clients[i] != null && Server.Clients[i].Actualy) {
                        NetDataEvent allResponse = new NetDataEvent(EventTypes.SyncMission, new Dictionary<string, ObjectWrapper>() { { "mission", new ObjectWrapper<BaseMission>(Server.Clients[i].MissionFirstInited ? mission : missionChanges) } });

                        lock(Server.Clients[i].MissionFirstInited ? mission.Locker : missionChanges.Locker) {
                            Server.SendEvent(Server.Clients[i], Utils.ToBytesJSON(allResponse), "EventGameManagerHandleSyncMission");
                        }

                        Server.Clients[i].MissionFirstInited = true;
                    }
                }
            }
        }
    }
}
