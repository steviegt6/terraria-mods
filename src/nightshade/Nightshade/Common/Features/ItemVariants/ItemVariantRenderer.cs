using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nightshade.Common.Hooks.ItemRendering;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Nightshade.Common.Features.ItemVariants;

/// <summary>
///     Responsible for rendering and handling item variants in-game.
/// </summary>
[Autoload(Side = ModSide.Client)]
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class ItemVariantRenderer : GlobalItem, IModifyItemDrawBasics
{
    public override bool InstancePerEntity => true;

    private int npcId;
    private int variantId;

    public override void SetDefaults(Item entity)
    {
        base.SetDefaults(entity);

        npcId     = 0;
        variantId = 0;
    }

    void IModifyItemDrawBasics.ModifyItemDrawBasics(
        Item          item,
        int           slot,
        ref Texture2D texture,
        ref Rectangle frame,
        ref Rectangle glowmaskFrame
    )
    {
        if (npcId == 0 && variantId == 0)
        {
            return;
        }

        if (!ItemVariantLoader.TryGetTextureForVariant(item.type, npcId, variantId, out var texturePath))
        {
            return;
        }

        texture = ModContent.Request<Texture2D>(texturePath).Value;

        if (Main.itemAnimations[item.type] is not null)
        {
            frame = glowmaskFrame = Main.itemAnimations[item.type].GetFrame(texture, Main.itemFrameCounter[slot]);
        }
        else
        {
            frame = glowmaskFrame = texture.Frame();
        }
    }

    public override void OnSpawn(Item item, IEntitySource source)
    {
        base.OnSpawn(item, source);

        // We can also just check for EntitySource_Parent if it's a concern.
        if (source is not EntitySource_Loot lootSource)
        {
            return;
        }

        // TODO: If we want to support not always using the NPC variant, we need
        //       to make GetVariant provide both of these values.
        npcId     = lootSource.Entity is NPC npc ? npc.type : 0;
        variantId = ItemVariantLoader.GetVariant(item.type, npcId);
    }

    public override bool CanStackInWorld(Item destination, Item source)
    {
        // If trying to merge with a dropped item of the same type, allow it to
        // merge when the other item has no variant.
        var otherRenderer = destination.GetGlobalItem<ItemVariantRenderer>();
        if (otherRenderer.npcId == 0 && otherRenderer.variantId == 0)
        {
            npcId     = 0;
            variantId = 0;
        }

        return base.CanStackInWorld(destination, source);
    }

    public override bool OnPickup(Item item, Player player)
    {
        // Clear the variant data on pickup since once in the inventory the data
        // is no longer relevant.  It'd be cool to support preserving this data,
        // but it has weird interactions when stacking such that it overrides
        // incoming variant data and would look weird in-game (unexpected
        // results), so let's not bother.
        npcId     = 0;
        variantId = 0;

        return base.OnPickup(item, player);
    }
}