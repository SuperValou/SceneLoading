using System;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.CrossSceneObjects
{
    public abstract class CrossSceneString : ScriptableObject
    {
        // -- Editor 

        [SerializeField]
        private string _value = string.Empty;

        // -- Class

        public string Value => _value;

        public void Set(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (_value == str)
            {
                return;
            }

            _value = str;
            OnValueChanged();
        }

        public void Reset()
        {
            if (_value == string.Empty)
            {
                return;
            }

            _value = string.Empty;
            OnValueChanged();
        }

        protected abstract void OnValueChanged(); // TODO: integrate SafeInvoke from Core

        protected virtual void OnDisable()
        {
            // Clear value
            _value = string.Empty;
        }
    }
}