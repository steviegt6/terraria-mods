using JetBrains.Annotations;

using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Tomat.TML.Mod.Nightshade.Common.Features.AssetReplacement;
using Tomat.TML.Mod.Nightshade.Core.Attributes;

namespace Tomat.TML.Mod.Nightshade.Content.VisualTweaks;

/// <summary>
///     Modifies the rendering of Rainbow Slimes to have a cooler rainbow effect
///     derived from the Queen Slime shader.
/// </summary>
[Autoload(Side = ModSide.Client)]
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class RainbowSlimeShaderTweak : GlobalNPC
{
    private const int rainbow_slime = NPCID.RainbowSlime;

    private const string rainbow_slime_texture = "Assets/Images/NPCs/RainbowSlime";

    [InitializedInLoad]
    private static AssetReplacementHandle<Texture2D>? rainbowSlimeTextureHandle;

    public override void Load()
    {
        base.Load();

        rainbowSlimeTextureHandle = AssetReplacer.Npc(
            rainbow_slime,
            Mod.Assets.Request<Texture2D>(rainbow_slime_texture)
        );
    }

    public override void Unload()
    {
        base.Unload();

        rainbowSlimeTextureHandle?.Dispose();
    }

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == rainbow_slime;
    }
}