using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Objects;
using SwiftKernelServerProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectKillersServer.RequestHandlers {
    public static class ShootHandler {
        public static void DoHandle(NetData data, Client client, string networkID) {
            List<Client> clients = new List<Client>(EntryPoint.Clients);
            clients.RemoveAll(x => !x.Actualy);

            string id = (string)data.Values["id"].ObjectValue;
            Vector3K pos = (Vector3K)data.Values["position"].ObjectValue;
            Vector3K eulerAngles = (Vector3K)data.Values["eulerAngles"].ObjectValue;

            BulletObject bullet = new BulletObject(pos, new Vector3K(0f, 0f, 0f), new Vector3K(0.28f, 0.09f, 0f), eulerAngles);
            bullet.Mission = EntryPoint.Mission;

            EntryPoint.Mission.AddDynamicObject(bullet);

            NetData allResponse = new NetData(RequestTypes.Shoot, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(id) } });
            EntryPoint.SendResponse(clients, Utils.ToBytesJSON(allResponse), networkID);
        }
    }
}
