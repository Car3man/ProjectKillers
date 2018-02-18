using ProjectKillersCommon.Data;
using ProjectKillersCommon.Data.Missions;
using SwiftKernelServerProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectKillersServer {
    public static class MissionDispenser {

        public static BaseMission GetMission(string roomID, string missionName) {
            lock(Server.Rooms) {
                Room room = Server.Rooms.Find(x => x.ID.Equals(roomID));
                if (room == null) throw new Exception(string.Format("unknow room with id: {0}", roomID));

                BaseMission mission = room.Mission;

                if (mission != null) return mission;

                switch (missionName) {
                    case "TestMission":
                        mission = new TestMission(true);
                        break;
                    default:
                        throw new Exception(string.Format("unknow mission name: {0}", missionName));
                }

                room.Mission = mission;
                Server.Updater.OnUpdate += room.Mission.Update;
                return mission;
            }
        }
    }
}
