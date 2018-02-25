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
        public static void DoHandle(NetDataRequest data, Client client, string networkID) {
            List<Client> clients = new List<Client>(Server.Clients);
            clients.RemoveAll(x => !x.Actualy);

            string id = (string)data.Values["id"].ObjectValue;

            Vector3K playerPos = client.CurrentPlayer.Position;
            Vector3K playerRot = client.CurrentPlayer.EulerAngles;

            Vector3K pos = new Vector3K(1.3F, -0.55F, 0).RotateVector2(playerRot.z);
            Vector3K rot = new Vector3K(0f, 0f, playerRot.z - 0.067F);

            pos.x += playerPos.x;
            pos.y += playerPos.y;

            BulletObject bullet = new BulletObject(pos, new Vector3K(0f, 0f, 0f), new Vector3K(0.28f, 0.09f, 0f), rot);
            bullet.Mission = client.Mission;

            client.Mission.AddDynamicObject(bullet, client.Mission.Physics.World);

            NetDataRequest allResponse = new NetDataRequest(RequestTypes.Shoot, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(id) } });
            Server.SendResponse(clients, Utils.ToBytesJSON(allResponse), networkID);
        }
    }
}
