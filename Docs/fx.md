# FX

FX helpers live under `SaltboxGames.Unity.FX`.

The camera effects are compiled when Unity detects `com.unity.cinemachine` and defines `CINEMACHINE` for the package assembly.

Runtime files:

- `Runtime/FX/CameraKickEffect.cs`
- `Runtime/FX/CameraShakeEffect.cs`
- `Runtime/FX/FreezeEffect.cs`

## CameraKickEffect

`CameraKickEffect` is a Cinemachine extension that applies a decaying positional offset.

Key members:

- `Kick(Vector3 direction, float strength, float duration)`: applies a scaled-time kick.
- `KickUnscaled(Vector3 direction, float strength, float duration)`: applies an unscaled-time kick.
- `SetDecayCurve(AnimationCurve curve)`: changes the kick falloff curve.

## CameraShakeEffect

`CameraShakeEffect` is a Cinemachine extension that applies random positional shake.

Key members:

- `Shake(float magnitude, float duration)`: starts or refreshes scaled-time shake.
- `ShakeUnscaled(float magnitude, float duration)`: starts or refreshes unscaled-time shake.
- `ShakeAdditive(float magnitude, float duration)`: adds scaled-time shake magnitude.
- `ShakeAdditiveUnscaled(float magnitude, float duration)`: adds unscaled-time shake magnitude.

## FreezeEffect

`FreezeEffect.Freeze(float duration)` temporarily sets `Time.timeScale` to zero for a real-time duration.
