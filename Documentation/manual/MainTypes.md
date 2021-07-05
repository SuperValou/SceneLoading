[![Generic badge](https://img.shields.io/badge/Status-WIP-yellow.svg)](https://shields.io/)

# Scenes

The following C# types are available to manage scenes from code.

### SceneType enum
The **SceneType** enum represents the type of scenes that are available in the project. 

From the definitions of [this page](~/manual/Concepts.md), default scene types are the following:
```csharp
public enum SceneType
{
	Master = 0,
	Gameplay = 1,
	Room = 2,
	Screen = 3	
}
```

#### Setting the type of a scene
The type of a scene is actually defined by the **label** put on the .unity asset file. 

The label **must** start with `Scene-` to be acknowledged as a valid SceneType label.

For example, a scene like "BossArena.unity" can be labeled with `Scene-Room` to identify this scene as a `Room`.

![Labelling a scene](~/resources/scene_label.png)

Note that the system will reject any scene having more than one `Scene-` label.

#### Custom scene types
You can add your own custom scene types: labelling a .unity file with `Scene-Playground` will make the scene be acknowledged as a `Playground` scene. 

However, it is recommended to write a strong definition of any new type of scene beforehand (see the [Concepts](~/manual/Concepts.md) page).



--------------------------------



### SceneId enum
The **SceneId** enum identifies all the scenes that were acknoledged by the system. Each scene have a corresponding unique enum member and enum value. 

The enum members are automatically generated (see [Editor Tools](~/manual/EditorTools.md) for more information about the generation itself).

```csharp
public enum SceneId
{
	AreaMaster = 1,
	SinglePlayerGamplay = 10001,
	BigRoom = 20001,
	BossChamber = 20002,
	SmallRoom = 20003,
	// etc.
}
```

Note there is a current limitation to 9,999 rooms before `SceneId` values start colliding. To put it in perspective, Castlevania Symphony of the Night contains a total of 1,890 rooms.

Also note that any scene not marked with a `Scene-` label will not be listed in the enumeration. This way you can have as many test scenes as you want without clogging the enum.



--------------------------------



### SceneInfo class
The **SceneInfo** class holds information about each scene in the system. The class also provide some methods to retrieve this data.

```csharp
IReadOnlyCollection<SceneInfo> sceneInfos = SceneInfo.GetAll();
foreach (SceneInfo sceneInfo in sceneInfos)
{
	Debug.Log($"{sceneInfo.SceneName} is a {sceneInfo.Type} scene.");
	// example: "BossChamber is a Room scene."
}
```

For more information about this class, you can visit its [API page](~/api/Assets.Scripts.LoadingSystems.SceneInfos.SceneInfo.yml).









