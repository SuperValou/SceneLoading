using Assets.Scripts.LoadingSystems.PersistentVariables;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    [RequireComponent(typeof(Animator))]
    public class OnOffAnimator : MonoBehaviour
    {
        // -- Editor

        [Header("Parts")]
        public PersistentBool initialState;

        [Header("Animator constants")]
        [SerializeField]
        private string _turnedOnStateName = "TurnedOn";

        [SerializeField]
        private string _turnedOffStateName = "TurnedOff";

        [SerializeField]
        private string _turnOnTriggerName = "TurnOnTrigger";

        [SerializeField]
        private string _turnOffTriggerName = "TurnOffTrigger";


        // -- Class

        private Animator _animator;

        void Start()
        {
            _animator = this.GetOrThrow<Animator>();

            if (initialState.Value)
            {
                _animator.Play(_turnedOnStateName);
            }
            else
            {
                _animator.Play(_turnedOffStateName);
            }
        }

        public void TurnOn()
        {
            _animator.ResetTrigger(_turnOffTriggerName);
            _animator.SetTrigger(_turnOnTriggerName);
        }

        public void TurnOff()
        {
            _animator.ResetTrigger(_turnOnTriggerName);
            _animator.SetTrigger(_turnOffTriggerName);
        }
    }
}