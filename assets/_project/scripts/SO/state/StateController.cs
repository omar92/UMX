using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO {
    public class StateController : MonoBehaviour
    {
        public SMType StateMachineType;
        public State StartingState;

        private State lastState;

        private void Awake()
        {
            StateMachineType.Subscribe(OnStateChange);
        }

        private void Start()
        {
            StateMachineType.Value = StartingState;
            OnStateChange(StartingState);
        }
        private void OnStateChange(State state)
        {
            if (lastState != null) lastState.OnExit();
            lastState = state;
            state.OnEnter();
        }

    }
}