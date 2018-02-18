using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Data;
using SwiftKernelServerProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectKillersServer.RequestHandlers {
    public class SyncRoomHandler {
        public static void DoHandle(NetData data, Client client, string networkID) {
            Room room = Server.Rooms.Find(x => x.ID.Equals(data.Values["id"].ObjectValue as string));

            NetDataEvent allResponse = new NetDataEvent(EventTypes.SyncRoom, new Dictionary<string, ObjectWrapper>() { { "room", new ObjectWrapper<Room>(room) } });
            Server.SendResponse(client, Utils.ToBytesJSON(allResponse), networkID);
        }
    }
}
