using ProjectKillersCommon.Data.Objects;
using ProjectKillersServer.Controllers.Objects;
using System;

namespace ProjectKillersServer.Factories {
    public static class ObjectControllerFactory {
        public static BaseMissionObjectController GetMission(BaseMissionObject obj) {
            if (obj.GetType() == typeof(BulletObject)) {
                return new BulletObjectController(obj);
            } else if (obj.GetType() == typeof(PlayerObject)) {
                return new PlayerObjectController(obj);
            } else if (obj.GetType() == typeof(SkeletonObject)) {
                return new SkeletonObjectController(obj);
            } else if (obj.GetType() == typeof(TestObject)) {
                return new TestObjectController(obj);
            } else {
                throw new Exception("unknow mission type " + obj.GetType().ToString());
            }
        }
    }
}
