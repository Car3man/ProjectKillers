using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Data;
using SwiftKernelServerProject;
using System.Collections.Generic;

namespace ProjectKillersServer.RequestHandlers {
    public class GetRoomsHandler {
        public static void DoHandle(NetData data, Client client, string networkID) {
            NetData response = new NetData(RequestTypes.EnterInMission, new Dictionary<string, ObjectWrapper>() { { "rooms", new ObjectWrapper<List<Room>>(Server.Rooms) } });
            Server.SendResponse(client, Utils.ToBytesJSON(response), networkID);
        }
    }
}
