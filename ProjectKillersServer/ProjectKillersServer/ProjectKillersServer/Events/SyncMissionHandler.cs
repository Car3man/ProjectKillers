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
            foreach(Client client in EntryPoint.Clients) {
                if(client.Actualy) {
                    BaseMission mission = EntryPoint.Mission;
                    if (client.MissionFirstInited) {
                        mission = mission.GetMissionChanges();
                    }

                    client.MissionFirstInited = true;

                    NetDataEvent allResponse = new NetDataEvent(EventTypes.SyncMission, new Dictionary<string, ObjectWrapper>() { { "mission", new ObjectWrapper<BaseMission>(mission) } });
                    byte[] sendData = Utils.ToBytesJSON(allResponse);
                    EntryPoint.SendEvent(new List<Client>() { client }, sendData, "GameManagerHandleSyncMission");
                }
            }
        }
    }
}
