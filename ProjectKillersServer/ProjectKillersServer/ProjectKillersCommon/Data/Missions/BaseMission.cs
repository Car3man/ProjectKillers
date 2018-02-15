using ProjectKillersCommon.Data.Objects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using Box2DX.Dynamics;

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

        private object objLock = new object();
        private object dynLock = new object();

        public virtual void AddObject (BaseMissionObject obj, World world) {
            lock (objLock) {
                obj.SetupPhysics(world);
                obj.Mission = this;

                Objects.Add(obj.ID, obj);
            }
        }

        public virtual void AddDynamicObject(BaseMissionObject obj, World world) {
            lock(dynLock) {
                obj.SetupPhysics(world);
                obj.Mission = this;

                DynamicObjects.Add(obj.ID, obj);
            }
        }

        public virtual void Update(float deltaTime) {
            lock (dynLock) {
                foreach (var o in Objects.Where(x => x.Value.Destroyed).ToList()) {
                    Objects.Remove(o.Key);
                    o.Value.OnDestroy();
                }

                foreach (var o in DynamicObjects.Where(x => x.Value.Destroyed).ToList()) {
                    DynamicObjects.Remove(o.Key);
                    o.Value.OnDestroy();
                }

                foreach (BaseMissionObject obj in Objects.Values.ToList()) obj.Update(deltaTime);
                foreach (BaseMissionObject obj in DynamicObjects.Values.ToList()) obj.Update(deltaTime);
            }
        }
    }
}
