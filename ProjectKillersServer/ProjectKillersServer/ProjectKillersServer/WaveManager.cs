using System.Collections.Generic;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Objects;
using ProjectKillersServer.Controllers;
using ProjectKillersServer.Controllers.Objects;
using ProjectKillersServer.Events;
using ProjectKillersServer.Factories;
using UnityEngine;

namespace ProjectKillersServer {
    public class WaveManager {
        public BaseMissionController MissionController;

        private bool waveStarted = false;
        private float waveWaitDown = 0f;
        private float waveDown = 0f;
        private float spawnDown = 0f;

        private int waveNumber = 0;

        public int WaveNumber {
            get {
                return waveNumber;
            }
            private set {
                waveNumber = value;

                NewWaveHandler.DoHandle(MissionController.RoomController, value);
            }
        }

        private float WaveDuration {
            get {
                return Mathf.Clamp(BASE_WAVE_DURATION + WaveNumber * FACTOR_WAVE_DURATION, BASE_WAVE_DURATION, MAX_WAVE_DURATION);
            }
        }

        private float SpawnInterval {
            get {
                return Mathf.Clamp(BASE_SPAWN_INTERVAL + WaveNumber * FACTOR_SPAWN_INTERVAL, MIN_SPAWN_INTERVAL, BASE_SPAWN_INTERVAL);
            }
        }

        private const float WAVE_WAIT = 7F;

        private const float BASE_WAVE_DURATION = 30F;
        private const float FACTOR_WAVE_DURATION = 1.2F;
        private const float MAX_WAVE_DURATION = 120F;

        private const float BASE_SPAWN_INTERVAL = 2.5f;
        private const float FACTOR_SPAWN_INTERVAL = 0.5F;
        private const float MIN_SPAWN_INTERVAL = 0.02f;

        private List<SkeletonObjectController> spawnedSkeletons = new List<SkeletonObjectController>();

        public void Update(float deltaTime) {
            if (!waveStarted) {
                if (spawnedSkeletons.Exists(x => ((x as IHuman) != null && (x as IHuman).IsLive))) return;

                waveWaitDown += deltaTime;

                if (waveWaitDown >= WAVE_WAIT) {
                    WaveNumber++;
                    waveStarted = true;

                    waveWaitDown = 0f;
                }
            } else {
                waveDown += deltaTime;

                if (waveDown >= WaveDuration) {
                    waveStarted = false;
                    waveDown = 0f;
                } else {
                    spawnDown += deltaTime;

                    if (spawnDown >= SpawnInterval) {
                        System.Random rnd = new System.Random();

                        int r = rnd.Next(0, 3);

                        Vector3K position = new Vector3K(-15f, 15f, 0f);
                        switch (r) {
                            case 1: position = new Vector3K(15f, 15f, 0f); break;
                            case 2: position = new Vector3K(15f, -15f, 0f); break;
                            case 3: position = new Vector3K(-15f, -15f, 0f); break;
                        }

                        SkeletonObject skeleton = new SkeletonObject(position, new Vector3K(0f, 0f, 0f), new Vector3K(2f, 2f, 2f), new Vector3K(0f, 0f, 0f));
                        SkeletonObjectController skeletonController = ObjectFactory.GetObject(skeleton) as SkeletonObjectController;

                        MissionController.AddDynamicObject(skeletonController);
                        spawnedSkeletons.Add(skeletonController);

                        spawnDown = 0f;
                    }
                }
            }
        }
    }
}
