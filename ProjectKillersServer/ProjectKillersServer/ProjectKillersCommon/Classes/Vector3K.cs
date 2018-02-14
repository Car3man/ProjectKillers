using ProtoBuf;
using System;

namespace ProjectKillersCommon.Classes {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class Vector3K {
        [ProtoMember(1)]
        public float x;
        [ProtoMember(2)]
        public float y;
        [ProtoMember(3)]
        public float z;

        public Vector3K(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
