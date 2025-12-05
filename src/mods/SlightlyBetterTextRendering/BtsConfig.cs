using System.ComponentModel;
using JetBrains.Annotations;
using Terraria.ModLoader.Config;

namespace SlightlyBetterTextRendering;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public sealed class BtsConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [Header("Header")]
    [DefaultValue(0.1f)]
    [Range(0f, 1f)]
    public float Factor { get; set; }

    [DefaultValue(false)]
    public bool OverrideWhiteToBlack { get; set; }
}
