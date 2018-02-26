using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Data;
using ProjectKillersServer.Controllers;
using SwiftKernelServerProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectKillersServer.Events {
    public static class SyncRoomHandler {
        public static void DoHandle(string roomID) {
            RoomController room = Server.RoomControllers.Find(x => x.Room.ID.Equals(roomID));

            NetDataEvent allResponse = new NetDataEvent(EventTypes.SyncRoom, new Dictionary<string, ObjectWrapper>() { { "room", new ObjectWrapper<Room>(room.Room) } });
            Server.SendEvent(room.Clients, Utils.ToBytesJSON(allResponse), "EventRoomHolderOnSyncRoom");
        }
    }
}
