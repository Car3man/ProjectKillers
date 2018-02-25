using Common;
using ProjectKillersCommon;
using ProjectKillersServer.Controllers;
using SwiftKernelServerProject;

namespace ProjectKillersServer.RequestHandlers {
    public class EnterInRoomHandler {
        public static void DoHandle(NetDataRequest data, ClientController client, string networkID) {
            RoomController room = Server.RoomControllers.Find(x => x.Room.ID.Equals(data.Values["id"].ObjectValue as string));
            room.AddClient(client);

            Server.SendResponse(client, Utils.ToBytesJSON(new NetDataRequest(RequestTypes.EnterInRoom, data.Values)), networkID);
            Events.SyncRoomHandler.DoHandle(room.Room.ID, "EventRoomHolderOnSyncRoom");
        }
    }
}
