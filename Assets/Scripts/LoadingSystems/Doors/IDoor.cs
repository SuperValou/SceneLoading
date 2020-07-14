using Assets.Scripts.LoadingSystems.SceneInfos;
using UnityEngine;

namespace Assets.Scripts.LoadingSystems.Doors
{
    public interface IDoor
    {
        SceneInfo SceneInfo { get; }
        SceneId Room { get; }
        SceneId RoomOnTheOtherSide { get; }
        DoorState State { get; }
        Vector3 Position { get; }
        
        bool PlayerIsAround { get; }

        void NotifyLoadingProgress(float progress);
        void OpenInSync();
        void CloseInSync();

        void Lock();
        void Unlock();
    }
}