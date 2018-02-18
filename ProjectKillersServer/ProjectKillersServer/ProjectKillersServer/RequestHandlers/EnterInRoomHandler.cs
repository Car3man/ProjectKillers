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
    public class EnterInRoomHandler {
        public static void DoHandle(NetData data, Client client, string networkID) {
            Room room = Server.Rooms.Find(x => x.ID.Equals(data.Values["id"].ObjectValue as string));
            room.Clients.Add(client);

            Server.SendResponse(client, Utils.ToBytesJSON(new NetData(RequestTypes.EnterInRoom, data.Values)), networkID);
        }
    }
}
