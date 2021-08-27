using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SO
{
    [CreateAssetMenu(fileName = "state", menuName = "SO/SM/state",order =0)]
    public class State : ScriptableObject
    {
        public SMType StateMachineType;

        List<StateBehaviour> stateBehaviours = new List<StateBehaviour>();

        public void Switch()
        {
            StateMachineType.Value = this;
        }


        public void Subscribe(StateBehaviour stateBehaviour)
        {
            if (!stateBehaviours.Contains(stateBehaviour))
            {
                stateBehaviours.Add(stateBehaviour);
            }
        }

        public void UnSubscribe(StateBehaviour stateBehaviour)
        {
            if (stateBehaviours.Contains(stateBehaviour))
            {
                stateBehaviours.Remove(stateBehaviour);
            }
        }


        public void OnEnter()
        {
            CoRef.StartCoroutineAway(ExcuteAtEndOfFrameCO(() =>
            {
                for (int i = 0; i < stateBehaviours.Count; i++)
                {
                    stateBehaviours[i].OnEnter.Invoke();
                }
            }));
        }
        public void OnExit()
        {
            CoRef.StartCoroutineAway(ExcuteAtEndOfFrameCO(() =>
            {
                for (int i = 0; i < stateBehaviours.Count; i++)
                {
                    stateBehaviours[i].OnExit.Invoke();
                }
            }));
        }
        IEnumerator ExcuteAtEndOfFrameCO(Action action)
        {
            yield return new WaitForEndOfFrame();
            action();
        }
    }
}