[![Generic badge](https://img.shields.io/badge/Status-WIP-yellow.svg)](https://shields.io/)

# Scenes

Three types are available to manager scenes from the code:
- SceneId (enum)
- SceneInfo (class)
- SceneType (enum)

*SceneType enum*
This enum represents the kind of scenes that are available in your game. The default types are Master, Gameplay, Room, and Screen, but you can add your own (see Editor Tools).

# Register your scenes
To register a scene into the list of managed scenes, you first have to add a label on it. The label has to start with "Scene-" and be followed by the type of the scene.

TODO: duplicate what is in the release note of 0.5.0