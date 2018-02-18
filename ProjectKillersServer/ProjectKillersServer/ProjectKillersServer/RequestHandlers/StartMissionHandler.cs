using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Data;
using SwiftKernelServerProject;
using System.Collections.Generic;

namespace ProjectKillersServer.RequestHandlers {
    public class StartMissionHandler {
        public static void DoHandle(NetData data, Client client, string networkID) {
            Room room = Server.Rooms.Find(x => x.ID.Equals(data.Values["id"].ObjectValue as string));
            Server.SendEvent(room.Clients, Utils.ToBytesJSON(new NetDataEvent(EventTypes.StartMission, new Dictionary<string, ObjectWrapper>())), "EventMapManagerOnStartedMission");
        }
    }
}
