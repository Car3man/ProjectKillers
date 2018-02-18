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
            BaseMission mission = EntryPoint.Mission;
            BaseMission missionChanges = mission.GetMissionChanges();

            lock (EntryPoint.ClientsLock) {
                for (int i = 0; i < EntryPoint.Clients.Count; i++) {
                    if (EntryPoint.Clients[i] != null && EntryPoint.Clients[i].Actualy) {
                        NetDataEvent allResponse = new NetDataEvent(EventTypes.SyncMission, new Dictionary<string, ObjectWrapper>() { { "mission", new ObjectWrapper<BaseMission>(EntryPoint.Clients[i].MissionFirstInited ? mission : missionChanges) } });

                        lock(EntryPoint.Clients[i].MissionFirstInited ? mission.Locker : missionChanges.Locker) {
                            EntryPoint.SendEvent(EntryPoint.Clients[i], Utils.ToBytesJSON(allResponse), "GameManagerHandleSyncMission");
                        }

                        EntryPoint.Clients[i].MissionFirstInited = true;
                    }
                }
            }
        }
    }
}
