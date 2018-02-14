using System;
using System.Collections;
using UnityEngine;

namespace UThen {
    public class Do {
        public static DoInstruction Local {
            get
            {
                return new DoInstruction(false);
            }
        }
        
        public static DoInstruction Global {
            get
            {
                return new DoInstruction(true);
            }
        }
        
        /// <summary>
        /// Waits for end of frame prior to executing the .Then() action.
        /// </summary>
        public static Promise WaitForEndOfFrame { get { return Local.WaitForEndOfFrame; } }

        /// <summary>
        /// Waits for fixed update prior to executing the .Then() action.
        /// </summary>
        public static Promise WaitForFixedUpdate { get { return Local.WaitForFixedUpdate; } }

        /// <summary>
        /// Waits for a number of seconds prior to executing the .Then() action.
        /// </summary>
        public static Promise WaitFor( float seconds ) {
            return Local.WaitFor(seconds);
        }

        /// <summary>
        /// Executes the action prior to executing the .Then() action.
        /// </summary>
        public static Promise Yield( YieldInstruction yieldInstruction ) {
            if (yieldInstruction == null) throw new ArgumentNullException("yieldInstruction");

            return Local.Yield(yieldInstruction);
        }

        /// <summary>
        /// Executes the action prior to executing the .Then() action.
        /// </summary>
        public static Promise Yield( CustomYieldInstruction customYieldInstruction ) {
            if (customYieldInstruction == null) throw new ArgumentNullException("customYieldInstruction");

            return Local.Yield(customYieldInstruction);
        }

        /// <summary>
        /// Runs the coroutine prior to executing the .Then() action.
        /// </summary>
        public static Promise Run( IEnumerator coroutine ) {
            if (coroutine == null) throw new ArgumentNullException("coroutine");

            return Local.Run(coroutine);
        }

        /// <summary>
        /// Performs the action on every Update.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        public static DoOnUpdate OnUpdate( Action action ) {
            if (action == null) throw new ArgumentNullException("action");

            return Local.OnUpdate(action);
        }
    }
}