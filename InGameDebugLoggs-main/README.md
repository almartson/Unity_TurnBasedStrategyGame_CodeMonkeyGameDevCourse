![alttext](https://img.shields.io/badge/Unity%20version-2021.3.16f1-lightgrey&?style=for-the-badge&logo=unity&color=lightgray) ![alttext](https://img.shields.io/badge/O.S-Windiws%2010-lightgrey&?style=for-the-badge&color=purple)
# In game debug logs
A tool that lets you have access to debug logs in game while playing, even in built version.

#### Customization options
* In game UI, can hide / Unhide with a UI button or keypress.
* Output log messages to a file in your applications persistant data path or a path of your choise.

#### How to use?
1. Import this package by downloading it and add it with the unity package manager
2. Drag a "InGameLogger.prefab" from the Prefabs folder inside the package into the scene.
3. Use the Menu item "Debug logger" to add the customization options you want.

#### The UI
Use the UI to see Logs in game, can be built and seen in game.
If you want toggle visibilty with a key instead of a button, turn the image off on the "LogButton".
There is also an option for selecting hotkey key - default is 'L'.

click a message to get more information about it.

#### The LoggerOutput
Is used to save logs to an file. Default output path is in the Application.persistentDataPath + Logger/output.txt
