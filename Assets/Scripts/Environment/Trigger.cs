using System;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Environments
{
    public class Trigger : MonoBehaviour
    {
        // -- Editor
        [Header("Values")]
        public string triggeringTag = "Player";

        [Header("Events")]
        public UnityEvent onTriggerEnter;

        public UnityEvent onTriggerExit;

        // -- Class

        void Start()
        {
            var col = this.GetOrThrow<Collider>();
            if (!col.isTrigger)
            {
                throw new ArgumentException($"Collider of {this.name} ({this.GetType().Name}) is not a Trigger. It will never be triggered.");
            }
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.tag != triggeringTag)
            {
                return;
            }

            onTriggerEnter.Invoke();
        }

        void OnTriggerExit(Collider col)
        {
            if (col.tag != triggeringTag)
            {
                return;
            }

            onTriggerExit.Invoke();
        }
    }
}