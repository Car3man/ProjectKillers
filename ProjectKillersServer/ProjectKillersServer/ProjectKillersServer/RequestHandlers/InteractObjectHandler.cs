using ProjectKillersCommon;
using ProjectKillersCommon.Data.Objects;
using SwiftKernelServerProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectKillersServer.RequestHandlers {
    public static class InteractObjectHandler {
        public static void DoHandle(NetData data, Client client, string networkID) {
            List<Client> clients = new List<Client>(Server.Clients);
            clients.RemoveAll(x => !x.Actualy);

            string clientId = (string)data.Values["id"].ObjectValue;
            string objectId = (string)data.Values["objectId"].ObjectValue;
            Dictionary<string, object> request = (Dictionary<string, object>)data.Values["request"].ObjectValue;

            if (Server.Mission.Objects.ContainsKey(objectId)) {
                Server.Mission.Objects[objectId].DoRequest(request);
            }
        }
    }
}
