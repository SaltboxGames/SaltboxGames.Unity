# SaltboxGames.Unity

SaltboxGames.Unity is a source-distributed Unity utility package for Saltbox Games projects.
It contains Unity-specific extensions, editor drawers, Addressables helpers, Cinemachine effects, player-loop helpers, and service lifecycle adapters for `SaltboxGames.Core`.

## Documentation

Start with the package documentation index: [Docs/README.md](./Docs/README.md).

## Unity Installation

`SaltboxGames.Unity` depends on `SaltboxGames.Core`. Install both packages in the same Unity project.

### Git Dependency

Add the package to your Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.saltboxgames.core": "git@github.com:SaltboxGames/SaltboxGames.Core.git",
    "com.saltboxgames.unity": "git@github.com:SaltboxGames/SaltboxGames.Unity.git"
  }
}
```

### Embedded Package

Use a git submodule if you want the package checked into `Packages/`:

```bash
git submodule add git@github.com:SaltboxGames/SaltboxGames.Core.git ./Packages/com.saltboxgames.core
git submodule add git@github.com:SaltboxGames/SaltboxGames.Unity.git ./Packages/com.saltboxgames.unity
```

Unity will detect it as an embedded package.

## Optional Integrations

Unity enables optional integrations automatically when the matching packages are installed:

1. Addressables: defines `ADDRESSABLES_2` for scene reference helpers.
2. Cinemachine: defines `CINEMACHINE` for camera FX helpers.
3. ZLinq: defines `ZLINQ` for optional value-enumerable integrations.

No manual compilation symbols are required in Unity. The package assembly definitions set the symbols through Unity `versionDefines`.

## License

This package is licensed under MPL 2.0. See [LICENSE](./LICENSE).
