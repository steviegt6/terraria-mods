using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nightshade.Common.Hooks.ItemRendering;
using Nightshade.Content.Projectiles;

using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items;

internal sealed class StarFragmentItem : ModItem
{
    private sealed class StarFragmentDrawer : GlobalItem, IModifyItemDrawBasics
    {
        private static readonly string[] textures =
        {
            Assets.Images.Items.Misc.StarFragment_1.KEY,
            Assets.Images.Items.Misc.StarFragment_2.KEY,
            Assets.Images.Items.Misc.StarFragment_3.KEY,
            Assets.Images.Items.Misc.StarFragment_4.KEY,
            Assets.Images.Items.Misc.StarFragment_5.KEY,
        };

        void IModifyItemDrawBasics.ModifyItemDrawBasics(Item item, int slot, ref Texture2D texture, ref Rectangle frame, ref Rectangle glowmaskFrame)
        {
            if (item.ModItem is not StarFragmentItem starFragmentItem)
            {
                return;
            }

            if (starFragmentItem.index < 0)
            {
                return;
            }

            var texturePath = textures[starFragmentItem.index - 1];
            texture = ModContent.Request<Texture2D>(texturePath).Value;
            frame   = glowmaskFrame = texture.Frame();
        }
    }

    public override string Texture => Assets.Images.Items.Misc.StarFragment_1.KEY;

    private int index;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        ItemID.Sets.ItemIconPulse[Type] = true;
        ItemID.Sets.ItemNoGravity[Type] = true;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (12, 20);

        Item.maxStack = 99;

        Item.SetShopValues(
            ItemRarityColor.Blue1,
            Item.sellPrice(silver: 1)
        );
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        if (source is not EntitySource_Parent parent)
        {
            return;
        }

        if (parent.Entity is not Projectile projectile)
        {
            return;
        }

        if (projectile.ModProjectile is not StarFragmentProj starFragmentProj)
        {
            return;
        }

        index = starFragmentProj.Index;
    }

    public override Color? GetAlpha(Color lightColor)
    {
        return Color.White;
    }

    public override void Update(ref float gravity, ref float maxFallSpeed)
    {
        base.Update(ref gravity, ref maxFallSpeed);

        if (!Main.dayTime || Main.remixWorld || Item.shimmered || Item.beingGrabbed)
        {
            return;
        }

        for (var i = 0; i < 10; i++)
        {
            Dust.NewDust(
                Item.position,
                Item.width,
                Item.height,
                DustID.MagicMirror,
                Item.velocity.X,
                Item.velocity.Y,
                150,
                default(Color),
                1.2f
            );
        }

        for (var i = 0; i < 3; i++)
        {
            Gore.NewGore(
                Item.position,
                new Vector2(Item.velocity.X, Item.velocity.Y),
                Main.rand.Next(16, 18)
            );
        }

        /*Item.active = false;
        Item.type   = ItemID.None;
        Item.stack  = 0;*/
        Item.TurnToAir();

        if (Main.netMode == NetmodeID.Server)
        {
            NetMessage.SendData(MessageID.SyncItem, -1, -1, null, Item.whoAmI);
        }
    }
}