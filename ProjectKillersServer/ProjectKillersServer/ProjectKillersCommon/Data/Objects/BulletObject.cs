using ProjectKillersCommon.Classes;
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
        public float MoveSpeed = 35F;

        public BulletObject(Vector3K position, Vector3K center, Vector3K size, Vector3K eulerAngles) : base(position, center, size, eulerAngles) {
            ID = Guid.NewGuid().ToString();

            Position = position;
            Center = center;
            Size = size;
            EulerAngles = eulerAngles;

            CanBreaked = false;

            Name = "Bullet Object";
            NameID = "BulletObject";
        }

        public override void DoRequest(Dictionary<string, object> request) {

        }

        public override void Update(float deltaTime) {
            float angle = EulerAngles.z;
            angle = angle * Mathf.Deg2Rad;
            Position = new Vector3K((Position.x + Mathf.Cos(angle) * deltaTime * MoveSpeed), (Position.y + Mathf.Sin(angle) * deltaTime * MoveSpeed), 0F);

            double dist = 0;
            foreach(string objID in Mission.Objects.Keys) {
                if (Mission.Objects[objID] == this || !Mission.Objects[objID].CanBreaked) continue;

                dist = Math.Sqrt(Math.Pow(Mission.Objects[objID].Position.x - Position.x, 2) + Math.Pow(Mission.Objects[objID].Position.y - Position.y, 2));
                if(dist < Mission.Objects[objID].Size.x / 2F) {
                    Mission.Objects[objID].Destroyed = true;
                }
            }
            foreach (string objID in Mission.DynamicObjects.Keys) {
                if (Mission.DynamicObjects[objID] == this || !Mission.DynamicObjects[objID].CanBreaked) continue;

                dist = Math.Sqrt(Math.Pow(Mission.DynamicObjects[objID].Position.x - Position.x, 2) + Math.Pow(Mission.DynamicObjects[objID].Position.y - Position.y, 2));
                if (dist < Mission.Objects[objID].Size.x / 2F) {
                    Mission.DynamicObjects[objID].Destroyed = true;
                }
            }
        }
    }
}
