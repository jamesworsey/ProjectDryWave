# DryWave
A VR surfing simulation for use in training high-level surfers to become comfortable with big-wave surfing (for olympic-style events). Compatible with the HTC Vive and Value Index headsets and developed with Unity 2018.4 and version 2 of the steamVR sdk.

## Installation
1. Ensure Unity Version 2018.4, Git and the Steam VR program are installed.
2. Create a new directory which will contain the project files.
3. Navigate to this directory with the git bash terminal.
4. Initialize a new repository with:
	```bash
	git init
	```
5. Add the remote repository with:
	 ```bash
	 git remote add [Remote Name] [Remote link]
	 ```
 6. Pull the remote repository with:
	 ```bash
	 git pull [Remote Name] master
	  ```
  7. If using Unity Hub to manage installations, click *Add* and select the directory containing the repository pulled from Github. 
  **Note:** when no headset or controllers are connected Unity might display errors in the console. 

## Project Structure
- **Root:** The projects root directory mainly contains files associated with Unity as well as the git repository. Any file that should not be added to the repository should be added to the .gitignore file here.
- **Assets:** The majority of project files are located here, this is the directory opened in Unity's project window. 
	- **Scripts** contains the main source code of the project
	- **Materials** contains material files for object textures
	- **Prefabs** Unity Gameobjects that have been saved locally
	- **Scenes** contains Unity's level files 
	- **SteamVR** directories contain the files for the Steam SDK 
- **Builds:** The Executables for the project  once a version has been built.
