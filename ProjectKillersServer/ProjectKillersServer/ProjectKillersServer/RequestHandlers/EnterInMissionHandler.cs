using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Missions;
using SwiftKernelServerProject;
using System;
using System.Collections.Generic;
using ProjectKillersCommon.Data.Objects;
using ProjectKillersCommon.Data;

namespace ProjectKillersServer.RequestHandlers {
    public static class EnterInMissionHandler {
        public static void DoHandle(NetData data, Client client, string networkID) {
            client.Actualy = true;

            List<Client> clients = new List<Client>(Server.Clients);
            clients.Remove(client);
            clients.RemoveAll(x => !x.Actualy);

            List<Client> actualyClients = new List<Client>(Server.Clients);
            actualyClients.RemoveAll(x => !x.Actualy);

            string id = (string)data.Values["id"].ObjectValue;

            Room clientRoom = Server.GetClientRoom(client);

            client.ID = id;
            client.SetMission(MissionDispenser.GetMission(clientRoom.ID, clientRoom.MissionName));

            PlayerObject player = new PlayerObject(new Vector3K(0f, 0f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(2f, 2f, 2f), new Vector3K(0f, 0f, 0f));
            player.OwnerID = client.ID;

            client.Mission.AddDynamicObject(player, client.Mission.Physics.World);
            client.ControlledObjects.Add(player.ID, player);

            NetData response = new NetData(RequestTypes.EnterInMission, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(id) } });
            Server.SendResponse(client, Utils.ToBytesJSON(response), networkID);
        }
    }
}
