using Common;
using ProjectKillersCommon;
using ProjectKillersServer.Controllers;
using SwiftKernelServerProject;
using System.Collections.Generic;

namespace ProjectKillersServer.RequestHandlers {
    public static class LoginHandler {
        public static void DoHandle(NetDataRequest data, ClientController client, string networkID) {
            string nickname = (string)data.Values["nickname"].ObjectValue;

            NetDataRequest response = null;

            if (Server.ClientControllers.Exists(x => x.Client.Nickname.Equals(nickname))) {
                response = new NetDataRequest(RequestTypes.Login, RequestResult.NicknameOccupied, new Dictionary<string, ObjectWrapper>());
            } else {
                client.Client.Nickname = nickname;

                response = new NetDataRequest(RequestTypes.Login, new Dictionary<string, ObjectWrapper>());
            }

            Server.SendResponse(client, Utils.ToBytesJSON(response), networkID);
        }
    }
}
