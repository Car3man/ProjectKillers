using ProjectKillersCommon.Classes;
using ProtoBuf;
using System;

namespace ProjectKillersCommon.Data.Objects {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class BulletObject : BaseMissionObject {
        public float MoveSpeed = 55F;

        public BulletObject(Vector3K position, Vector3K center, Vector3K size, Vector3K eulerAngles) : base(position, center, size, eulerAngles) {
            ID = Guid.NewGuid().ToString();

            Position = position;
            Center = center;
            Size = size;
            EulerAngles = eulerAngles;

            CanBreaked = false;

            Name = "Bullet Object";
            NameID = "BulletObject";
        }
    }
}
