using System;
using Assets.Scripts.LoadingSystems.PersistentVariables;
using Assets.Scripts.Utilities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class PersistentSwitch : MonoBehaviour
    {
        // -- Editor

        [Header("Value")]
        public Color enabledColor = Color.green;
        public Color disabledColor = Color.red;

        public string triggeringTag = "Player";

        [Header("Parts")]
        public PersistentBool state;
        public MeshRenderer meshRenderer;
        
        // -- Class

        void Start()
        {
            var col = this.GetOrThrow<Collider>();
            if (col.isTrigger)
            {
                throw new ArgumentException("Collider is not a Trigger.");
            }

            ApplyState(state.Value);
            state.ValueChanged += ApplyState;
        }

        void OnTriggerEnter(Collider collidingObject)
        {
            if (collidingObject.tag != triggeringTag)
            {
                return;
            }
            
            state.Value = !state.Value;
        }

        private void ApplyState(bool isTurnedOn)
        {
            var mat = meshRenderer.materials[0];
            if (isTurnedOn)
            {
                mat.color = enabledColor;
            }
            else
            {
                mat.color = disabledColor;
            }
        }

        void OnDestroy()
        {
            state.ValueChanged -= ApplyState;
        }
    }
}