using ProjectKillersCommon.Data.Objects;

namespace ProjectKillersServer.Controllers.Objects {
    public class PlayerObjectController : BaseMissionObjectController {
        public PlayerObjectController(BaseMissionObject obj) : base(obj) {

        }

        public override void Update(float deltaTime) {
            CheckHealth();
        }

        private void CheckHealth() {
            if ((Object as IHuman).Health <= 0) {
                Destroy();
            }
        }
    }
}
