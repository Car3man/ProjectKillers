using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Data;
using ProjectKillersServer.Controllers;
using SwiftKernelServerProject;
using System.Collections.Generic;

namespace ProjectKillersServer.RequestHandlers {
    public class GetRoomsHandler {
        public static void DoHandle(NetDataRequest data, ClientController client, string networkID) {
            NetDataRequest response = new NetDataRequest(RequestTypes.EnterInMission, new Dictionary<string, ObjectWrapper>() { { "rooms", new ObjectWrapper<List<Room>>(Server.GetRooms()) } });
            Server.SendResponse(client, Utils.ToBytesJSON(response), networkID);
        }
    }
}
