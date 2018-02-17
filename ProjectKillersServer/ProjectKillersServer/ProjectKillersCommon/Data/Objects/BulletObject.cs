﻿using ProjectKillersCommon.Classes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ProjectKillersCommon.Data.Objects {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class BulletObject : BaseMissionObject {
        public float MoveSpeed = 70f;

        public BulletObject(Vector3K position, Vector3K center, Vector3K size, Vector3K eulerAngles) : base(position, center, size, eulerAngles) {
            ID = Guid.NewGuid().ToString();

            Position = position;
            Center = center;
            Size = size;
            EulerAngles = eulerAngles;

            CanBreaked = false;
            Changed = true;

            Name = "Bullet Object";
            NameID = "BulletObject";
        }

        public override void DoRequest(Dictionary<string, object> request) {

        }

        public override void Update (float deltaTime) {
            base.Update(deltaTime);

            if(LifeTime >= 20F) {
                Destroy();
            }
        }

        public override void SetupPhysics (Box2DX.Dynamics.World world) {
            base.SetupPhysics(world);

            body.SetBullet(true);
            body.SetLinearVelocity(new Box2DX.Common.Vec2(Mathf.Cos(EulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(EulerAngles.z * Mathf.Deg2Rad)) * MoveSpeed);
        }

        public override void OnCollide (BaseMissionObject other) {
            Console.WriteLine("Bullet collided with " + other.Name);

            if (other.CanBreaked) other.Destroy();
            Destroy();
        }
    }
}
