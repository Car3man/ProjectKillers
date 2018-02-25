using System;
using System.Linq;
using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Objects;
using SwiftKernelServerProject;
using UnityEngine;

namespace ProjectKillersServer.Controllers.Objects {
    public class SkeletonObjectController : BaseMissionObjectController {
        private float hitTimeDown;
        public float HitTime = 1.5f;

        public SkeletonObjectController(BaseMissionObject obj) : base(obj) {

        }

        public override void Update(float deltaTime) {
            CheckHealth();

            PlayerObjectController targetPlayer = MissionController.DynamicObjects.Values.FirstOrDefault(x => x.GetType() == typeof(PlayerObjectController)) as PlayerObjectController;

            if (targetPlayer == null) return;

            if (Vector3K.Distance(targetPlayer.Object.Position, Object.Position) > 2f) {
                hitTimeDown = HitTime;

                Object.EulerAngles.z = (float)(Mathf.Rad2Deg * (Math.Atan2(targetPlayer.Object.Position.y - Object.Position.y, targetPlayer.Object.Position.x - Object.Position.x)));

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

        private void CheckHit(PlayerObjectController player) {
            if (hitTimeDown <= 0) {
                (player.Object as PlayerObject).Health -= 10;
                hitTimeDown = HitTime;

                Server.SendEvent(MissionController.RoomController.Clients, Utils.ToBytesJSON(new NetDataEvent(EventTypes.Unknow, new System.Collections.Generic.Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(this.Object.ID) } })), "EventSkeletonDoHit");

                Console.WriteLine("Player health: {0}", (player.Object as PlayerObject).Health);
            }
        }
    }
}
