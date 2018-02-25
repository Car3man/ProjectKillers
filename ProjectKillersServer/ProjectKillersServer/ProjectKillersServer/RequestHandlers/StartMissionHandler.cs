using Common;
using ProjectKillersCommon;
using ProjectKillersServer.Controllers;
using SwiftKernelServerProject;
using System.Collections.Generic;

namespace ProjectKillersServer.RequestHandlers {
    public class StartMissionHandler {
        public static void DoHandle(NetDataRequest data, ClientController client, string networkID) {
            RoomController room = Server.RoomControllers.Find(x => x.Room.ID.Equals(data.Values["id"].ObjectValue as string));
            room.MissionStarted = true;

            Server.SendEvent(room.Clients, Utils.ToBytesJSON(new NetDataEvent(EventTypes.StartMission, new Dictionary<string, ObjectWrapper>())), "EventMapManagerOnStartedMission");
        }
    }
}
