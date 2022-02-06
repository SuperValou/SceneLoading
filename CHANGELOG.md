# 2.0.0

**2022-02-05**

### Core updates
- SceneLoading now relies on ScriptableObjects instead of generated C# classes
- Added the RoomLoader component to be able to trigger a scene load from anywhere (instead of specifically at doors)
- SceneLoading now relies on the UniKit package

### Editor updates
- Removed C# generation
- Added auto-generation of SceneInfo ScriptableObjects from labels on .unity files

### Documentation updates
- Added some articles about Installation, Editor Tools, and basic overview of the available classes and components

# 1.0.0

**2021-09-23**

[Play the demo on itch.io](https://supervalou.itch.io/sceneloading-demo)


# 0.6.0

**2021-07-05**

### Core updates
- Added Persistent classes for bool, float, int, string, Quaternion, Vector3, sets and and generic TStruct (like enums) that can be used for:
    - sharing data accross different scenes
    - restoring a previous state when a scene gets reloaded after being unloaded

### Editor updates
- Refactored the SceneId/SceneInfo/SceneType C# types generation to avoid bugs and issues
- Fixed a bug in the Editor that was making the inspector flicker when selecting a Door prefab

### Documentation updates
- Added API documentation (auto-generated from C# comments)
- Added few articles to get started with the concepts in the package





# 0.5.0

**2021-06-10**

### Editor changes
- A new menu is now available to directly load a scene into the Editor: **Scene Loading > Load Scene**
    - First, be sure your scenes are labelled  properly, then execute "Scene Loading > Generate Scene Classes" to ensure the corresponding C# are up to date.
    - Use "Scene Loading > Generate Load Scene" to generate the menu from the C# classes.
    - Use SceneLoading > Room > MyRoom to load your room directly into the Editor.
![LoadSceneMenu](https://user-images.githubusercontent.com/6672340/121600671-18a5b800-ca45-11eb-9ae3-d0643e6e76e9.png)

- SceneLoading.Editor was not flagged as Editor only, so the game could not be built. This is now fixed.

### Core changes
- 0.5.0 now relies on ScriptableObjects to share data between Scenes, instead of proxy classes calling Object.FindObjectOfType<T> on Awake.





# 0.4.0

**2021-05-10**

The type of a scene is now defined by the label put on the .unity asset. For example, a scene like "BossArena.unity" can be labeled with "Scene-Room" to identify this scene as a Room. Default scene types are Master, Room, Gameplay, and Screen, but other custom types can be added.

# 0.3.0

**2021-03-24**

*(no release note)*



# 0.2.0

**2021-03-15**

*(no release note)*



# 0.1.0

**2020-07-22**

- Create separate lib for scene loading system