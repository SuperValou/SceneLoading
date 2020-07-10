namespace Assets.Scripts.LoadingSystems.Systems.SceneInfos
{
    public enum SceneId
    {
        [SceneInfo("MasterScene", SceneType.Master)]
        MasterScene = 0,

        [SceneInfo("0-Gameplay", SceneType.Gameplay)]
        GameplayScene,

        [SceneInfo("1-FirstRoom", SceneType.Room)]
        WakeUpRoomScene,

        [SceneInfo("lol", SceneType.Room)]
        CorridorRoomScene,

        [SceneInfo("lul", SceneType.Room)]
        BossRoomScene
    }
}