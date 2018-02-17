using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Missions;
using SwiftKernelServerProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectKillersServer.RequestHandlers {
    public static class SyncMissionHandler {
        public static void DoHandle(NetData data, Client client, string networkID) {
            List<Client> clients = new List<Client>(EntryPoint.Clients);
            clients.RemoveAll(x => !x.Actualy);

            string id = (string)data.Values["id"].ObjectValue;

            BaseMission mission = EntryPoint.Mission;
            if(client.MissionFirstInited) {
                mission = mission.GetMissionChanges();
            }

            client.MissionFirstInited = true;

            NetData allResponse = new NetData(RequestTypes.SyncMission, new Dictionary<string, ObjectWrapper>() { { "mission", new ObjectWrapper<BaseMission>(mission) } });
            byte[] sendData = Utils.ToBytesJSON(allResponse);

            //Console.WriteLine(sendData.Length);

            EntryPoint.SendResponse(clients, sendData, networkID);
        }
    }
}
