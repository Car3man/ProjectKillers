using System;
using Box2DX.Dynamics;
using ProjectKillersCommon.Data.Objects;
using ProjectKillersServer.Controllers.Objects;

namespace ProjectKillersServer {
    public class PhysicsSolver : ContactListener {
        public delegate void EventSolver (BaseMissionObjectController body1, BaseMissionObjectController body2);
        public event EventSolver OnAdd;
        public event EventSolver OnPersist;
        public event EventSolver OnResult;
        public event EventSolver OnRemove;

        public override void Add (ContactPoint point) {
            base.Add(point);

            OnAdd?.Invoke((BaseMissionObjectController)point.Shape1.GetBody().GetUserData(), (BaseMissionObjectController)point.Shape2.GetBody().GetUserData());
        }

        public override void Persist (ContactPoint point) {
            base.Persist(point);

            OnPersist?.Invoke((BaseMissionObjectController)point.Shape1.GetBody().GetUserData(), (BaseMissionObjectController)point.Shape2.GetBody().GetUserData());
        }

        public override void Result (ContactResult point) {
            base.Result(point);

            OnResult?.Invoke((BaseMissionObjectController)point.Shape1.GetBody().GetUserData(), (BaseMissionObjectController)point.Shape2.GetBody().GetUserData());
        }

        public override void Remove (ContactPoint point) {
            base.Remove(point);

            OnRemove?.Invoke((BaseMissionObjectController)point.Shape1.GetBody().GetUserData(), (BaseMissionObjectController)point.Shape2.GetBody().GetUserData());
        }
    }
}
