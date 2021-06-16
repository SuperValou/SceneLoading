using System;
using Assets.Scripts.LoadingSystems.Extensions;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.PersistentVariables
{
    [CreateAssetMenu(fileName = nameof(PersistentString), menuName = nameof(LoadingSystems) + "/"
                                                                + nameof(PersistentVariables) + "/"
                                                                + nameof(PersistentString))]
    public class PersistentString : ScriptableObject
    {
        [SerializeField] private string _value = string.Empty;

        public string Value
        {
            get => _value;
            set => Set(value);
        }

        public event Action<string> ValueChanged;

        protected virtual void Set(string str)
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
            ValueChanged.SafeInvoke(_value);
        }

        protected virtual void OnDisable()
        {
            // Clear subscribers
            ValueChanged = null;

            // Clear value
            _value = string.Empty;
        }
    }
}