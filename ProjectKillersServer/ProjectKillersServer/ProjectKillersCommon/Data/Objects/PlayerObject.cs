using System;
using System.Collections.Generic;
using ProjectKillersCommon.Classes;
using ProtoBuf;

namespace ProjectKillersCommon.Data.Objects {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class PlayerObject : BaseMissionObject {
        public float MoveSpeed = 5f;

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
    }
}
