using System;
using Box2DX.Dynamics;
using ProjectKillersCommon.Data.Objects;

namespace ProjectKillersServer.Physics {
    public class PhysicsSolver : ContactListener {
        public delegate void EventSolver (BaseMissionObject body1, BaseMissionObject body2);
        public event EventSolver OnAdd;
        public event EventSolver OnPersist;
        public event EventSolver OnResult;
        public event EventSolver OnRemove;

        public override void Add (ContactPoint point) {
            base.Add(point);

            OnAdd?.Invoke((BaseMissionObject)point.Shape1.GetBody().GetUserData(), (BaseMissionObject)point.Shape2.GetBody().GetUserData());
        }

        public override void Persist (ContactPoint point) {
            base.Persist(point);

            OnPersist?.Invoke((BaseMissionObject)point.Shape1.GetBody().GetUserData(), (BaseMissionObject)point.Shape2.GetBody().GetUserData());
        }

        public override void Result (ContactResult point) {
            base.Result(point);

            OnResult?.Invoke((BaseMissionObject)point.Shape1.GetBody().GetUserData(), (BaseMissionObject)point.Shape2.GetBody().GetUserData());
        }

        public override void Remove (ContactPoint point) {
            base.Remove(point);

            OnRemove?.Invoke((BaseMissionObject)point.Shape1.GetBody().GetUserData(), (BaseMissionObject)point.Shape2.GetBody().GetUserData());
        }
    }
}
