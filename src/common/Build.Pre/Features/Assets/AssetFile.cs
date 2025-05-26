using Build.Shared;

namespace Build.Pre.Features.Assets;

internal readonly record struct AssetFile(string Name, string Path, IAssetReference Reference, ProjectFile File);