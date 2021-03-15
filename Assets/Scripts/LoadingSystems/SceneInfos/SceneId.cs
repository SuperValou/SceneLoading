namespace Assets.Scripts.LoadingSystems.SceneInfos
{
    public enum SceneId
    {
        [SceneInfo("MasterScene", SceneType.Master)]
        MasterScene = 0,

        [SceneInfo("0-Gameplay", SceneType.Gameplay)]
        GameplayScene,

        [SceneInfo("1-WakeUpRoom", SceneType.Room)]
        WakeUpRoomScene,

        [SceneInfo("1-1-TransitionCorridorRoom", SceneType.Room)]
        CorridorRoomScene,

        [SceneInfo("3-HubRoom", SceneType.Room)]
        BossRoomScene,

        [SceneInfo("3-1-CorridorRoom", SceneType.Room)]
        CorridorA,

        [SceneInfo("3-2-CorridorRoom", SceneType.Room)]
        CorridorB,

        [SceneInfo("4-1-LabAccessRoom", SceneType.Room)]
        CorridorC,

        [SceneInfo("4-LabRoom", SceneType.Room)]
        Lab
    }
}