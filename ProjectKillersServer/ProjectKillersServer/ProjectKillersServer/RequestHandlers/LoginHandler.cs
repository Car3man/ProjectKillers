using Common;
using ProjectKillersCommon;
using SwiftKernelServerProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectKillersServer.RequestHandlers {
    public static class LoginHandler {
        public static void DoHandle(NetDataRequest data, Client client, string networkID) {
            string nickname = (string)data.Values["nickname"].ObjectValue;

            NetDataRequest response = null;

            if (Server.Clients.Exists(x => x.Nickname.Equals(nickname))) {
                response = new NetDataRequest(RequestTypes.Login, RequestResult.NicknameOccupied, new Dictionary<string, ObjectWrapper>());
            } else {
                client.Nickname = nickname;

                response = new NetDataRequest(RequestTypes.Login, new Dictionary<string, ObjectWrapper>());
            }

            Server.SendResponse(client, Utils.ToBytesJSON(response), networkID);
        }
    }
}
