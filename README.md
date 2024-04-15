# K2UI

This set of controls is used by the K2D2 Kerbal Space Program 2 mod

## Create your mod with K2UI

This set of controls have be mainly made for KSP2 Modding but it should be used with any Unity project that use Ui Toolkit

To use it with your own mod :

1. : create the mod with the @munix SpaceWarp.Template builder : https://github.com/SpaceWarpDev/SpaceWarp.Template

download the `create-project.bat` and run it. It will magically do the main process 

2. : add K2UI as a sub module in the `src/[yourMod].Unity/[yourMod].Unity/Assets/Runtime` folder
3. : the K2UI needs Newtonsoft.Json, it is included in Kerbal Main Game But should be added in your Unity Project

edit : `src/[yourMod].Unity/[yourMod].Unity/Packages/manifest.json`

and add the line in the modules definition
```json
    "com.unity.nuget.newtonsoft-json": "3.2.1"
```

4. Open the Unity Project : `src/yourMod.Unity/yourMod.Unity` 

and checks that everything is ok (no compilation trouble)

-------------
Success, you use the K2D2 controls

## Create Your first Test UI

https://github.com/cfloutier/test_k2ui is the test project I use to test and build this set of controls.

You can use it as a simple example on how the controls are used and linked. 
All control have it's own little test among the differents tabs.

see the wiki : https://github.com/cfloutier/K2UI/wiki for a description of each control


