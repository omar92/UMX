using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace so
{
    public class StateListener : MonoBehaviour
    {
        [System.Serializable]
        public struct StateBehaviourPair
        {
            public State state;
            public StateBehaviour stateBehaviour;
        }

        public StateBehaviourPair[] states = new StateBehaviourPair[0];
        private void OnEnable()
        {
            for (int i = 0; i < states.Length; i++)
            {
                states[i].state.Subscribe(states[i].stateBehaviour);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < states.Length; i++)
            {
                states[i].state.UnSubscribe(states[i].stateBehaviour);
            }
        }

    }
}