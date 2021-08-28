using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace so
{
    public abstract class SOVariable<T> : ScriptableObject
    {
       
        public T Value { get => _value; set { SetValue(value); } }
        [SerializeField] T startingValue;
        [SerializeField] private T _value;
   

        List<Action<T>> subscribers = new List<Action<T>>();

        public void SetValue(T newVal)
        {
            if ((_value == null && newVal != null) || (_value != null && !_value.Equals(newVal)))
            {
                _value = newVal;
                for (int i = 0; i < subscribers.Count; i++)
                {
                    subscribers[i]?.Invoke(_value);
                }
            }
        }

        public void Subscribe(Action<T> action)
        {
            subscribers.Add(action);
        }
        public void UnSubscribe(Action<T> action)
        {
            subscribers.Remove(action);
        }

        public static implicit operator T(SOVariable<T> v)
        {
            return v.Value;
        }

        private void OnAfterDeserialize()
        {
            _value = startingValue;
            UnSubscripeAll();
        }
        private void OnBeforeSerialize() { UnSubscripeAll(); ResetValue(); }
        private void UnSubscripeAll()
        {
            subscribers.Clear();
        }
        private void ResetValue()
        {
            _value = startingValue;
        }
    }
}