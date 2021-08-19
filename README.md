# SceneLoading
Management of scene loading in a Metroidvania fashion for Unity.

## Quick demo
Click on the video below to get an idea of what it does.

**(video coming soon)**

## Documentation
Documentation about concepts, installation, how to get started, etc. is available [here](https://supervalou.github.io/SceneLoading/)


## How to get the demo project running

This repository includes a full demo of what you can do with this package. As it's both the sandbox from which the package gets developped and an actual demo of its features, a few tricks have to be applied to make the demo run properly (a tool to automate this process may appear at some point):
- Clone the repository
- Open the project in Unity 2019.2 or later
- In the menu bar, go to SceneLoading and click on Generate Scene classes
- Once compilation is finished, open the file at "...\SceneLoading\Assets\Scripts\SceneId_export.txt" and copy all its content
- Open the file at "...\SceneLoading\Assets\Scripts\LoadingSystems\SceneInfos\SceneId.Generated.cs" in a text editor
- Paste what was copied to replace the content of the enum declaration (the main idea is to set the correct numbers for each member instead of the auto-generated ones)
- Save the file and go back to Unity
- (Optional) Go to SceneLoading > Load Scene and click on Generate Load Scene menu
- Once compilation is finished, load Master.unity (or go to SceneLoading > Load Scene > Master > Load Master Scene if the menu was generated at the previous step)
- Hit play to start the demo
