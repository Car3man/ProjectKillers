using System;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Objects;
using ProjectKillersServer.Controllers;
using ProjectKillersServer.Factories;

namespace ProjectKillersServer {
    public class SpawnManager {
        public BaseMissionController MissionController;

        private float lastTimeSpawn = 0f;
        private float time = 0f;

        public float SpawnInterval = 5f;

        public void Update(float deltaTime) {
            time += deltaTime;

            if (Math.Abs(time - lastTimeSpawn) >= SpawnInterval) {
                Random rnd = new Random();

                float x = rnd.Next(-50, 50);
                float y = rnd.Next(-50, 50);
                float z = rnd.Next(-50, 50);

                SkeletonObject skeleton = new SkeletonObject(new Vector3K(x, y, z), new Vector3K(0f, 0f, 0f), new Vector3K(2f, 2f, 2f), new Vector3K(0f, 0f, 0f));
                MissionController.AddDynamicObject(ObjectFactory.GetObject(skeleton));

                lastTimeSpawn = time;
            }
        }
    }
}
