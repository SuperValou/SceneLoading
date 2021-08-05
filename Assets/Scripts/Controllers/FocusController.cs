using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class FocusController : MonoBehaviour
    {
        public bool Locked { get; private set; }

        void Start()
        {
            Lock();
        }

        void Update()
        {
            if (Locked && Input.GetKeyDown(KeyCode.Escape))
            {
                Unlock();
            }
            else if (!Locked && Input.GetMouseButtonDown(0))
            {
                Lock();
            }
        }

        public void Lock()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Locked = true;
        }

        public void Unlock()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Locked = false;
        }

        void OnDestroy()
        {
            Unlock();
        }
    }
}