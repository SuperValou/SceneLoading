namespace Assets.Scripts.LoadingSystems.SceneLoadings.SceneInfos
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
        BossRoomScene
    }
}