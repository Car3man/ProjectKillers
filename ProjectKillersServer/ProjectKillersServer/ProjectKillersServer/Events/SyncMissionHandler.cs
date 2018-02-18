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

            foreach(Client client in EntryPoint.Clients) {
                if(client != null && client.Actualy) {
                    NetDataEvent allResponse = new NetDataEvent(EventTypes.SyncMission, new Dictionary<string, ObjectWrapper>() { { "mission", new ObjectWrapper<BaseMission>(client.MissionFirstInited ? mission : missionChanges) } });
                    byte[] sendData = Utils.ToBytesJSON(allResponse);
                    EntryPoint.SendEvent(new List<Client>() { client }, sendData, "GameManagerHandleSyncMission");

                    client.MissionFirstInited = true;
                }
            }
        }
    }
}
