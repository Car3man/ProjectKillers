using System;
using System.Linq;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Objects;
using UnityEngine;

namespace ProjectKillersServer.Controllers.Objects {
    public class SkeletonObjectController : BaseMissionObjectController {
        private float hitTimeDown;
        public float HitTime = 1f;

        public SkeletonObjectController(BaseMissionObject obj) : base(obj) {

        }

        public override void Update(float deltaTime) {
            CheckHealth();

            PlayerObject targetPlayer = MissionController.Mission.DynamicObjects.Values.FirstOrDefault(x => x.GetType() == typeof(PlayerObject)) as PlayerObject;

            if (targetPlayer == null) return;

            if (Vector3K.Distance(targetPlayer.Position, Object.Position) > 2f) {
                hitTimeDown = HitTime;

                Object.EulerAngles.z = (float)(Mathf.Rad2Deg * (Math.Atan2(targetPlayer.Position.y - Object.Position.y, targetPlayer.Position.x - Object.Position.x)));

                Object.Position.x += Mathf.Cos(Object.EulerAngles.z * Mathf.Deg2Rad) * (Object as SkeletonObject).MoveSpeed * deltaTime;
                Object.Position.y += Mathf.Sin(Object.EulerAngles.z * Mathf.Deg2Rad) * (Object as SkeletonObject).MoveSpeed * deltaTime;

                body.SetXForm(new Box2DX.Common.Vec2(Object.Position.x, Object.Position.y), Object.EulerAngles.z * Mathf.Deg2Rad);
            } else {
                hitTimeDown -= deltaTime;
            }

            CheckHit(targetPlayer);
        }

        private void CheckHealth() {
            if ((Object as IHuman).Health <= 0) {
                Destroy();
            }
        }

        private void CheckHit(PlayerObject player) {
            if (hitTimeDown <= 0) {
                player.Health -= 10;
                hitTimeDown = HitTime;

                Console.WriteLine("Player health: {0}", player.Health);
            }
        }
    }
}
