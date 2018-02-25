using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Data;
using ProjectKillersServer.Controllers;
using SwiftKernelServerProject;
using System.Collections.Generic;

namespace ProjectKillersServer.RequestHandlers {
    public class SyncRoomHandler {
        public static void DoHandle(NetDataRequest data, ClientController client, string networkID) {
            Room room = Server.GetRooms().Find(x => x.ID.Equals(data.Values["id"].ObjectValue as string));

            NetDataEvent allResponse = new NetDataEvent(EventTypes.SyncRoom, new Dictionary<string, ObjectWrapper>() { { "room", new ObjectWrapper<Room>(room) } });
            Server.SendResponse(client, Utils.ToBytesJSON(allResponse), networkID);
        }
    }
}
