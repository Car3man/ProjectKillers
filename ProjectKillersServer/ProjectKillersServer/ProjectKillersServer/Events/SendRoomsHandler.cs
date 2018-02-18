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
    public static class SendRoomsHandler {
        public static void DoHandle(string networkID) {
            NetDataEvent allResponse = new NetDataEvent(EventTypes.SendRooms, new Dictionary<string, ObjectWrapper>() { { "rooms", new ObjectWrapper<List<Room>>(Server.Rooms) } });
            Server.SendEvent(Server.Clients, Utils.ToBytesJSON(allResponse), networkID);
        }
    }
}
