namespace Assets.Scripts.LoadingSystems.SceneInfos
{
    public enum SceneId
    {
        [SceneInfo("MasterScene", SceneType.Master)]
        MasterScene = 0,

        [SceneInfo("0-Gameplay", SceneType.Gameplay)]
        GameplayScene,

        [SceneInfo("1-FirstRoom", SceneType.Room)]
        WakeUpRoomScene,

        [SceneInfo("2-TransitionCorridor", SceneType.Room)]
        CorridorRoomScene,

        [SceneInfo("3-BossRoom", SceneType.Room)]
        BossRoomScene,

        [SceneInfo("3-1-Corridor-A", SceneType.Room)]
        CorridorA,

        [SceneInfo("3-2-Corridor-B", SceneType.Room)]
        CorridorB,

        [SceneInfo("3-3-Corridor-C", SceneType.Room)]
        CorridorC
    }
}