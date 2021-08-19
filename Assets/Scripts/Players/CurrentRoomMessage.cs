using Assets.Scripts.LoadingSystems.PersistentVariables;
using Assets.Scripts.Utilities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class CurrentRoomMessage : MonoBehaviour
    {
        public PersistentRoomId playerCurrentRoom;

        private TMP_Text _text;
        private string _initialText;

        void Start()
        {
            _text = this.GetOrThrow<TMP_Text>();
            _initialText = _text.text;
        }

        void Update()
        {
            _text.text = string.Format(_initialText, playerCurrentRoom.Value.ToString());
        }
    }
}