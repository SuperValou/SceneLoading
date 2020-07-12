namespace Assets.Scripts.LoadingSystems.RoomManagement
{
    public enum DoorState
    {
        /// <summary>
        /// Door is normally closed.
        /// </summary>
        Closed,

        /// <summary>
        /// Door is closed, and the player wants to open it.
        /// </summary>
        WaitingToOpen,

        /// <summary>
        /// Door is opening.
        /// </summary>
        Opening,

        /// <summary>
        /// Door is completely opened.
        /// </summary>
        Opened,

        /// <summary>
        /// Door is completely opened and the player wants to close it.
        /// </summary>
        WaitingToClose,

        /// <summary>
        /// Door is closing.
        /// </summary>
        Closing,

        /// <summary>
        /// Door is closed and cannot be opened by the player in the current situation.
        /// </summary>
        Locked
    }
}