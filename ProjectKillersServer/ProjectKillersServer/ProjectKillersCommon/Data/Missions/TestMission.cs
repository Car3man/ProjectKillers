using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Objects;
using ProtoBuf;
using System;

namespace ProjectKillersCommon.Data.Missions {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class TestMission : BaseMission {
        public TestMission(bool createPhysic) : base(createPhysic) {
            Name = "Test Mission";

            TestObject testObj1 = new TestObject(new Vector3K(2f, 5f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.75f, 0.75f, 0.75f), new Vector3K(0f, 0f, 0f));
            TestObject testObj2 = new TestObject(new Vector3K(2f, 4f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.75f, 0.75f, 0.75f), new Vector3K(0f, 0f, 0f));
            TestObject testObj3 = new TestObject(new Vector3K(2f, 3f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.75f, 0.75f, 0.75f), new Vector3K(0f, 0f, 0f));
            TestObject testObj4 = new TestObject(new Vector3K(2f, 2f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.75f, 0.75f, 0.75f), new Vector3K(0f, 0f, 0f));
            TestObject testObj5 = new TestObject(new Vector3K(2f, 1f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.75f, 0.75f, 0.75f), new Vector3K(0f, 0f, 0f));
            TestObject testObj6 = new TestObject(new Vector3K(2f, 0f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.75f, 0.75f, 0.75f), new Vector3K(0f, 0f, 0f));
            TestObject testObj7 = new TestObject(new Vector3K(2f, -1f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.75f, 0.75f, 0.75f), new Vector3K(0f, 0f, 0f));
            TestObject testObj8 = new TestObject(new Vector3K(2f, -2f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.75f, 0.75f, 0.75f), new Vector3K(0f, 0f, 0f));
            TestObject testObj9 = new TestObject(new Vector3K(2f, -3f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.75f, 0.75f, 0.75f), new Vector3K(0f, 0f, 0f));
            TestObject testObj10 = new TestObject(new Vector3K(2f, -4f, 0f), new Vector3K(0f, 0f, 0f), new Vector3K(0.75f, 0.75f, 0.75f), new Vector3K(0f, 0f, 0f));

            AddObject(testObj1, Physics.World);
            AddObject(testObj2, Physics.World);
            AddObject(testObj3, Physics.World);
            AddObject(testObj4, Physics.World);
            AddObject(testObj5, Physics.World);
            AddObject(testObj6, Physics.World);
            AddObject(testObj7, Physics.World);
            AddObject(testObj8, Physics.World);
            AddObject(testObj9, Physics.World);
            AddObject(testObj10, Physics.World);
        }
    }
}
