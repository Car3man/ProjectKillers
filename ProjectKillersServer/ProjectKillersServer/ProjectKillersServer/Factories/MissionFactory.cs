using ProjectKillersCommon.Data.Missions;
using ProjectKillersServer.Controllers;
using System;

namespace ProjectKillersServer.Factories {
    public static class MissionFactory {
        public static BaseMissionController GetMissionController(BaseMission mission, RoomController roomController) {
            BaseMissionController result = null;

            if(mission.GetType() == typeof(TestMission)) {
                result = new TestMissionController(mission);
            } else {
                throw new Exception("unknow mission type " + mission.GetType().ToString());
            }

            if (result != null) result.RoomController = roomController;
            return result;
        }
    }
}
