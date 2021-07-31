using Assets.Scripts.LoadingSystems.Doors;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Doors
{
    public class SlidingDoor : Door
    {
        // -- Editor

        [Header("Animator constants")]
        [SerializeField]
        private string OnOpenTriggerName = "OnOpen";

        [SerializeField]
        private string OnCloseTriggerName = "OnClose";

        // -- Class

        private Animator _animator;

        void Start()
        {
            _animator = this.GetOrThrow<Animator>();
        }

        protected override void OnLoading(float progress)
        {
            // discard
        }

        protected override void OnOpen()
        {
            _animator.ResetTrigger(OnCloseTriggerName);
            _animator.SetTrigger(OnOpenTriggerName);
        }

        protected override void OnClose()
        {
            _animator.ResetTrigger(OnOpenTriggerName);
            _animator.SetTrigger(OnCloseTriggerName);
        }
    }
}