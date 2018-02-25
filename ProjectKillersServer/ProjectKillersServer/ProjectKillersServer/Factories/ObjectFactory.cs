using ProjectKillersCommon.Data.Objects;
using ProjectKillersServer.Controllers.Objects;
using System;
using ProjectKillersCommon.Classes;

namespace ProjectKillersServer.Factories {
    public static class ObjectFactory {
        public static BaseMissionObjectController GetObject(BaseMissionObject obj) {
            if (obj.GetType() == typeof(BulletObject)) {
                return new BulletObjectController(obj);
            } else if (obj.GetType() == typeof(PlayerObject)) {
                return new PlayerObjectController(obj);
            } else if (obj.GetType() == typeof(SkeletonObject)) {
                return new SkeletonObjectController(obj);
            } else if (obj.GetType() == typeof(TestObject)) {
                return new TestObjectController(obj);
            } else {
                throw new Exception("unknow mission type " + obj.GetType());
            }
        }

        public static BaseMissionObjectController GetObject(Type missionObjectType) {
            if (missionObjectType == typeof(BulletObject)) {
                return new BulletObjectController(new BulletObject(new Vector3K(), new Vector3K(0.5F, 0.5F, 0.5F), new Vector3K(1f,1f,1f), new Vector3K()));
            } else if (missionObjectType == typeof(PlayerObject)) {
                return new PlayerObjectController(new PlayerObject(new Vector3K(), new Vector3K(0.5F, 0.5F, 0.5F), new Vector3K(1f, 1f, 1f), new Vector3K()));
            } else if (missionObjectType == typeof(SkeletonObject)) {
                return new SkeletonObjectController(new SkeletonObject(new Vector3K(), new Vector3K(0.5F, 0.5F, 0.5F), new Vector3K(1f, 1f, 1f), new Vector3K()));
            } else if (missionObjectType == typeof(TestObject)) {
                return new TestObjectController(new TestObject(new Vector3K(), new Vector3K(0.5F, 0.5F, 0.5F), new Vector3K(1f, 1f, 1f), new Vector3K()));
            } else {
                throw new Exception("unknow mission type " + missionObjectType);
            }
        }
    }
}
