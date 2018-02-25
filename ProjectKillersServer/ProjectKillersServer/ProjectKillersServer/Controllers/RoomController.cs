using ProjectKillersCommon.Data;
using System.Collections.Generic;

namespace ProjectKillersServer.Controllers {
    public class RoomController {
        public Room Room;
        public BaseMissionController MissionController;

        public bool MissionStarted = false;

        public List<ClientController> Clients = new List<ClientController>();

        public RoomController(Room room) {
            Room = room;
        }

        public RoomController(Room room, BaseMissionController missionController) {
            Room = room;
            MissionController = missionController;
        }

        public void AddClient(ClientController client) {
            Clients.Add(client);
            Room.Clients.Add(client.Client);
        }

        public void RemoveClient(ClientController client) {
            Clients.Remove(client);
            Room.Clients.Remove(client.Client);
        }
    }
}
