using ProjectKillersCommon.Classes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectKillersCommon.Data.Objects {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class SkeletonObject : BaseMissionObject, IHuman {
        private int health = 100;
        private int maxHealth = 100;

        public float MoveSpeed = 5f;
        public float HitTime = 1f;

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

        private float hitTimeDown;

        public SkeletonObject(Vector3K position, Vector3K center, Vector3K size, Vector3K eulerAngles) : base(position, center, size, eulerAngles) {
            ID = Guid.NewGuid().ToString();

            Position = position;
            Center = center;
            Size = size;
            EulerAngles = eulerAngles;

            CanBreaked = false;
            Changed = true;

            Name = "Skeleton";
            NameID = "Skeleton";

            hitTimeDown = HitTime;
        }

        public override void DoRequest(Dictionary<string, object> request) {

        }

        public override void Update(float deltaTime) {
            CheckHealth();

            PlayerObject targetPlayer = Mission.DynamicObjects.Values.FirstOrDefault(x => x.GetType() == typeof(PlayerObject)) as PlayerObject;

            if (targetPlayer == null) return;

            if(Vector3K.Distance(targetPlayer.Position, Position) > 2f) {
                hitTimeDown = HitTime;

                EulerAngles.z = (float)(Mathf.Rad2Deg * (Math.Atan2(targetPlayer.Position.y - Position.y, targetPlayer.Position.x - Position.x)));

                Position.x += Mathf.Cos(EulerAngles.z * Mathf.Deg2Rad) * MoveSpeed * deltaTime;
                Position.y += Mathf.Sin(EulerAngles.z * Mathf.Deg2Rad) * MoveSpeed * deltaTime;

                body.SetXForm(new Box2DX.Common.Vec2(Position.x, Position.y), EulerAngles.z * Mathf.Deg2Rad);
            } else {
                hitTimeDown -= deltaTime;
            }

            CheckHit(targetPlayer);
        }

        private void CheckHealth() {
            if(Health <= 0) {
                Destroy();
            }
        }

        private void CheckHit(PlayerObject player) {
            if(hitTimeDown <= 0) {
                player.Health -= 10;
                hitTimeDown = HitTime;

                Console.WriteLine("Player health: {0}", player.Health);
            }
        }
    }
}
