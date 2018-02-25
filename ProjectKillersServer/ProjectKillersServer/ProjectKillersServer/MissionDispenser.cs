using ProjectKillersCommon.Data.Missions;
using ProjectKillersServer.Controllers;
using ProjectKillersServer.Factories;
using SwiftKernelServerProject;
using System;

namespace ProjectKillersServer {
    public static class MissionDispenser {

        public static BaseMissionController GetMission(string roomID, string missionName) {
            lock(Server.RoomControllers) {
                RoomController room = Server.RoomControllers.Find(x => x.Room.ID.Equals(roomID));
                if (room == null) throw new Exception(string.Format("unknow room with id: {0}", roomID));

                BaseMissionController missionController = room.MissionController;
                BaseMission mission = null;

                if (missionController != null) return missionController;

                switch (missionName) {
                    case "TestMission":
                        mission = new TestMission();
                        break;
                    default:
                        throw new Exception(string.Format("unknow mission name: {0}", missionName));
                }

                room.MissionController = MissionFactory.GetMissionController(mission, room);
                room.MissionController.Mission = mission;

                Server.Updater.OnUpdate += room.MissionController.Update;
                return room.MissionController;
            }
        }
    }
}
