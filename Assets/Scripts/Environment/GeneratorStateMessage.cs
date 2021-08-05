using Assets.Scripts.LoadingSystems.PersistentVariables;
using Assets.Scripts.Utilities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class GeneratorStateMessage : MonoBehaviour
    {
        // -- Inspector

        [Header("Parts")]
        public PersistentBool generatorState;

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
            string state = generatorState.Value ? "on" : "off";
            _text.text = string.Format(_initialText, state);
        }
    }
}