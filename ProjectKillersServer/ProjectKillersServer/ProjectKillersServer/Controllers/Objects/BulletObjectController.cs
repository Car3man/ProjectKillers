using System;
using Box2DX.Dynamics;
using ProjectKillersCommon.Data.Objects;
using UnityEngine;

namespace ProjectKillersServer.Controllers.Objects {
    public class BulletObjectController : BaseMissionObjectController {
        public BulletObjectController(BaseMissionObject obj) : base(obj) {

        }

        public override void SetupPhysics(World world) {
            base.SetupPhysics(world);

            body.SetBullet(true);
            body.SetLinearVelocity(new Box2DX.Common.Vec2(Mathf.Cos(Object.EulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(Object.EulerAngles.z * Mathf.Deg2Rad)) * (Object as BulletObject).MoveSpeed);
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);

            if (LifeTime >= 20F) {
                Destroy();
            }
        }

        public override void OnCollide(BaseMissionObjectController other) {
            Console.WriteLine("Bullet collided with " + other.Object.Name);

            if (other.Object.CanBreaked) other.Destroy();
            if (other.Object is IHuman) {
                (other.Object as IHuman).Health -= 35;
                Console.WriteLine("Health - {0}", (other.Object as IHuman).Health);
            }

            Destroy();
        }
    }
}
