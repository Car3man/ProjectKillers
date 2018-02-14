using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UThen {
    public abstract class PromiseBase<T> : UThenYieldInstruction<PromiseBase<T>> {
        private bool _isChainStarted;
        
        private readonly Queue<object> _instructionsQueue = new Queue<object>();

        protected void Enqueue( object obj ) {
            _instructionsQueue.Enqueue(obj);

            if (_instructionsQueue.Count == 0 && obj is Action) {
                throw new InvalidOperationException("First instruction in a chain can't be an Action.");
            }

            StartChain();
        }

        private void StartChain() {
            if (!_isChainStarted && _instructionsQueue.Count > 0) {
                RunCoroutine(DoRunChain());
            }

            _isChainStarted = true;
        }

        private IEnumerator DoRunChain() {
            KeepWaiting = true;
            
            while (_instructionsQueue.Count > 0) {
                object instruction = _instructionsQueue.Dequeue();

                if (instruction is YieldInstruction)
                {
                    yield return instruction as YieldInstruction;
                    continue;
                }

                if (instruction is CustomYieldInstruction)
                {
                    yield return instruction as CustomYieldInstruction;
                    continue;
                }

                if (instruction is Action)
                {
                    if (instruction != null)
                        (instruction as Action)();
                    
                    continue;
                }

                throw new InvalidOperationException("Unsupported instruction type " + instruction.GetType());
            }
            
            KeepWaiting = false;
        }
    }
}