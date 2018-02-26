using ProjectKillersCommon.Classes;
using ProtoBuf;
using System;
using UnityEngine;

namespace ProjectKillersCommon.Data.Objects {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class SkeletonObject : BaseMissionObject, IHuman {
        private int health = 100;
        private int maxHealth = 100;

        public float MoveSpeed = 5f;

        [ProtoMember(1)]
        public int Health {
            get {
                return health;
            }
            set {
                health = Mathf.Clamp(value, 0, int.MaxValue);
            }
        }

        [ProtoMember(2)]
        public int MaxHealth {
            get {
                return maxHealth;
            }
            set {
                maxHealth = value;
            }
        }

        public bool IsLive {
            get {
                return health > 0;
            }
        }

        public SkeletonObject(Vector3K position, Vector3K center, Vector3K size, Vector3K eulerAngles) : base(position, center, size, eulerAngles) {
            ID = Guid.NewGuid().ToString();

            Position = position;
            Center = center;
            Size = size;
            EulerAngles = eulerAngles;

            CanBreaked = false;

            Name = "Skeleton";
            NameID = "Skeleton";
        }
    }
}
