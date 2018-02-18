using ProjectKillersCommon.Data.Missions;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ProjectKillersCommon.Data {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class Room {
        [ProtoMember(1)]
        public string ID;
        [ProtoMember(2)]
        public string Name;
        [ProtoMember(3)]
        public string OwnerID;
        [ProtoMember(4)]
        public List<Client> Clients = new List<Client>();
        [ProtoMember(5)]
        public string MissionName;

        public BaseMission Mission;

        public Room(string name, string ownerID) {
            ID = Guid.NewGuid().ToString();

            Name = name;
            OwnerID = ownerID;

            //Mission list
            List<Type> missions = BaseMission.GetMissionTypes();
            if (missions.Count > 0)
                MissionName = missions[0].Name;
        }
    }
}
