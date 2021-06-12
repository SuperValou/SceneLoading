using Assets.Scripts.LoadingSystems.Doors;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Doors
{
    public class PrototypeDoor : Door
    {
        // -- Editor

        // -- Class

        private Animator _animator;

        protected override void Start()
        {
            base.Start();
            _animator = this.GetOrThrow<Animator>();
        }

        protected override void OnLoading(float progress)
        {
            // discard
        }

        protected override void OnOpen()
        {
            _animator.SetTrigger("OnOpen");
        }

        protected override void OnClose()
        {
            _animator.SetTrigger("OnClose");
        }
    }
}