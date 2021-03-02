# Geometry Dash Rich Presence (next - GDRPC) (BETA)
GDRPC allow you to show additional information about the game on Discord. For example, what level are you playing. GDRPC is distributed under the MIT licenses. You can use in your launcher, we will find the game automatic. 
You only call one function.

## Download for original game
Go to [releases](https://github.com/hopixteam/gdrpc/releases)

## Example (for private servers)
Recommended: use a LDR service (you just need to compile GDRPC.dll and add the files (download them from release): `libcurl.dll`,` gdrpc_cli.dll`, `gdrpc_ldr.dll` and your builded `GDRPC.dll` to game)

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
### Stop GDRPC with Discord (app not automatic exit)
#### if you start with thread
```c#
rpc.Abort();
GDRPC.AppRunner.Stop();
```
#### if you start with task or only function
```c#
GDRPC.AppRunner.Stop();
```

## Config file (need to compiling GDRPC)
### `src/resources/config.ini`
appID - your discord app id.
processName - game process name. Default: GeometryDash
ext=.exe - ext file. (for windows). leave it as it is.

## Thanks
[@Partur](https://github.com/partur1) - help with GDRPC loader (ldr) 
