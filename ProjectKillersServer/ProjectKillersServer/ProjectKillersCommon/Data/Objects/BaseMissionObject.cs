using ProjectKillersCommon.Classes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using ProjectKillersCommon.Data.Missions;
using UnityEngine;
using Box2DX.Dynamics;
using Box2DX.Collision;

namespace ProjectKillersCommon.Data.Objects {
    [Serializable]
    [ProtoContract(SkipConstructor = true, UseProtoMembersOnly = true)]
    [ProtoInclude(101, typeof(BulletObject))]
    [ProtoInclude(102, typeof(TestObject))]
    public abstract class BaseMissionObject {
        [ProtoMember(1)]
        public string ID;

        [ProtoMember(2)]
        public string NameID;
        [ProtoMember(3)]
        public string Name;

        [ProtoMember(4)]
        public bool Destroyed = false;

        [ProtoMember(5)]
        public Vector3K EulerAngles;
        [ProtoMember(6)]
        public Vector3K Position;
        [ProtoMember(7)]
        public Vector3K Center;
        [ProtoMember(8)]
        public Vector3K Size;
        [ProtoMember(9)]
        public bool CanBreaked = true;

        public BaseMission Mission;
        public bool Changed = false;
        public bool NotObserve = false;
        public float LifeTime = 0f;

        //PHYSICS
        public bool IsStatic = false;

        protected BodyDef bodyDef;
        protected CircleDef circleDef;
        protected Body body;

        protected World world;

        public BaseMissionObject(Vector3K position, Vector3K center, Vector3K size, Vector3K eulerAngles) {
            ID = Guid.NewGuid().ToString();

            Position = position;
            Center = center;
            Size = size;
            EulerAngles = eulerAngles;

            Changed = true;
        }

        //must called for work physics 
        public virtual void SetupPhysics(World world) {
            float angle = EulerAngles.z;
            angle = angle * Mathf.Deg2Rad;

            bodyDef = new BodyDef();
            bodyDef.Position.Set(Position.x, Position.y);
            bodyDef.Angle = angle;

            circleDef = new CircleDef();
            circleDef.Restitution = 0.2f;
            circleDef.Friction = 0.3f;
            circleDef.Density = 0.5f;
            circleDef.Radius = Mathf.Max(Size.x / 3F, Size.y / 3F, Size.z / 3F);

            body = world.CreateBody(bodyDef);

            while(body==null) {
                body = world.CreateBody(bodyDef);
            }

            body.CreateShape(circleDef);
            body.SetMassFromShapes();
            body.SetUserData(this);

            this.world = world;
        }

        public void SetPosition(Vector3K position) {
            if(Vector3K.Distance(Position, position) > 0.01F){
                Changed = true;
            }

            Position = position;

            if (Mathf.Abs(Position.x) > 1000 || Mathf.Abs(Position.y) > 1000) {
                Destroy();
                return;
            }

            if (body != null)
                body.SetXForm(new Box2DX.Common.Vec2(position.x, position.y), EulerAngles.z * Mathf.Deg2Rad);
        }

        public void Destroy(){
            Destroyed = true;
            Changed = true;
        }

        public abstract void DoRequest(Dictionary<string, object> request);
        public virtual void Update(float deltaTime) {
            LifeTime += deltaTime;
        }

        public virtual void OnDestroy() {
            world.DestroyBody(body);
            NotObserve = true;
        }
        public virtual void OnCollide(BaseMissionObject other) { }
    }
}
