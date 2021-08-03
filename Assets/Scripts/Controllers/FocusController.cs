using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class FocusController : MonoBehaviour
    {
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
            }

            if (Input.anyKeyDown)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}