using ProjectKillersCommon;
using ProjectKillersCommon.Data;
using ProjectKillersServer.Events;
using SwiftKernelServerProject;

namespace ProjectKillersServer.RequestHandlers {
    public class CreateRoomHandler {
        public static void DoHandle(NetData data, Client client, string networkID) {
            Room room = data.Values["room"].ObjectValue as Room;
            Server.Rooms.Add(room);

            SendRoomsHandler.DoHandle("EventLobbyHolderOnGetRooms");
        }
    }
}
