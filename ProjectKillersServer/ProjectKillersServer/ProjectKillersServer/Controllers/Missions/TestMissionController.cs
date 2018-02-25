using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Missions;
using ProjectKillersCommon.Data.Objects;
using ProjectKillersServer.Factories;

namespace ProjectKillersServer.Controllers {
    public class TestMissionController : BaseMissionController {
        private SpawnManager spawnManager;

        public TestMissionController(BaseMission mission) : base(mission) {
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

            AddObject(ObjectFactory.GetObject(testObj1));
            AddObject(ObjectFactory.GetObject(testObj2));
            AddObject(ObjectFactory.GetObject(testObj3));
            AddObject(ObjectFactory.GetObject(testObj4));
            AddObject(ObjectFactory.GetObject(testObj5));
            AddObject(ObjectFactory.GetObject(testObj6));
            AddObject(ObjectFactory.GetObject(testObj7));
            AddObject(ObjectFactory.GetObject(testObj8));
            AddObject(ObjectFactory.GetObject(testObj9));
            AddObject(ObjectFactory.GetObject(testObj10));

            spawnManager = new SpawnManager();
            spawnManager.MissionController = this;
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);

            spawnManager.Update(deltaTime);
        }
    }
}
