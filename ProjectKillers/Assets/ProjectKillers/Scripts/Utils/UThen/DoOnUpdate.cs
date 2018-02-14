using System;
using System.Collections;
using UnityEngine;

namespace UThen {
    /// <summary>
    /// Performs an action on every Update.
    /// </summary>
    public sealed class DoOnUpdate : UThenObject<DoOnUpdate> {
        private readonly Action _updateAction;
        private Func<bool> _updateWhilePredicate;

        public DoOnUpdate( Action updateAction) {
            if (updateAction == null) throw new ArgumentNullException("updateAction");
            _updateAction = updateAction;

            GlobalCoroutineRunner.RunCoroutine(DoEveryFrame());
        }
        
        public DoOnUpdate While( Func<bool> predicate) {
            if (predicate == null) throw new ArgumentNullException("predicate");

            _updateWhilePredicate = predicate;

            return this;
        }
        
        private IEnumerator DoEveryFrame() {
            while (true) {
                if (_updateWhilePredicate != null && _updateWhilePredicate() == false)
                    break;
                
                _updateAction();
                yield return new WaitForEndOfFrame();
            }
        }
    }
}