using System.Collections.Generic;

using Newtonsoft.Json;

namespace Tomat.Terraria.TML.SourceGenerator.Generators.DataDriven;

public sealed class EffectFile
{
    [JsonProperty("samplers")]
    public Dictionary<string, string> Samplers { get; set; } = [];

    [JsonProperty("uniforms")]
    public Dictionary<string, string> Uniforms { get; set; } = [];

    [JsonProperty("passes")]
    public Dictionary<string, Dictionary<string, string>> Passes { get; set; } = [];

    [JsonProperty("profile")]
    public string Profile { get; set; } = "";
}