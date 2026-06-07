# Addressables

Addressables helpers are compiled when Unity detects `com.unity.addressables` and defines `ADDRESSABLES_2` for the package assembly.

Runtime files:

- `Runtime/Addressables/AssetReferenceScene.cs`
- `Runtime/Addressables/AddressablesSafeGuidExtensions.cs`
- `Editor/Addressables/AssetReferenceSceneDrawer.cs`

## AssetReferenceScene

`AssetReferenceScene` stores a scene reference as a `SafeGuid` and loads the scene through Addressables.

Key members:

- `_sceneGuid`: serialized Unity asset GUID for the scene.
- `LoadSceneAsync(LoadSceneMode mode = LoadSceneMode.Additive)`: loads the referenced scene and returns the Addressables `SceneInstance`.

Example:

```csharp
using SaltboxGames.Unity;
using UnityEngine.SceneManagement;

SceneInstance scene = await sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
```

## SafeGuidPassthroughLocator

`SafeGuidPassthroughLocator` lets Addressables look up resources by `SafeGuid` keys by converting the key to the exact string address format.

Register this locator with Addressables when project code needs to resolve assets directly from `SafeGuid` values.
