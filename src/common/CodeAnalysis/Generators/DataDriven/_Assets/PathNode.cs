using System.Collections.Generic;

namespace Tomat.Terraria.TML.SourceGenerator.Generators.DataDriven;

public sealed record PathNode(string Name, Dictionary<string, PathNode> Nodes, List<AssetFile> Files);