using Common;
using ProjectKillersCommon;
using SwiftKernelServerProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectKillersServer.Events
{
    public static class LeaveMissionHandler
    {
        public static void DoHandle(Client client, string networkID)
        {
            List<Client> clients = new List<Client>(Server.Clients);
            clients.Remove(client);
            clients.RemoveAll(x => !x.Actualy);

            NetDataEvent allResponse = new NetDataEvent(EventTypes.LeaveMission, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(client.ID) } });
            Server.SendEvent(clients, Utils.ToBytesJSON(allResponse), networkID);
        }
    }
}
