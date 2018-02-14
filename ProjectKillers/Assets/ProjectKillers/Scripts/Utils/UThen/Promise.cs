using System;
using System.Collections;
using UnityEngine;

namespace UThen {
    /// <summary>
    /// Allows chaining instructions one after another.
    /// <example>
    /// Chain.WaitForEndOfFrame.Then(actionA).Then(actionB);
    /// Chain.WaitForSeconds(1f).Then(action);
    /// Chain.Do(actionA).ThenWaitForSeconds(1f).Then(actionB);
    /// Chain.Yield(www).ThenWaitForSeconds(1f).Then(actionB);
    /// </example>
    /// </summary>
    public partial class Promise : PromiseBase<Promise> {
        public static Promise Create { get { return new Promise(); } }

        public Promise ThenYield( YieldInstruction yieldInstruction ) {
            if (yieldInstruction == null) throw new ArgumentNullException("yieldInstruction");

            Enqueue(yieldInstruction);
            
            return this;
        }
        
        public Promise ThenYield( CustomYieldInstruction yieldInstruction ) {
            if (yieldInstruction == null) throw new ArgumentNullException("yieldInstruction");

            Enqueue(yieldInstruction);
            
            return this;
        }

        public Promise ThenRun( IEnumerator coroutine ) {
            if (coroutine == null) throw new ArgumentNullException("coroutine");

            Enqueue( RunCoroutine(coroutine) );
            
            return this;
        }

        public Promise ThenDo( Action action ) {
            if (action == null) throw new ArgumentNullException("action");

            Enqueue(action);
            
            return this;
        }
    }
}