using Assets.Scripts.LoadingSystems.PersistentVariables;
using Assets.Scripts.Utilities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class PlayerPositionMessage : MonoBehaviour
    {
        // -- Inspector

        [Header("Parts")]
        public PersistentVector3 playerPosition;
    
        // -- Class

        private TMP_Text _text;
        private string _initialText;

        void Start()
        {
            _text = this.GetOrThrow<TMP_Text>();
            _initialText = _text.text;
        }
    
        void Update()
        {
            string position = $"{playerPosition.Value.x:0.0} | {playerPosition.Value.z:0.0}";
            _text.text = string.Format(_initialText, position);
        }
    }
}
