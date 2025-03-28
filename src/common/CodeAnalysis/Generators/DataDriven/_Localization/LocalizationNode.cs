using System.Collections.Generic;

namespace Tomat.Terraria.TML.SourceGenerator.Generators.DataDriven;

public sealed record LocalizationNode(string Name, Dictionary<string, LocalizationNode> Nodes, List<(string key, string value)> Keys);