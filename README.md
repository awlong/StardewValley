# StardewValley - TAS Engine

Tool Assisted Speedrun (TAS) Engine built ontop of veywrn's [Decompiled Stardew Valley](https://github.com/veywrn/StardewValley) repo. I've built multiple variations of this code on OSX, this is an attempt to move over to Windows.

[Current TODO List](TODO.md)

## Building
To build the repository:

1.  Purchase and install [Stardew Valley](https://www.stardewvalley.net/)

2.  Copy the following files from the Stardew Valley install to .\lib\:
    - BmFont.dll
    - CSteamworks.dll
    - Galaxy.dll
    - GalaxyCSharp.dll
    - GalaxyCSharpGlue.dll
    - GalaxyPeer.dll
    - Lidgren.Network.dll
    - Netcode.dll
    - steam_api.dll
    - Steamworks.NET.dll
    - xTile.dll

3.  Download and install [Microsoft XNA Redistributable 4.0](https://www.microsoft.com/en-us/download/details.aspx?id=27598)

4.  Copy the following files from the XNA install to .\lib\:
    - Microsoft.Xna.Framework.dll
    - Microsoft.Xna.Framework.Game.dll
    - Microsoft.Xna.Framework.Graphics.dll
    - Microsoft.Xna.Framework.Xact.dll

    For me, ran the following command in terminal:

`cp C:\Windows\Microsoft.NET\assembly\GAC_32\Microsoft.Xna.Framework.*\*\*.dll .\lib\`

5.  Build (I had best luck with Community version of Visual Studio 2019)

## Running
Copy the /Content/ directory from the Stardew Valley install to the build output 
location.
