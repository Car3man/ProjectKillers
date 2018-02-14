using UnityEngine;

namespace UThen {
    public sealed partial class Promise {
        public Promise ThenWaitForEndOfFrame { get { return this.ThenYield(new WaitForEndOfFrame()); } }

        public Promise ThenWaitForFixedUpdate { get { return this.ThenYield(new WaitForFixedUpdate()); } }

        public Promise ThenWaitForSeconds( float seconds ) {
            return this.ThenYield(new WaitForSeconds(seconds));
        }

        public Promise ThenWaitForSecondsRealtime( float seconds ) {
            return this.ThenYield(new WaitForSecondsRealtime(seconds));
        }
    }
}