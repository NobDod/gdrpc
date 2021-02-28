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
