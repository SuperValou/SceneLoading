namespace Assets.Scripts.LoadingSystems.Trackings
{
    internal class AlreadyDoneTracker : ILoadingTracker
    {
        public bool LoadingIsDone => true;
        public float Progress => 1f;
    }
}