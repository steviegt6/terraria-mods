using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
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

        // Since our assets are purely visual, they are still restricted by the
        // existing item hitbox parameters.  To fix this, we calculate the
        // necessary offset and adjust rendering that way.  These textures don't
        // exist on the server and we don't provide syncing guarantees, so we
        // should not and will not modify the actual hitbox values.
        // Origin is in the top left corner, so only handle cases where the
        // sprite is shorter than vanilla's for now.
        // TODO: Will we need to support cases where it's larger?
        // TODO: Handle cases for framed animations when the time comes.

        Main.instance.LoadItem(item.type);
        var vanillaTexture = TextureAssets.Item[item.type];
        var difference     = vanillaTexture.Height() - texture.Height;

        var offset = new Vector2(0, difference > 0 ? difference : 0);
        spriteBatch.Draw(texture, item.position - Main.screenPosition + offset, null, lightColor, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);

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