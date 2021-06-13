# SceneLoading
Management of scene loading in Unity.

## How to install in your Unity project
*(work in progress)*

- Download the 0.5.0 version from the Release page
- In your Unity project, go to Assets > Import Package > Custom Package
- Browse to the location of the downloaded version, then import everything
- (optional) For housekeeping, it is recommend to create a new folder like "External/SceneLoading.0.5.0/" in your Assets folder, and move the new CrossSceneData/, Scripts/LoadingSystems/ and Scripts/LoadingSystems.Editor/ into it

## Concepts
*(work in progress)*

The following concepts are inspired from how classic Metroidvania games are usually broken down: big regions/sectors/area, consisting of small rooms/locations/level, that the player can explore. From this, four different types of scenes are defined:
- Master scene

Entry points, corresponding to the "big regions". These scenes actually do not contain any geometry, meshes or other "physical objects", but are responsible for loading the player and the first physical location she's supposed to be into. Additionally, main managers (shared between locations) are supposed to be found here.

- Gameplay scene

Everything related to the player. Controls, main camera and UI are expected to be found here.

- Room scene

A physical location in the game, usually small and contained. Environment like ground, walls, crates, and entities like enemies or bosses are expected to be found here.

- Screen

This scene type is not fully defined yet. For now, let's assume it's the expected type of a Title Screen or Game Over scene.

## Features
*(work in progress)*

- RestrictedSceneIdAttribute


## How to setup
*(work in progress)*

First, you have to put the correct labels on your scene files. Lets assume you have the following unity scene files:
- Initialization.unity being your Master scene
- Gameplay.unity being your Gameplay scene
- Forest.unity being a Room scene
- Beach.unity being a Room scene
- TitleScreen.unity being the main menu scene
- GameOver.unity being the game over screen

From the Editor, add the following labels on your scenes:
- Scene-Master on Initialization.unity
- Scene-Gameplay on Gameplay.unity
- Scene-Room on Forest.unity
- Scene-Room on Beach.unity
- Scene-Screen on TitleScreen.unity
- Scene-Screen on GameOver.unity

Once this is done, go to SceneLoading > Generate Scene classes. You'll now have the SceneId enumeration and the SceneInfo class available to load scene from your code.

Note that you can add your own types of scene if you want. Putting a "Scene-Foo" label on a scene file will register it as being of the type "Foo".

You can also go to SceneLoading > Load Scene > Generate Load Scene Menu to have a shortcut to load all your scene in the editor from the SceneLoading > Load Scene menu.


## How to get the demo project running
*(work in progress)*

The demo included in this project uses TextMeshPro. To avoid clogging the version control, TextMeshPro files were excluded from the repository. 
It means you'll have to do the following actions to get the demo project working properly:
- Clone the repository
- Open it in Unity 2019.2 or later
- Go to Window > TextMeshPro > **Import TMP Essential Ressources**
- Enter Play mode, then exit it to force the Editor to refresh
- The project should now be ready
