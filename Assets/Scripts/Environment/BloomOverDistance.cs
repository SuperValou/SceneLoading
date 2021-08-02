using System;
using Assets.Scripts.LoadingSystems.PersistentVariables;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Assets.Scripts.Environment
{
    public class BloomOverDistance : MonoBehaviour
    {
        // -- Inspector

        [Header("Values")]
        public float maxIntensity;
        public AnimationCurve factorOverDistance;

        [Header("References")]
        public PersistentVector3 playerPosition;
        public PostProcessProfile postProcessProfile;

        [Header("Debug")]
        public  AnimationCurve debugBloom;

        // -- Class

        [SerializeField]
        private AnimationCurve _factorOverSquaredDistance = new AnimationCurve();

        private Bloom _bloom;
        private float _initialBloomIntensity;

        private float _maxSquaredDistance;

        void Start()
        {
            const float Min = 0;
            const float Max = 1;

            foreach (var keyframe in factorOverDistance.keys)
            {
                float distance = keyframe.time;
                if (distance > Max)
                {
                    Debug.LogError($"{nameof(factorOverDistance)} keyframe must be between {Min} and {Max} on X axis.");
                    distance = Max;
                }

                if (distance < Min)
                {
                    Debug.LogError($"{nameof(factorOverDistance)} keyframe time must be between {Min} and {Max} on X axis.");
                    distance = Min;
                }

                float multiplier = keyframe.value;
                if (multiplier > Max)
                {
                    Debug.LogError($"{nameof(factorOverDistance)} keyframe value must be between {Min} and {Max} on Y axis.");
                    multiplier = Max;
                }

                if (multiplier < Min)
                {
                    Debug.LogError($"{nameof(factorOverDistance)} keyframe value must be between {Min} and {Max} on Y axis.");
                    multiplier = Min;
                }

                float squaredDistance = distance * distance;
                float inWeight = Mathf.Sqrt(keyframe.inWeight);
                float outWeight = Mathf.Sqrt(keyframe.outWeight);
                Keyframe newKeyframe = new Keyframe(squaredDistance, multiplier, keyframe.inTangent, keyframe.outTangent, inWeight, outWeight);
                _factorOverSquaredDistance.AddKey(newKeyframe);
            }

            _bloom = postProcessProfile.GetSetting<Bloom>() ?? throw new ArgumentNullException(nameof(Bloom));
            _initialBloomIntensity = _bloom.intensity.value;
        }

        void Update()
        {
            debugBloom.AddKey(Time.realtimeSinceStartup, _bloom.intensity.value);
            if (_maxSquaredDistance <= 0)
            {
                return;
            }

            Vector3 relativePlayerPosition = playerPosition.Value - transform.position;
            float squaredDistance = Vector3.SqrMagnitude(relativePlayerPosition);

            float clampedSquaredDistance = Mathf.Clamp(squaredDistance, min: 0, max: _maxSquaredDistance);
            float normalizedSquaredDistance = clampedSquaredDistance / _maxSquaredDistance;
            float multiplier = _factorOverSquaredDistance.Evaluate(normalizedSquaredDistance);
            
            _bloom.intensity.value = multiplier * maxIntensity + (1 - multiplier) * _initialBloomIntensity;
        }

        public void StartApply()
        {
            Vector3 relativePlayerPosition = playerPosition.Value - transform.position;
            float squaredDistance = Vector3.SqrMagnitude(relativePlayerPosition);

            _maxSquaredDistance = Mathf.Max(float.Epsilon, squaredDistance);
        }

        public void Reset()
        {
            _bloom.intensity.value = _initialBloomIntensity;
            _maxSquaredDistance = -1;
        }

        void OnDestroy()
        {
            _bloom.intensity.value = _initialBloomIntensity;
        }
    }
}
