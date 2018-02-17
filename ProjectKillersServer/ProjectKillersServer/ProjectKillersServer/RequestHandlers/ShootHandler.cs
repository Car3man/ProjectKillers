using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Objects;
using ProjectKillersCommon.Extensions;
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

            Vector3K playerPos = client.Position;
            Vector3K playerRot = client.EulerAngles;

            Vector3K pos = new Vector3K(1.3F, -0.55F, 0).RotateVector2(playerRot.z);
            Vector3K rot = new Vector3K(0f, 0f, playerRot.z - 0.067F);

            pos.x += playerPos.x;
            pos.y += playerPos.y;

            BulletObject bullet = new BulletObject(pos, new Vector3K(0f, 0f, 0f), new Vector3K(0.28f, 0.09f, 0f), rot);
            bullet.Mission = EntryPoint.Mission;

            EntryPoint.Mission.AddDynamicObject(bullet, EntryPoint.Physics.World);

            NetData allResponse = new NetData(RequestTypes.Shoot, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(id) } });
            EntryPoint.SendResponse(clients, Utils.ToBytesJSON(allResponse), networkID);
        }
    }
}
