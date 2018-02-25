using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Data;
using ProjectKillersServer.Events;
using SwiftKernelServerProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectKillersServer.RequestHandlers {
    public class LeaveRoomHandler {
        public static void DoHandle(NetDataRequest data, Client client, string networkID) {
            Room room = Server.Rooms.Find(x => x.ID.Equals(data.Values["id"].ObjectValue as string));
            room.Clients.Remove(client);

            Server.SendResponse(client, Utils.ToBytesJSON(new NetDataRequest(RequestTypes.LeaveRoom, data.Values)), networkID);
            Events.SyncRoomHandler.DoHandle(room.ID, "EventRoomHolderOnSyncRoom");
        }
    }
}
