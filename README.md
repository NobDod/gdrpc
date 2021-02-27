# Geometry Dash Rich Presence (next - GDRPC)
GDRPC allow you to show additional information about the game on Discord. For example, what level are you playing. GDRPC is distributed under the MIT licenses. You can use in your launcher, we will find the game automatic. 
You only call one function.

## Example
### With thread
```c#
Thread rpc = new Thread(GDRPC.AppRunner.Run);
rpc.Start();
```
### With task
```c#
Task.Run(() => GDRPC.AppRunner.Run());
```
### Only function
```c#
GDRPC.AppRunner.Run();
```

## Config file (need to compiling GDRPC)
### `src/resources/config.ini`
appID - your discord app id.
processName - game process name. Default: GeometryDash
ext=.exe - ext file. (for windows). leave it as it is.

## Help me!
### How to create discord app
1. Go to [Discord Developer](https://discord.com/developers/applications)
2. Find button `New Application` and click to the button
3. Enter name application and save.
4. Go to tab: Rich Presence -> Art Assets
5. Click to the button `add image(s)`
6. Go to folder `icons` and add image`
7. Save.

### How to change config app?
Example for appID
1. Download GDRPC source code.
2. Extract to any folder.
3. Go to src/resources/config.ini
4. Change `appID=` app id. (Example: `appID=YOUR_ID`)
5. Open project (src/GDRPC.sln) (need visual studio 2019 or large)
6. Build debug or release mode.
