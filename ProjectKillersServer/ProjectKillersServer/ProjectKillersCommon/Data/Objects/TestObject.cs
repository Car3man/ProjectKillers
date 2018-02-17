using ProjectKillersCommon.Classes;
using ProtoBuf;
using System;
using System.Collections.Generic;

namespace ProjectKillersCommon.Data.Objects {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class TestObject : BaseMissionObject {
        public TestObject(Vector3K position, Vector3K center, Vector3K size, Vector3K eulerAngles) : base(position, center, size, eulerAngles) {
            ID = Guid.NewGuid().ToString();

            Position = position;
            Center = center;
            Size = size;
            EulerAngles = eulerAngles;

            Name = "Test Object";
            NameID = "TestObject";

            IsStatic = true;
        }

        public override void DoRequest(Dictionary<string, object> request) {
            
        }
    }
}
