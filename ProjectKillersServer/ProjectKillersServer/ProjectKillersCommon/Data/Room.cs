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
        public string Name;
        [ProtoMember(2)]
        public Client Owner;
        [ProtoMember(3)]
        public List<Client> Clients = new List<Client>();
        [ProtoMember(4)]
        public string MissionName;

        public Room(string name, Client owner) {
            Name = name;
            Owner = owner;

            //Mission list
            List<Type> missions = Assembly.GetAssembly(typeof(BaseMission)).GetTypes().Where(t => t.IsSubclassOf(typeof(BaseMission))).ToList();
            if (missions.Count > 0)
                MissionName = missions[0].Name;
        }
    }
}
