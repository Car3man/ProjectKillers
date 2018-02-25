using ProjectKillersCommon;
using ProjectKillersCommon.Classes;
using ProjectKillersServer.Controllers;
using SwiftKernelServerProject;
using System.Collections.Generic;

namespace ProjectKillersServer.RequestHandlers {
    public static class SyncPlayerHandler {
        public static void DoHandle(NetDataRequest data, ClientController client, string networkID) {
            List<ClientController> clients = new List<ClientController>(Server.ClientControllers);
            clients.RemoveAll(x => !x.Actualy);

            string id = (string)data.Values["id"].ObjectValue;

            if (!client.MissionController.DynamicObjects.ContainsKey(id)) return;
            if (client.CurrentPlayer == null) return;

            Vector3K position = (Vector3K)data.Values["position"].ObjectValue;
            Vector3K eulerAngles = (Vector3K)data.Values["eulerAngles"].ObjectValue;

            client.CurrentPlayer.SetTransform(position, eulerAngles);
        }
    }
}
