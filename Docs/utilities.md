# Utilities

Runtime utility helpers live under `SaltboxGames.Unity.Utilities`.

Runtime files:

- `Runtime/Utilities/LogUtil.cs`
- `Runtime/Utilities/PlayerLoopUtil.cs`
- `Runtime/Utilities/ReadOnlyField.cs`

## LogUtil

`LogUtil.Color(string message, Color color)` wraps a message in Unity rich-text color tags in the editor and returns the raw message in player builds.

## PlayerLoopUtil

`PlayerLoopUtil` inserts and removes callbacks from Unity's player loop.

Key members:

- `InsertIntoEarlyUpdate<T>(PlayerLoopSystem.UpdateFunction updateFunction)`
- `RemoveFromEarlyUpdate<T>(PlayerLoopSystem.UpdateFunction updateFunction)`
- `InsertIntoUpdate<T>(PlayerLoopSystem.UpdateFunction updateFunction)`
- `RemoveFromUpdate<T>(PlayerLoopSystem.UpdateFunction updateFunction)`
- `InsertIntoLateUpdate<T>(PlayerLoopSystem.UpdateFunction updateFunction)`
- `RemoveFromLateUpdate<T>(PlayerLoopSystem.UpdateFunction updateFunction)`
- `InsertInto(Type target, PlayerLoopSystem newSystem)`

In the Unity editor, the player loop is reset to Unity's default loop when exiting play mode.

## ReadOnlyFieldAttribute

`ReadOnlyFieldAttribute` marks a serialized field as read-only for the package's custom property drawer.
