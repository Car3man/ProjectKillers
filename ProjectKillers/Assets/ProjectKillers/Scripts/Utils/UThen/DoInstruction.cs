using System;
using System.Collections;
using UnityEngine;

namespace UThen {
    public sealed class DoInstruction {
        private readonly bool _isGlobal;

        internal DoInstruction( bool isGlobal ) {
            _isGlobal = isGlobal;
        }
        
        private Promise NewPromise {
            get {
                Promise promise = Promise.Create;

                if (_isGlobal) {
                    promise.Global();
                }

                return promise;
            }
        }
        
        /// <summary>
        /// Waits for end of frame prior to executing the .Then() action.
        /// </summary>
        public Promise WaitForEndOfFrame { get { return NewPromise.ThenYield(new WaitForEndOfFrame()); } }

        /// <summary>
        /// Waits for fixed update prior to executing the .Then() action.
        /// </summary>
        public Promise WaitForFixedUpdate { get { return NewPromise.ThenYield(new WaitForFixedUpdate()); } }

        /// <summary>
        /// Waits for a number of seconds prior to executing the .Then() action.
        /// </summary>
        public Promise WaitFor( float seconds ) {
            return NewPromise.ThenYield(new WaitForSeconds(seconds));
        }
        
        /// <summary>
        /// Executes the action prior to executing the .Then() action.
        /// </summary>
        public Promise Yield( YieldInstruction yieldInstruction) {
            if (yieldInstruction == null) throw new ArgumentNullException("yieldInstruction");

            return NewPromise.ThenYield(yieldInstruction);
        }
        
        /// <summary>
        /// Executes the action prior to executing the .Then() action.
        /// </summary>
        public Promise Yield( CustomYieldInstruction customYieldInstruction) {
            if (customYieldInstruction == null) throw new ArgumentNullException("customYieldInstruction");

            return NewPromise.ThenYield(customYieldInstruction);
        }
        
        /// <summary>
        /// Runs the coroutine prior to executing the .Then() action.
        /// </summary>
        public Promise Run( IEnumerator coroutine) {
            if (coroutine == null) throw new ArgumentNullException("coroutine");

            return NewPromise.ThenRun(coroutine);
        }
        
        /// <summary>
        /// Performs the action on every Update.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        public DoOnUpdate OnUpdate( Action action ) {
            if (action == null) throw new ArgumentNullException("action");

            return new DoOnUpdate(action);
        }
    }
}