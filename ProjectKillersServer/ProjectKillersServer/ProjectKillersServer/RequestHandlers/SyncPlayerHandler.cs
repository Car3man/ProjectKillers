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
        public static void DoHandle(NetDataRequest data, Client client, string networkID) {
            List<Client> clients = new List<Client>(Server.Clients);
            clients.RemoveAll(x => !x.Actualy);

            string id = (string)data.Values["id"].ObjectValue;

            if (!client.Mission.DynamicObjects.ContainsKey(id)) return;
            if (client.CurrentPlayer == null) return;

            Vector3K position = (Vector3K)data.Values["position"].ObjectValue;
            Vector3K eulerAngles = (Vector3K)data.Values["eulerAngles"].ObjectValue;

            client.CurrentPlayer.SetTransform(position, eulerAngles);
        }
    }
}
