using System.Collections.Generic;

namespace Build.Pre.Features.Assets;

internal sealed record PathNode(string Name, Dictionary<string, PathNode> Nodes, List<AssetFile> Files);