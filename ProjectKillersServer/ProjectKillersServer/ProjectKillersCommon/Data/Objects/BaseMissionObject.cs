using ProjectKillersCommon.Classes;
using ProtoBuf;
using System;

namespace ProjectKillersCommon.Data.Objects {
    [Serializable]
    [ProtoContract(SkipConstructor = true, UseProtoMembersOnly = true)]
    [ProtoInclude(101, typeof(BulletObject))]
    [ProtoInclude(102, typeof(TestObject))]
    [ProtoInclude(103, typeof(PlayerObject))]
    [ProtoInclude(106, typeof(SkeletonObject))]
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
        [ProtoMember(10)]
        public string OwnerID = "";

        //PHYSICS
        public bool IsStatic = false;

        public BaseMissionObject(Vector3K position, Vector3K center, Vector3K size, Vector3K eulerAngles) {
            ID = Guid.NewGuid().ToString();

            Position = position;
            Center = center;
            Size = size;
            EulerAngles = eulerAngles;
        }
    }
}
