using Box2DX.Dynamics;
using ProjectKillersCommon.Data.Missions;
using ProjectKillersServer.Controllers.Objects;
using System.Collections.Generic;
using System.Linq;

namespace ProjectKillersServer.Controllers {
    public class BaseMissionController {
        public BaseMission Mission;

        public RoomController RoomController;

        public Dictionary<string, BaseMissionObjectController> Objects = new Dictionary<string, BaseMissionObjectController>();
        public Dictionary<string, BaseMissionObjectController> DynamicObjects = new Dictionary<string, BaseMissionObjectController>();

        //PHYSICS
        public Physics Physics { get; private set; }
        public PhysicsSolver PhysicsSolver { get; private set; }
        public CollisionHandler CollisionHandler { get; private set; }

        private object locker = new object();
        public object Locker {
            get {
                return locker;
            }
        }

        public BaseMissionController(BaseMission mission) {
            Mission = mission;

            CollisionHandler = new CollisionHandler();

            PhysicsSolver = new PhysicsSolver();
            PhysicsSolver.OnAdd += CollisionHandler.OnCollide;

            Physics = new Physics(-1100, -1100, 1100, 1100, 0, 0, false);
            Physics.SetSolver(PhysicsSolver);
        }

        public virtual void Update(float deltaTime) {
            lock (locker) {
                foreach (var o in Objects.Where(x => x.Value.Object.Destroyed && !x.Value.NotObserve).ToList()) {
                    o.Value.OnDestroy();
                }
                foreach (var o in DynamicObjects.Where(x => x.Value.Object.Destroyed && !x.Value.NotObserve).ToList()) {
                    o.Value.OnDestroy();
                }
                foreach (BaseMissionObjectController obj in Objects.Values.Where(x => !x.Object.Destroyed)) obj.Update(deltaTime);
                foreach (BaseMissionObjectController obj in DynamicObjects.Values.Where(x => !x.Object.Destroyed)) obj.Update(deltaTime);
            }

            Physics.Update(deltaTime);
        }

        public virtual void AddObject(BaseMissionObjectController obj) {
            lock (locker) {
                obj.SetupPhysics(Physics.World);
                obj.MissionController = this;

                Objects.Add(obj.Object.ID, obj);
                Mission.Objects.Add(obj.Object.ID, obj.Object);
            }
        }

        public virtual void AddDynamicObject(BaseMissionObjectController obj) {
            lock (locker) {
                obj.SetupPhysics(Physics.World);
                obj.MissionController = this;

                DynamicObjects.Add(obj.Object.ID, obj);
                Mission.DynamicObjects.Add(obj.Object.ID, obj.Object);
            }
        }
    }
}
