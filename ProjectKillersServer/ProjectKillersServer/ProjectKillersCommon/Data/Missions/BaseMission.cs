using ProjectKillersCommon.Data.Objects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProjectKillersCommon.Data.Missions {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    [ProtoInclude(100, typeof(TestMission))]
    public class BaseMission {
        [ProtoMember(1)]
        public string Name;
        [ProtoMember(2)]
        public Dictionary<string, BaseMissionObject> Objects = new Dictionary<string, BaseMissionObject>();
        [ProtoMember(3)]
        public Dictionary<string, BaseMissionObject> DynamicObjects = new Dictionary<string, BaseMissionObject>();

        public static List<Type> GetMissionTypes() {
            return Assembly.GetAssembly(typeof(BaseMission)).GetTypes().Where(t => t.IsSubclassOf(typeof(BaseMission))).ToList();
        }
    }
}
