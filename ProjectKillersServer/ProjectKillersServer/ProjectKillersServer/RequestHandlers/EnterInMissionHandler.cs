using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Missions;
using SwiftKernelServerProject;
using System;
using System.Collections.Generic;
using ProjectKillersCommon.Data.Objects;

namespace ProjectKillersServer.RequestHandlers {
    public static class EnterInMissionHandler {
        public static void DoHandle(NetData data, Client client, string networkID) {
            client.Actualy = true;

            List<Client> clients = new List<Client>(EntryPoint.Clients);
            clients.Remove(client);
            clients.RemoveAll(x => !x.Actualy);

            List<Client> actualyClients = new List<Client>(EntryPoint.Clients);
            actualyClients.RemoveAll(x => !x.Actualy);

            string id = (string)data.Values["id"].ObjectValue;

            client.ID = id;

            PlayerObject player = new PlayerObject(new Vector3K(0f, 0f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(3.2f, 3.2f, 3.2f), new Vector3K(0f, 0f, 0f));
            player.OwnerID = client.ID;

            EntryPoint.Mission.AddDynamicObject(player, EntryPoint.Physics.World);
            client.ControlledObjects.Add(player.ID, player);

            NetData response = new NetData(RequestTypes.EnterInMission, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(id) } });
            EntryPoint.SendResponse(client, Utils.ToBytesJSON(response), networkID);
        }
    }
}
