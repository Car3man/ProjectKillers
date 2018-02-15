using System;
using ProjectKillersCommon.Data.Objects;

namespace ProjectKillersServer.Physics {
    public class CollisionHandler {
        public void OnCollide(BaseMissionObject body1, BaseMissionObject body2) {
            body1.OnCollide(body2);
            body2.OnCollide(body1);
        }
    }
}
