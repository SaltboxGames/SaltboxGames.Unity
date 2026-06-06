# Editor Utilities

Editor utility helpers live under `SaltboxGames.Unity.Editor.Utilities`.

Editor files:

- `Editor/Utilities/EditorReflection.cs`
- `Editor/Utilities/ReadOnlyPropertyDrawer.cs`
- `Editor/Utilities/SerializedPropertyExtensions.cs`

## EditorReflection

`EditorReflection` binds Unity editor internals used by custom drawers.

Key members:

- `GetActiveFolderPath()`: returns the active Project window folder path, falling back to `Assets`.
- `GetFieldInfoFromProperty(SerializedProperty property)`: returns reflected backing field information for a serialized property when available.

## SerializedPropertyExtensions

`SerializedPropertyExtensions` contains helpers for editor code that works with custom serialized values.

Key members:

- `GetFieldInfo(this SerializedProperty property)`: returns the backing field info.
- `GetGuidValue(this SerializedProperty p)`: reconstructs a `SafeGuid` from serialized integer segments.
- `SetGuidValue(this SerializedProperty p, SafeGuid guid)`: writes a `SafeGuid` into serialized integer segments.

## ReadOnlyFieldDrawer

`ReadOnlyFieldDrawer` displays fields marked with `ReadOnlyFieldAttribute` in a disabled state while preserving normal child-property rendering.
