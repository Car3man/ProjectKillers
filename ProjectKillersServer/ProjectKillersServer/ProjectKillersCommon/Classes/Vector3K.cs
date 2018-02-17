using ProtoBuf;
using System;
using UnityEngine;

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

        public static float Distance (Vector3K vec1, Vector3K vec2) {
            return Mathf.Sqrt(Mathf.Pow((vec2.x - vec1.x), 2) + Mathf.Pow((vec2.y - vec1.y), 2));
        }
    }
}
