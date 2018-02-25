using System;
using ProjectKillersCommon.Data.Objects;
using ProjectKillersServer.Controllers.Objects;

namespace ProjectKillersServer {
    public class CollisionHandler {
        public void OnCollide(BaseMissionObjectController body1, BaseMissionObjectController body2) {
            if (body1 == null || body2 == null) return;

            body1.OnCollide(body2);
            body2.OnCollide(body1);
        }
    }
}
