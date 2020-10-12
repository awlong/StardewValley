# TODO List 

This will be continuously added to, I should have probably used github projects vs this.

## Upcoming

- [ ] Add overlays beyond mouse
    - [ ] Tile grid
    - [ ] Draw on Tile
    - [ ] Draw line between tiles
    - [ ] Draw text inside tile
    - [ ] Hitboxes
    - [ ] Mouse Coords
    - [ ] Crop Quality Date
    - [ ] Rock Ladder Drop #
    - [ ] Walkable tiles
    - [ ] Mixed Seed planting
    - [ ] Sleep details
    - [ ] Info panels
- [ ] Reimplement command console
    - [ ] toggle key to bring down window
    - [ ] take keyboard input to write text
    - [ ] run command based on input text
    - [ ] function where code can launch the console/write to log

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

## Things to consider later

Space size of input files isn't small... :man_shrugging:

- json output gets somewhat large compared to old custom serialization 
    - current: ~1MB/4500f or 48MB/hr of playback currently with just inputs
    - previously: 130MB/hr but I stored a complete reconstruction of the RNG seed (~570 bytes/frame, basically 120MB/hr)
    - not sure this actually matters right now, the current method is super legible (could store RNG and be at <200MB/hr which seems fine).