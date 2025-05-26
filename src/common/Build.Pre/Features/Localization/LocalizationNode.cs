using System.Collections.Generic;

namespace Build.Pre.Features.Localization;

public sealed record LocalizationNode(string Name, Dictionary<string, LocalizationNode> Nodes, List<(string key, string value)> Keys);