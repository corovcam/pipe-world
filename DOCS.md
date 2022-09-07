# Documentation

## Unity Project Description
### Folders:
- **Assets** - scripts, pictures, sprites, audio, animations and user-made files
- **Packages** - ontains information about required packages to install and build the Unity project
- **ProjectSettings, UserSettings** - contains Project settings and custom Unity Editor preferences

### **Assets**
- **Animations** (Controllers have just a default implementation)
    - PointerAnimation - red-invisible Pointer animation that points at the current active tile
    - SkipAnimation - Skip button animation to make it flicker
    - Start Flow Animation - Flow button animation to make it flicker
- **Prefabs** (Instantiated and configured in scripts)
    - BackTile Prefab - Background tile used in Grid for the individual Pipes
    - Pointer Prefab - Instantiated on the grid as pointing/marking tool
    - **AudioSources** - Audio Source Prefabs with Audio Clips used for button clicking, hovering, pipe rotation and End Game win sound respectively
    - **Buttons** - Level Select button Prefabs used in pagination for Next Page and Previous Page buttons
    - **Pipes** - Default Pipe Prefabs with default green sprites with pre-set *tileType* and *IODirs* for respective Pipe types (except for EMPTY Pipe)
- **Puzzle stage & settings GUI Pack**[^1] - A Unity Asset Store GUI Pack used for labels, buttons, Pause and End Game menus
- **Resources** (Uses custom Unity API for access)
    - LevelX - Custom level data accessible from Level Select menu, can easily be added new level
    - **Sounds** - Audio Clips[^4] used in *AudioSources* Prefabs
    - **UI** - Additional Sprite and Font data used in-game
- **Scenes**
    - *Game* - Main Game scene where all the mechanics and physics occur, either chosen *LevelX* from Level Select menu or Arcade mode
    - *LevelSelect* - All levels included in Resources are automatically added to the Level Select menu in a paginated view
    - *MainMenu* - First scene in Build with 2 buttons
- **Scripts** (Only MonoBehaviour scripts attached to GameObjects)
    - *Extensions.cs* - Used only to extend RectTransform methods by adding a amount of screen units to left/right/top/bottom offset
    - *GameManager.cs* - Manages core water flowing mechanic after Flow start is triggered
    - *GUIHandler.cs* - Handles *Game* scene GUI Components, End Game Menu, Total Score calculaction and Timer and its mechanism
    - *LevelData.cs* - Static class used to store all information about the current Game and Level
    - *LevelHandler.cs* - Handles Level building, Tile/Pipe management, Restart, Tiles shuffle, rotation and audio
    - *LevelSelectHandler.cs* - Handles Level Select menu GUI components, their construction and management including pagination buttons and page counting
    - *MenuHandler.cs* - Handles Main Menu GUI components construction and management
    - *PauseControl.cs* - Handles Pause mechanic in the game as well as Pause menu and buttons
    - *PipeHandler.cs* - Handles individual information about a Pipe (location, rotation) and its surrounding Pipes
    - *Player.cs* - Player Input interaction with the game. Game Controls.
    - *PuzzleGenerator.cs* - Static class used to generate a maze puzzle consisting of cells that are surrounded by walls in a 2D Array made by CellWalls struct
    - *SceneHandler.cs* - Used to handle the load of new Scenes in the game and to configure the static default values
- **Settings** - Universal Render Pipeline default settings (not used extensively in the project)
- **Sprites**
    - Square.png - Used for Marker animation
    - **BackTiles** - BackTiles[^2] used in Grid, that are chosen at random when the game starts
    - **PipeTiles** - Green Pipes and Red Pipes[^3] as well as their water- variants
- **TextMesh Pro** - A Unity plugin used in all TextMeshPro components
- **UI Toolkit** - Not used in the project, just the default UI settings

***

## Scenes
### **Scene: LevelSelect**
Scene contains a Title and a Grid of Levels in a paginated view.  

- **Main Camera** - A *LevelSelectHandler.cs* is attached to the Camera.
    - Inspector view and *LevelSelectHandler.cs* settings:
        - *Previous Page Btn*: Reference to the *Previous Page Btn* Prefab in *</Assets/Prefabs/Buttons>*
        - *Next Page Btn*: Reference to the *Next Page Btn* Prefab in *</Assets/Prefabs/Buttons>*
        - *Level Btn Background* - Reference to the *candy bar* Sprite in *</Assets/Puzzle stage & settings GUI Pack/Image_green>*
        - *Numbers* - Reference to the respective number Sprite in *</Assets/Puzzle stage & settings GUI Pack/Image_green/text>*
        - *Levels Count*: When the number of Levels in Resources changes, this number should be changed to reflect the current number of Levels.
- **Audio Click Source** - Audio Source used in scripts when a button is clicked
- **Audio Enter Source** - Audio Source used in scripts when a mouse hovers over a button
- **Level Select Canvas** - The Canvas is rendered upon the Main Camera (referenced in Inspector)
    - *Canvas Scaler* - Used for Responsive design that scales the width and height (***Screen Match Mode***) equally (***Match*** = 0.5) using the 1920x1080 resolution as a reference point
    - **Page #** - Represents a collection of at most 8 *Level Numbers* all organized using the *Grid Layout Group*
        - *Grid Layout Group* - Childs are aligned along the middle center line using fixed *Cell Size* and *Spacing* values
        - **# (Buttons)** - Are all Interactable button parents
            - **Number#** - Contains *Horizontal Layout Group* component to organize multi-digit Numbers one by one; Also has custom padding
    - **Previous Page Btn(Clone)**, **Next Page Btn(Clone)** - An instantiated prefab that represents a cloned button that changes ***Interactable*** property when the *currentPage* is either the *Last Page* or *First Page*

### **Scene: Game**
Game scene is the main scene in the build.  

- **Grid** - A *LevelHandler.cs* script is attached to Grid GameObject.
    - Inspector view and *LevelHandler.cs* settings:
        - *Pipe Prefabs* - Referenced from the *</Assets/Prefabs/Pipes>* directory; The last one is always EMPTY
        - *Pipe Sprites*, *Filled Pipe Sprites*, *Red Pipe Sprites*, *Filled Red Pipe Sprites* - all respectively referenced using the spritesheets in *<Assets/Sprites/PipeTiles>*
        - *Tile Pointer* - Referenced ***Pointer*** Scene GameObject
        - *Back Tile Prefab* - Referenced from the *</Assets/Prefabs>* directory
        - *Back Tile Sprites* - Chosen at random at the beginning of the game
        - *Active Pipe*, *Active Pipe Handler* (private fields) - reference to the currently selected Active Pipe and its PipeHandler script instance
        - *Board Size*, *Level Num*, *Arcade Mode* (private fields) - for Debug purposes shown in the Inspector
        - *Pause Control* - Referenced from the ***Manager*** scene GameObject
    - **Pointer** - GameObject
    - **(X, Y)** - Tile GameObjects rendered as *BackTile* Sprites at [X, Y] coordinates in the Grid
        - **Pipe(Clone)** - Instantiated *Pipe Prefab* (with default values mentioned in the ***Prefabs/Pipes*** section) with a *PipeHandler.cs* script instance attached to it
- **GUI** - Scales with resolution, same as ***Level Select Canvas***
    - Contains ***Pause Button***, ***Skip Button***, ***Timer Panel*** and ***Start Flow Button*** GameObjects all anchored to their respective corners
- **Pause Menu** - Scales with resolution, same as ***Level Select Canvas***; Can be enabled in-game using *ESC* key or the ***Pause Button***
    - **Menu Background** - The white-green *box* panel Sprite
        - **Panel** - Used for *Vertical Layout Group* child layout positioning
            - **Sound Control** - *Horizontal Layout Group* for ***Sound*** Image and ***Toggle*** Interactable
                - **Toggle** - Default property value ***IsOn*** is true
                    - Toggle mechanism is handled inside *PauseControl.cs* script instance of the ***Manager*** GameObject
            - **Help Button** - Displays the ***Help Dialog*** (enables the GameObject), rendered over the ***Pause Menu***
            - **Restart Button**, **Quit Button**, **Resume Button** - all referenced in ***Manager*** GameObject and managed using *PauseControl.cs* and *GUIHandler.cs* scripts
    - **Help Dialog** - Contains ***Cancel Help Dialog Button*** to set the ***Help Dialog*** as Not Enabled
- **End Game Menu** - Scales with resolution, same as ***Level Select Canvas*** and ***Pause Menu***; Enabled when the ***Countdown*** runs to 0, or when the ***Flow Button*** is pressed and the flow runs to the *End Pipe* (Won game) or the Flow is stuck and can't reach the *End Pipe* (Lost game)
    - Similar hierarchy as ***Pause Menu***
    - **Menu Background** - Anchored to the center to scale with bigger/smaller screens in the middle
        - **Panel** - *Vertical Layout Group*
            - **Score Number** - Calculated and handled using the *GUIHandler.cs* script in the ***Manager** GameObject
- **Manager** - An auxiliary (invisible) GameObject holding and managing Script instances
    - **Game Manager** - *GameManager.cs*
    - **Player** - *Player.cs*
    - **Pause Control** - *PauseControl.cs*
    - **GUI Handler** - *GUIHandler.cs*
        - *Is Debug* (private field) - For debugging purposes, look inside code
        - *Default Time Limit* (private field) - For debugging purposes, mainly to increse/decrease Arcade Mode Time Limit
- **Audio Click Source**, **Audio Enter Source**, **Audio Rotate Source**, **Audio Winning Source** - Same as in ***LevelSelect*** scene, referenced with their respective *Audio Clips*
    - ***Audio Winning Source*** plays only when the player *Won* game, not when the player *Lost* game

***

## Build, install and run
Game is built using the Unity Ediotr, version 2021.3.5f1, so the assemblies and scripts are mostly compatible with the 2021.3 major versions. If run built using a version lower than this one, compatibility issues may arise considering that the game uses newer Universal Render Pipeline, Input system and Coroutines.  

When cloned, start the Editor using the Unity Hub (or manually) and wait till the Editor finishes downloading and importing packages.

The game is mainly configured for the desktop Windows and WebGl platforms. The intended resolution aspect ratio is 16:9 (developed using the 1920x1080 resolution as a reference endpoint) and played in the landscape mode. Though the game can be run on mobile device browsers, the settings need to be configured properly for the game to run smoothly.

The WebGl build can be found here: <https://play.unity.com/mg/other/plumber-7>  
Or using: <https://developer.cloud.unity3d.com/share/share.html?shareId=-JHuPBavt_>

***

## Credits and 3rd Party Assets
[^1]: <https://assetstore.unity.com/packages/2d/gui/puzzle-stage-settings-gui-pack-147389> "Puzzle stage & settings GUI Pack"
[^2]: <https://opengameart.org/content/puzzle-pack-2-795-assets> "Puzzle Pack 2, made by Kenney.nl"
[^3]: <https://opengameart.org/content/2d-pipe-parts> "2D Pipe parts, made by TwistedDonkey in Blender, further costumized by me"
[^4]: <https://opengameart.org/content/51-ui-sound-effects-buttons-switches-and-clicks> "51 UI sound effects (buttons, switches and clicks), made by Kenney.nl"