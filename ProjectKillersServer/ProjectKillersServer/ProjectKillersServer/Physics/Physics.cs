using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Objects;

namespace ProjectKillersServer.Physics {
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
            world.Step(deltaTime, 20, 20);

            for (Body list = world.GetBodyList(); list != null; list = list.GetNext()) {
                if (list.GetUserData() != null) { 
                    float angle = list.GetAngle() * 180.0f / (float)System.Math.PI; 
                    BaseMissionObject model = (BaseMissionObject)list.GetUserData();

                    model.SetPosition(new Vector3K(list.GetPosition().X, list.GetPosition().Y, 0)); 
                    model.EulerAngles = new Vector3K(0f, 0f, angle); 
                }
            }
        }

        public void SetSolver (ContactListener listener) {
            world.SetContactListener(listener);
        }
    }
}
