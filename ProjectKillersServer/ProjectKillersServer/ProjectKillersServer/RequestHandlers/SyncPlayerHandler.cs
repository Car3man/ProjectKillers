using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Classes;
using SwiftKernelServerProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectKillersServer.RequestHandlers {
    public static class SyncPlayerHandler {
        public static void DoHandle(NetData data, Client client, string networkID) {
            List<Client> clients = new List<Client>(EntryPoint.Clients);
            clients.RemoveAll(x => !x.Actualy);

            string id = (string)data.Values["id"].ObjectValue;
            Vector3K position = (Vector3K)data.Values["position"].ObjectValue;
            Vector3K eulerAngles = (Vector3K)data.Values["eulerAngles"].ObjectValue;

            client.Position = position;
            client.EulerAngles = eulerAngles;

            NetData allResponse = new NetData(RequestTypes.SyncPlayer, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(id) }, { "position", new ObjectWrapper<Vector3K>(position) }, { "eulerAngles", new ObjectWrapper<Vector3K>(eulerAngles) } });

            EntryPoint.SendResponse(clients, Utils.ToBytesJSON(allResponse), networkID);
        }
    }
}
