using System;
using System.Collections.Generic;
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
            Changed = true;

            Name = "Player";
            NameID = "Player";
        }

        public override void DoRequest (Dictionary<string, object> request) {
            
        }

        public override void Update(float deltaTime) {
            CheckHealth();
        }

        private void CheckHealth() {
            if (Health <= 0) {
                Destroy();
            }
        }
    }
}
