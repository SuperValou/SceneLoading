# SceneLoading
Management of scene loading in a Metroidvania fashion for Unity.

![Screenshot_3](https://user-images.githubusercontent.com/6672340/130272738-47b7f27e-a3ac-4646-88fd-280b16849643.png)

SceneLoading is a Unity package to handle transitioning between different rooms. To illustrate what it does and ensure it works as expected, I built a mini-game with it. The demo shows that you can split rooms into individual scenes and still have your player moving freely between them like they were all part of the same area. Information can also be "remembered" by a room once it is unloaded/reloaded, and communication can also occur accros different rooms. 



## Try it out
- [Play the demo](https://supervalou.itch.io/sceneloading-demo) on itch.io
- Want to have a quick look at it? Watch the video below

https://user-images.githubusercontent.com/6672340/134586343-fa3649bd-4448-415c-958f-1d06f2d2a062.mp4





## Documentation
- [Manual](https://supervalou.github.io/SceneLoading/manual/Concepts.html)
- [Scripting API](https://supervalou.github.io/SceneLoading/api/Assets.Scripts.LoadingSystems.SceneLoadings.SceneLoadingSystem.html)

## Getting started

This repository includes the project of the demo showcasing what you can do with this package. As it's both the sandbox from which the package gets developped and an actual demo of its features, a few tricks have to be applied to make the demo run properly (a tool to automate this process may appear at some point):
- Clone the repository
- Open the project in Unity 2019.2 or later
- In the menu bar, go to SceneLoading and click on Generate Scene classes
- Once compilation is finished, open the file at "...\SceneLoading\Assets\Scripts\SceneId_export.txt" and copy all its content
- Open the file at "...\SceneLoading\Assets\Scripts\LoadingSystems\SceneInfos\SceneId.Generated.cs" in a text editor
- Paste what was copied to replace the content of the enum declaration (the main idea is to set the correct numbers for each member instead of the auto-generated ones)
- Save the file and go back to Unity
- (Optional) Go to SceneLoading > Load Scene and click on Generate Load Scene menu
- Once compilation is finished, load Master.unity (or go to SceneLoading > Load Scene > Master > Load Master Scene if the menu was generated at the previous step)
- Hit play to start the demo project
