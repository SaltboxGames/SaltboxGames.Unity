# Type References

Type reference helpers live under `SaltboxGames.Unity`.

Runtime files:

- `Runtime/TypeReference.cs`
- `Editor/TypeReferenceDrawer.cs`

## TypeReferenceBase

`TypeReferenceBase` stores an assembly-qualified type name and restores it to a runtime `Type` during Unity deserialization.

Key members:

- `ReferenceType`: resolved runtime type.
- `OnBeforeSerialize()`: writes the current type name.
- `OnAfterDeserialize()`: restores the type from the serialized name.

## TypeReference<T>

`TypeReference<T>` restricts the stored type to values assignable to `T`.

The editor drawer presents a popup of concrete, non-abstract types assignable to the requested base type.
