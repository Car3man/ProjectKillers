using ProtoBuf;
using System;

namespace ProjectKillersCommon.Data.Missions {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class TestMission : BaseMission {
        public TestMission(bool createPhysic) : base(createPhysic) {
            Name = "Test Mission";
        }
    }
}
