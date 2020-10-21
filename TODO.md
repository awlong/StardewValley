# TODO List 

This will be continuously added to, I should really use github projects vs this but :man_shrugging:.

## Upcoming

- [ ] Add overlays beyond mouse
    - [X] Tile grid
    - [X] Draw on Tile
    - [X] Draw line between tiles
    - [ ] Draw text inside tile
    - [X] Hitboxes
    - [X] Mouse Coords
    - [ ] Crop Quality Date
    - [ ] Rock Ladder Drop #
    - [X] Walkable tiles
    - [ ] Mixed Seed planting
    - [ ] Sleep details
    - [ ] Info panels
- [ ] Commands via console
    - [ ] Peek the next set of random numbers
    - [ ] List MineContainers
    - [X] Basic filesystem commands
    - [X] Create new SaveState
    - [X] Get current frame
    - [X] Get details on SaveState
    - [X] Advance frame (or multiple)
    - [X] Advance to next second
    - [X] Reset to frame (including FastAdvance)
    - [X] Save/Load input file (including FastAdvance)
    - [X] Quit
    - [X] Toggle IOverlay
    - [X] Toggle IGameLogic
    - [X] List overlays/logic/commands
    - [X] List Forage in named zone (or current zone)
    - [X] List Friendships
    - [X] List Player Details (pos, luck, steps, ene, hp, xp)
    - [X] List TrashCans
- Automation logic
    - [ ] Minimum time path to tile
        - [ ] Code to find the path (copy)
        - [ ] Overlay to draw the path
        - [ ] GameLogic to advance along the path
        - [ ] Commands to find the path and advance either the next stage or all steps
    - [X] Auto swing pickup
        - [X] auto swap to melee weapon
        - [X] on next frame anim cancel
- [ ] DOCUMENTATION
    - [ ] getting started for code build
    - [ ] getting started with the tas interface
    - [X] ingame help (method in place for commands)
        - ENFORCED that every callable/addressable thing by the console has help text

Features to look into:
- Tab Complete/Suggest for the command console
    - I can know if tab was pressed, I just need to prefix scan the current string against available options
    - if multiple hits I can write those as options to console
    - if solo hit then you just replace that token of the entry text with the correct one and advance the cursor


## Completed

- [X] Wrap Update/Draw calls with basic controller logic
    - [X] sync draws and calls
    - [X] force async code to run sync'd
- [X] Change game to use alternate constants for save location/etc
- [X] Wrap the input state
- [X] Add generic Reflector class to maintain references to System.Reflection info/streamline metaprogramming
- [X] Change Draw to write to a RenderTarget2D instead of implicit target (maintain screen when frozen)
- [X] Handle keypress to advance 1 frame
- [X] Store input state on frame advance
    - [X] Basic FrameState/StateList classes
    - [X] Push to/pull from StateList on frame advance
    - [X] Read/write StateList to file
    - [X] Store active logic toggles
    - [X] Store language code
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
- [X] Get fast restart working
    - [X] Override SpriteBatch.cs
    - [X] Loop Update/Draw up to some frame threshold from finished
- [X] Add basic state detection code
    - wrap only the info I need from the global/current game objects
- [X] Add basic updating logic
    - [X] auto animation cancel
    - [X] auto screen fade advance
    - [X] auto skip event advance
    - [X] auto advance when player can't move
    - [X] auto sleep (handling dialogue box/animations)
    - [X] auto dialogue box (non-select)
    - [X] auto save game advance
- [X] Reimplement command console
    - [X] toggle key to bring down window
    - [X] take keyboard input to write text
    - [X] simple scrolling/history and inline ctrl+left/right to move between tokens
    - [X] bundled monospace font as an xnb
    - [X] run command based on input text
    - [X] write to log/entry from outside function
    - [X] handle subscribing to current console input (sub/unsub)
    - [X] function where code can open/close the console

## Things to consider later

Space size of input files isn't small... :man_shrugging:

- json output gets somewhat large compared to old custom serialization 
    - current: ~1MB/4500f or 48MB/hr of playback currently with just inputs
    - previously: 130MB/hr but I stored a complete reconstruction of the RNG seed (~570 bytes/frame, basically 120MB/hr)
    - not sure this actually matters right now, the current method is super legible (could store RNG and be at <200MB/hr which seems fine).


NOTE: To create a working xnb font on PC, I needed a Mac. I used the monogame pipeline creator to bundle a ttf spritefont into an xnb, then unpacked and converted the Monogame.Framwork links to the correct Microsoft.XNA libs that corresponded.