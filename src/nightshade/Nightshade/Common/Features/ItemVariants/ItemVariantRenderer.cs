using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Nightshade.Common.Features.ItemVariants;

/// <summary>
///     Responsible for rendering and handling item variants in-game.
/// </summary>
[Autoload(Side = ModSide.Client)]
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class ItemVariantRenderer : GlobalItem
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

    public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        if (npcId == 0 && variantId == 0)
        {
            return true;
        }

        if (!ItemVariantLoader.TryGetTextureForVariant(item.type, npcId, variantId, out var texturePath))
        {
            return true;
        }

        var texture = ModContent.Request<Texture2D>(texturePath).Value;
        spriteBatch.Draw(texture, item.position - Main.screenPosition, null, lightColor, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);

        return false;
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
        var otherRenderer = destination.GetGlobalItem<ItemVariantRenderer>();
        if (otherRenderer.npcId == 0 && otherRenderer.variantId == 0)
        {
            npcId     = 0;
            variantId = 0;
        }

        return base.CanStackInWorld(destination, source);
    }
}