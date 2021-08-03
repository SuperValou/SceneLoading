using Assets.Scripts.LoadingSystems.PersistentVariables;
using UnityEngine;

namespace Assets.Scripts.RoomScripts
{
    public class LabRoomLayout : MonoBehaviour
    {
        // -- Inspector

        [Header("Conditions")]
        public PersistentBool eastGeneratorState;
        public PersistentBool westGeneratorState;

        [Header("GameObjects")]
        public GameObject[] gameObjects;


        // -- Class

        void Awake()
        {
            bool portalActivated = eastGeneratorState.Value && westGeneratorState.Value;
            foreach (var gameObj in gameObjects)
            {
                gameObj.SetActive(portalActivated);
            }
        }
    }
}
