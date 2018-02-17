using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Missions;
using SwiftKernelServerProject;
using System;
using System.Collections.Generic;

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

            NetData allResponse = new NetData(RequestTypes.EnterInMission, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(id) } });
            NetData playerResponse = new NetData(RequestTypes.EnterInMission, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(id) }, { "clients", new ObjectWrapper<List<Client>>(actualyClients) } });

            EntryPoint.SendResponse(clients, Utils.ToBytesJSON(allResponse), networkID);
            EntryPoint.SendResponse(new List<Client>() { client }, Utils.ToBytesJSON(playerResponse), networkID);
        }
    }
}
