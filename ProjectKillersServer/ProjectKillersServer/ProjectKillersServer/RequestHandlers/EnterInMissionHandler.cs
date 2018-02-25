using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Missions;
using SwiftKernelServerProject;
using System;
using System.Collections.Generic;
using ProjectKillersCommon.Data.Objects;
using ProjectKillersCommon.Data;
using ProjectKillersServer.Controllers;
using ProjectKillersServer.Controllers.Objects;

namespace ProjectKillersServer.RequestHandlers {
    public static class EnterInMissionHandler {
        public static void DoHandle(NetDataRequest data, ClientController client, string networkID) {
            client.Actualy = true;

            List<ClientController> clients = new List<ClientController>(Server.ClientControllers);
            clients.Remove(client);
            clients.RemoveAll(x => !x.Actualy);

            List<ClientController> actualyClients = new List<ClientController>(Server.ClientControllers);
            actualyClients.RemoveAll(x => !x.Actualy);

            string id = (string)data.Values["id"].ObjectValue;

            RoomController clientRoom = Server.GetClientRoom(client);

            client.Client.ID = id;
            client.SetMissionController(MissionDispenser.GetMission(clientRoom.Room.ID, clientRoom.Room.MissionName));

            PlayerObject player = new PlayerObject(new Vector3K(0f, 0f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(2f, 2f, 2f), new Vector3K(0f, 0f, 0f));
            player.OwnerID = client.Client.ID;

            PlayerObjectController playerController = new PlayerObjectController(player);
            playerController.MissionController = client.MissionController;
 

            client.MissionController.AddDynamicObject(playerController, client.MissionController.Physics.World);
            client.ControlledObjects.Add(player.ID, playerController);

            NetDataRequest response = new NetDataRequest(RequestTypes.EnterInMission, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(id) } });
            Server.SendResponse(client, Utils.ToBytesJSON(response), networkID);
        }
    }
}
