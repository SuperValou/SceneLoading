using Assets.Scripts.LoadingSystems.SceneLoadings.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.RoomManagement
{
    public interface IDoor
    {
        DoorState State { get; }
        Vector3 Position { get; }
        SceneId RoomOnTheOtherSide { get; }

        bool PlayerIsAround { get; }

        void NotifyLoadingProgress(float progress);
        void OpenInSync();
        void CloseInSync();

        void Lock();
        void Unlock();
    }
}