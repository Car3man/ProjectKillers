using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Data;
using SwiftKernelServerProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectKillersServer.Events {
    public static class SyncRoomHandler {
        public static void DoHandle(string roomID, string networkID) {
            Room room = Server.Rooms.Find(x => x.ID.Equals(roomID));

            NetDataEvent allResponse = new NetDataEvent(EventTypes.SyncRoom, new Dictionary<string, ObjectWrapper>() { { "room", new ObjectWrapper<Room>(room) } });
            Server.SendEvent(room.Clients, Utils.ToBytesJSON(allResponse), networkID);
        }
    }
}
