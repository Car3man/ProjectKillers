using ProjectKillersCommon.Classes;
using UnityEngine;

namespace ProjectKillersCommon.Extensions {
    public static class VectorExtensions {
        public static Vector3K ToServerVector3(this Vector3 vector) {
            return new Vector3K(vector.x, vector.y, vector.z);
        }
        public static Vector3 FromServerVector3(this Vector3K vector) {
            return new Vector3(vector.x, vector.y, vector.z);
        }
    }
}
