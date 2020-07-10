namespace Assets.Scripts.LoadingSystems.Trackings
{
    public interface ILoadingTracker
    {
        bool LoadingIsDone { get; }
        float Progress { get; }
    }
}