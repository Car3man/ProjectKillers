using ProjectKillersCommon.Data.Missions;
using ProjectKillersServer.Controllers;
using System;

namespace ProjectKillersServer.Factories {
    public static class MissionControllerFactory {
        public static BaseMissionController GetMission(BaseMission mission) {
            if(mission.GetType() == typeof(TestMission)) {
                return new TestMissionController(mission);
            } else {
                throw new Exception("unknow mission type " + mission.GetType().ToString());
            }
        }
    }
}
