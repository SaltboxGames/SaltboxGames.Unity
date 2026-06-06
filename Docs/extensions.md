# Extensions

Unity extension helpers live under `SaltboxGames.Unity.Extensions`.

Runtime files:

- `Runtime/Extensions/SceneExtensions.cs`
- `Runtime/Extensions/Vector2Extensions.cs`
- `Runtime/Extensions/Vector3Extensions.cs`

## SceneExtensions

`TryGetRootComponent<T>(this Scene scene, out T component)` searches a scene's root game objects for a component of type `T` without allocating a new list each call.

Example:

```csharp
using SaltboxGames.Unity.Extensions;
using UnityEngine.SceneManagement;

if (scene.TryGetRootComponent(out GameBootstrap bootstrap))
{
    bootstrap.StartGame();
}
```

## Vector Extensions

`Vector2Extensions` and `Vector3Extensions` provide distance checks that avoid square roots.

Key members:

- `SqrDistance(a, b)`: returns squared distance between two vectors.
- `IsDistanceWithin(a, b, maxDistance)`: returns true when two vectors are within the distance threshold.
- `IsDistanceOutside(a, b, maxDistance)`: returns true when two vectors are farther apart than the distance threshold.
