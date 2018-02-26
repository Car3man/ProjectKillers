using ProjectKillersCommon;
using ProjectKillersCommon.Data;
using ProjectKillersServer.Controllers;
using ProjectKillersServer.Events;
using SwiftKernelServerProject;

namespace ProjectKillersServer.RequestHandlers {
    public class CreateRoomHandler {
        public static void DoHandle(NetDataRequest data, ClientController client, string networkID) {
            Room room = data.Values["room"].ObjectValue as Room;
            Server.RoomControllers.Add(new RoomController(new Room(room.Name, client.Client.ID)));

            SendRoomsHandler.DoHandle();
        }
    }
}
