# StardewValley - TAS Engine

Tool Assisted Speedrun (TAS) Engine built ontop of veywrn's [Decompiled Stardew Valley](https://github.com/veywrn/StardewValley) repo. I've built multiple variations of this code on OSX, this is an attempt to move over to Windows.

## TODO List (List in progress)

- [X] Wrap Update/Draw calls with basic controller logic
    - [X] sync draws and calls
    - [X] force async code to run sync'd
- [X] Change game to use alternate constants for save location/etc
- [X] Wrap the input state
- [X] Add generic Reflector class to maintain references to System.Reflection info/streamline metaprogramming
- [X] Change Draw to write to a RenderTarget2D instead of implicit target (maintain screen when frozen)
- [X] Handle keypress to advance 1 frame
- [ ] Store input state on frame advance
    - [X] Basic FrameState/StateList classes
    - [X] Push to/pull from StateList on frame advance
    - [X] Read/write StateList to file
- [X] Get clean restart working (from program launch)
    - [X] Override Random.cs
    - [X] Override DateTime.cs
    - [X] Keyboard shortcuts
    - [X] Override textbox entry (listens to different keyboard events than keypresses)
- [X] Get dirty restart working (from mid-run state)
    - [X] Create new intermediate SGame to handle XNA services (inherit from Microsoft.Xna.Framework.Game)
    - [X] Decouple Game1 from Microsoft.Xna.Framework.Game
    - [X] Setup static constructor/initializer reset system for things in StardewValley namespace
    - [X] Configure Controller to trigger the reset
- [ ] Get fast restart working
    - [ ] Override SpriteBatch.cs
- [ ] Reimplement command console
    - [ ] toggle key to bring down overlay
    - [ ] take keyboard input to write text
    - [ ] run command based on input text
    - [ ] function where code can launch the console/write to log
- [ ] Add basic state detection code
    - [ ] Player can move
    - [ ] Player using tool
    - [ ] In dialogue box with prompt
- [ ] Add basic updating logic
    - [ ] auto animation cancel
    - [ ] auto screen fade avance
    - [ ] auto skip event advance
    - [ ] auto sleep advance
    - [ ] auto dialogue box (non-select)

More generic things to consider

- json output gets somewhat large compared to old custom serialization 
    - current: ~1MB/4500f or 48MB/hr of playback currently with just inputs
    - previously: 130MB/hr but I stored a complete reconstruction of the RNG seed (~570 bytes/frame, basically 120MB/hr)
    - not sure this actually matters right now, the current method is super legible (could store RNG and be at <200MB/hr which seems fine).

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
