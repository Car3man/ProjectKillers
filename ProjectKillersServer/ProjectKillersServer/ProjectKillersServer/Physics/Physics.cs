using System.Threading;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using ProjectKillersCommon.Classes;
using ProjectKillersServer.Controllers.Objects;

namespace ProjectKillersServer {
    public class Physics {
        private World world;

        public World World {
            get{
                return world;
            }
        }

        public Physics (float x, float y, float w, float h, float g_x, float g_y, bool doSleep) {
            AABB aabb = new AABB();
            aabb.LowerBound.Set(x, y); 
            aabb.UpperBound.Set(w, h); 
            Vec2 g = new Vec2(g_x, g_y); 
            world = new World(aabb, g, doSleep);
        }

        public void Update (float deltaTime) {
            world.Step(deltaTime, 5, 5);

            for (Body list = world.GetBodyList(); list != null; list = list.GetNext()) {
                if (list.GetUserData() != null) { 
                    float angle = list.GetAngle() * 180.0f / (float)System.Math.PI; 
                    BaseMissionObjectController model = (BaseMissionObjectController)list.GetUserData();
                    SetTransformModel(model, new Vector3K(list.GetPosition().X, list.GetPosition().Y, 0), new Vector3K(0f, 0f, angle));
                }
            }
        }

        public void SetTransformModel (BaseMissionObjectController model, Vector3K position, Vector3K eulerAngles) {
            model.Object.Position = position;
            model.Object.EulerAngles = eulerAngles;

            if (Math.Abs(model.Object.Position.x) > 1000 || Math.Abs(model.Object.Position.y) > 1000) {
                model.Destroy();
                return;
            }
        }

        public void SetSolver (ContactListener listener) {
            world.SetContactListener(listener);
        }
    }
}
