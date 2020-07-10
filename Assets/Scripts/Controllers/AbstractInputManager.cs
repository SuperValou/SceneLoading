using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public abstract class AbstractInputManager : MonoBehaviour
    {
        /// <summary>
        /// Returns the input vector for looking around.
        /// </summary>
        public abstract Vector2 GetLookVector();

        /// <summary>
        /// Returns the input vector for moving around.
        /// </summary>
        public abstract Vector2 GetMoveVector();

        /// <summary>
        /// Returns true during the frame where the Fire button is pressed.
        /// </summary>
        public abstract bool FireButtonDown();

        /// <summary>
        /// Returns wheter or not the Fire button is held down.
        /// </summary>
        public abstract bool FireButton();

        /// <summary>
        /// Returns true during the frame where the Fire button is released.
        /// </summary>
        public abstract bool FireButtonUp();

        public abstract bool RunButtonDown();
        public abstract bool RunButton();
        public abstract bool JumpButton();
        public abstract bool JumpButtonDown();
    }
}