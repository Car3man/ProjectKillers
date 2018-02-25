using System;
using ProjectKillersCommon.Classes;
using ProtoBuf;

namespace ProjectKillersCommon.Data.Objects {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class PlayerObject : BaseMissionObject, IHuman {
        private int health = 100;
        private int maxHealth = 100;

        public float MoveSpeed = 5f;

        [ProtoMember(1)]
        public int Health {
            get {
                return health;
            }
            set {
                health = value;
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

        public PlayerObject (Vector3K position, Vector3K center, Vector3K size, Vector3K eulerAngles) : base(position, center, size, eulerAngles) 
        {
            ID = Guid.NewGuid().ToString();

            Position = position;
            Center = center;
            Size = size;
            EulerAngles = eulerAngles;

            CanBreaked = false;

            Name = "Player";
            NameID = "Player";
        }
    }
}
