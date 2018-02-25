using ProtoBuf;
using System;

namespace ProjectKillersCommon.Data.Missions {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class TestMission : BaseMission {
        public TestMission() {
            Name = "Test Mission";
        }
    }
}
