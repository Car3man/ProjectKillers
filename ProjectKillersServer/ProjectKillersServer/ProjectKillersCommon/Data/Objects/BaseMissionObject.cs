using ProjectKillersCommon.Classes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using ProjectKillersCommon.Data.Missions;

namespace ProjectKillersCommon.Data.Objects {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    [ProtoInclude(101, typeof(BulletObject))]
    [ProtoInclude(102, typeof(TestObject))]
    public abstract class BaseMissionObject {
        [ProtoMember(1)]
        public string ID;

        [ProtoMember(2)]
        public string NameID;
        [ProtoMember(3)]
        public string Name;

        [ProtoMember(4)]
        public bool Destroyed = false;

        [ProtoMember(5)]
        public Vector3K EulerAngles;
        [ProtoMember(6)]
        public Vector3K Position;
        [ProtoMember(7)]
        public Vector3K Center;
        [ProtoMember(8)]
        public Vector3K Size;
        [ProtoMember(9)]
        public bool CanBreaked = true;

        public BaseMission Mission;

        public BaseMissionObject(Vector3K position, Vector3K center, Vector3K size, Vector3K eulerAngles) {
            ID = Guid.NewGuid().ToString();

            Position = position;
            Center = center;
            Size = size;
            EulerAngles = eulerAngles;
        }

        public abstract void DoRequest(Dictionary<string, object> request);
        public abstract void Update(float deltaTime);
    }
}
