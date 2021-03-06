﻿using Common;
using ProjectKillersCommon;
using ProjectKillersServer.Controllers;
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
        public static void DoHandle(ClientController client)
        {
            List<ClientController> clients = new List<ClientController>(Server.ClientControllers);
            clients.RemoveAll(x => x.Client == client.Client);
            clients.RemoveAll(x => !x.Actualy);

            NetDataEvent allResponse = new NetDataEvent(EventTypes.LeaveMission, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(client.Client.ID) } });
            Server.SendEvent(clients, Utils.ToBytesJSON(allResponse), "EventGameManagerHandleLeaveMission");
        }
    }
}
