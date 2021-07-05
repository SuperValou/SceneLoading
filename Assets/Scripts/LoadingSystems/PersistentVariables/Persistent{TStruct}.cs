using System;
using Assets.Scripts.LoadingSystems.Extensions;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.PersistentVariables
{
    public abstract class Persistent<TStruct> : ScriptableObject
        where TStruct : struct
    {
        [SerializeField]
        private TStruct _value;

        public TStruct Value
        {
            get => _value;
            set => Set(value);
        }

        public event Action<TStruct> ValueChanged;

        public virtual void Set(TStruct value)
        {
            _value = value;
            ValueChanged.SafeInvoke(_value);
        }

        protected virtual void OnDisable()
        {
            // Clear subscribers
            ValueChanged = null;

            // Clear value
            _value = default;
        }
    }
}