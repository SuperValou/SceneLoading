using Assets.Scripts.LoadingSystems.SceneLoadings.SceneInfos;
using Assets.Scripts.LoadingSystems.Trackings;

namespace Assets.Scripts.LoadingSystems.RoomManagement
{
    public interface IDoor
    {
        DoorState State { get; }
        SceneId RoomOnTheOtherSide { get; }

        void Open(ILoadingTracker openingTracker);

        void Lock();
        void Unlock();
    }

    public enum DoorState
    {
        Closed,
        RequestingOpening,
        Opening,
        Opened,
        Closing,
        Locked
    }
}