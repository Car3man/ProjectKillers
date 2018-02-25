using Box2DX.Collision;
using Box2DX.Dynamics;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Missions;
using ProjectKillersCommon.Data.Objects;
using UnityEngine;

namespace ProjectKillersServer.Controllers.Objects {
    public class BaseMissionObjectController {
        public BaseMissionController MissionController;

        public BaseMissionObject Object;

        public bool NotObserve;
        public float LifeTime = 0f;

        protected BodyDef bodyDef;
        protected CircleDef circleDef;
        protected Body body;

        protected World world;

        public BaseMissionObjectController(BaseMissionObject obj) {
            Object = obj;
        }

        public virtual void SetupPhysics(World world) {
            float angle = Object.EulerAngles.z;
            angle = angle * Mathf.Deg2Rad;

            bodyDef = new BodyDef();
            bodyDef.Position.Set(Object.Position.x, Object.Position.y);
            bodyDef.Angle = angle;

            circleDef = new CircleDef();
            circleDef.Restitution = 0.2f;
            circleDef.Friction = 0.3f;
            circleDef.Density = 0.5f;
            circleDef.Radius = Mathf.Max(Object.Size.x / 3f, Object.Size.y / 3f, Object.Size.z / 3f);

            body = world.CreateBody(bodyDef);

            while (body == null) {
                body = world.CreateBody(bodyDef);
            }

            body.CreateShape(circleDef);
            body.SetMassFromShapes();
            body.SetUserData(this);

            this.world = world;
        }

        public void SetTransform(Vector3K position, Vector3K eulerAngles) {
            Object.Position = position;
            Object.EulerAngles = eulerAngles;

            if (Mathf.Abs(Object.Position.x) > 1000 || Mathf.Abs(Object.Position.y) > 1000) {
                Destroy();
                return;
            }

            if (body != null) {
                body.SetXForm(new Box2DX.Common.Vec2(position.x, position.y), Object.EulerAngles.z * Mathf.Deg2Rad);
            }
        }

        public void Destroy() {
            Object.Destroyed = true;
            world.DestroyBody(body);
        }

        public virtual void Update(float deltaTime) {
            LifeTime += deltaTime;
        }

        public virtual void OnDestroy() {
            NotObserve = true;
        }
        public virtual void OnCollide(BaseMissionObjectController other) { }
    }
}
