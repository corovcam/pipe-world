---
layout: default
---

## [TutorialPlayer.cs](https://github.com/corovcam/pipe-world/blob/main/Assets/Scripts/TutorialPlayer.cs)

MonoBehaviour script, `TutorialPlayer`, is responsible for playing video tutorials before the game. It obtains the video file URL from the `StreamingAssets` folder (determined using the `LevelData.IsArcadeMode` and `LevelData.IsFreeWorldMode` static properties) and plays it using the `VideoPlayer` component.

The `SkipTutorial` public function handles the skip button click event. It is called by the `Skip Tutorial` button in the `Tutorial` scene.
