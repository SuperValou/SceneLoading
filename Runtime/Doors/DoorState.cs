namespace Packages.SceneLoading.Runtime.Doors
{
    /// <summary>
    /// State of a <see cref="Door"/>.
    /// </summary>
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
    }
}