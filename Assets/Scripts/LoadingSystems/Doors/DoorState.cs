namespace Assets.Scripts.LoadingSystems.Doors
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
        /// Door is completely open.
        /// </summary>
        Open,
        
        /// <summary>
        /// Door is closed and cannot be opened by the player in the current situation.
        /// </summary>
        Locked
    }
}