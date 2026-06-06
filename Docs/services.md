# Services

Unity service helpers live under `SaltboxGames.Unity.Services` and adapt `SaltboxGames.Core.Services` to Unity components and the Unity player loop.

Runtime files:

- `Runtime/Services/MonoService.cs`
- `Runtime/Services/MonoServiceScope.cs`
- `Runtime/Services/PlayerLoopService.cs`

## MonoService

`MonoService` is a `MonoBehaviour` base class that implements `IService` with no-op lifecycle defaults.

Override only the lifecycle methods the component needs:

- `InitializeAsync(IServiceInitializer services)`
- `StartService()`
- `StopService()`
- `ShutdownAsync()`

## MonoServiceScope

`MonoServiceScope` wraps a Core `ServiceScope` in a Unity component. It gathers child `MonoService` components, sorts them by `DefaultExecutionOrder`, registers them, and coordinates their lifecycle.

Use it when scene or prefab-owned services should participate in the same service lifecycle model as plain C# services.

## PlayerLoopService

`PlayerLoopService` exposes Unity player-loop phases through disposable subscriptions.

Key members:

- `SubscribeEarlyUpdate(Action callback)`
- `SubscribeUpdate(Action callback)`
- `SubscribeLateUpdate(Action callback)`

Dispose a subscription to stop receiving callbacks.
