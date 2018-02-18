﻿using ProjectKillersCommon.Data.Objects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using Box2DX.Dynamics;
using System.Reflection;
using ProjectKillersServer.Physics;

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

        //PHYSICS
        public Physics Physics { get; private set; }
        public PhysicsSolver PhysicsSolver { get; private set; }
        public CollisionHandler CollisionHandler { get; private set; }

        private object locker = new object();

        public BaseMission(bool createPhysic) {
            if(createPhysic) {
                CollisionHandler = new CollisionHandler();

                PhysicsSolver = new PhysicsSolver();
                PhysicsSolver.OnAdd += CollisionHandler.OnCollide;

                Physics = new Physics(-1100, -1100, 1100, 1100, 0, 0, false);
                Physics.SetSolver(PhysicsSolver);
            }
        }

        public object Locker {
            get {
                return locker;
            }
        }

        public virtual void AddObject (BaseMissionObject obj, World world) {
            lock (locker) {
                obj.SetupPhysics(world);
                obj.Mission = this;

                Objects.Add(obj.ID, obj);
            }
        }

        public virtual void AddDynamicObject(BaseMissionObject obj, World world) {
            lock(locker) {
                obj.SetupPhysics(world);
                obj.Mission = this;

                DynamicObjects.Add(obj.ID, obj);
            }
        }

        public virtual void Update(float deltaTime) {
            lock(locker) {
                foreach (var o in Objects.Where(x => x.Value.Destroyed && x.Value.Changed && !x.Value.NotObserve).ToList()) {
                    o.Value.OnDestroy();
                }
                foreach (var o in DynamicObjects.Where(x => x.Value.Destroyed && x.Value.Changed && !x.Value.NotObserve).ToList()) {
                    o.Value.OnDestroy();
                }
                foreach (BaseMissionObject obj in Objects.Values.Where(x => !x.Destroyed)) obj.Update(deltaTime);
                foreach (BaseMissionObject obj in DynamicObjects.Values.Where(x => !x.Destroyed)) obj.Update(deltaTime);
            }

            Physics.Update(deltaTime);
        }

        public virtual BaseMission GetMissionChanges () {
            BaseMission mission = new BaseMission(false);
            mission.Name = Name;
            mission.Objects = new Dictionary<string, BaseMissionObject>();
            mission.DynamicObjects = new Dictionary<string, BaseMissionObject>();
            mission.Physics = Physics;
            mission.PhysicsSolver = PhysicsSolver;
            mission.CollisionHandler = CollisionHandler;

            lock (locker) {
                foreach (string k in Objects.Keys) {
                    if (Objects[k].Changed) {
                        mission.Objects.Add(k, Objects[k]);
                        Objects[k].Changed = false;
                    }
                }
                foreach (string k in DynamicObjects.Keys) {
                    if (DynamicObjects[k].Changed) {
                        mission.DynamicObjects.Add(k, DynamicObjects[k]);
                        DynamicObjects[k].Changed = false;
                    }
                }
            }

            return mission;
        }

        public static List<Type> GetMissionTypes() {
            return Assembly.GetAssembly(typeof(BaseMission)).GetTypes().Where(t => t.IsSubclassOf(typeof(BaseMission))).ToList();
        }
    }
}
