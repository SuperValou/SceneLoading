using UnityEngine;

namespace Assets.Scripts.LoadingSystems.CrossSceneObjects
{
    public class CrossScene<TStruct> : ScriptableObject
        where TStruct : struct
    {
        [SerializeField]
        private TStruct _value;

        public TStruct Value
        {
            get => _value;
            set => _value = value;
        }
    }
}