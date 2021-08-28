using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace so
{
    public abstract class SOVariable<T> : ScriptableObject, ISerializationCallbackReceiver
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
                Invoke();
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
        public void Invoke()
        {
            for (int i = 0; i < subscribers.Count; i++)
            {
                subscribers[i]?.Invoke(_value);
            }
        }

        public static implicit operator T(SOVariable<T> v)
        {
            return v.Value;
        }

        private void UnSubscripeAll()
        {
            subscribers.Clear();
        }
        private void ResetValue()
        {
            _value = startingValue;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            UnSubscripeAll(); ResetValue();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _value = startingValue;
            UnSubscripeAll();
        }
    }
}