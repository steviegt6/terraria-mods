using Daybreak.Common.Features.Hooks;

using Microsoft.Xna.Framework;

using Nightshade.Common.Features;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items._Cursors;

internal abstract class HairDyeCursor(int hairDye) : ModItem
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        VanityCursorSets.IsVanityCursor[Type] = true;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.CloneDefaults(hairDye);

        Item.value *= 5;
        Item.consumable = false;

        Item.vanity = true;
        Item.hasVanityEffects = true;

        Item.maxStack = 1;
    }

    [OnLoad]
    private static void UpdateVisibleAccessories()
    {
        On_Player.UpdateVisibleAccessory += (orig, self, slot, item, modded) =>
        {
            orig(self, slot, item, modded);

            if (item.ModItem is HairDyeCursor hairDyeCursor)
            {
                self.GetModPlayer<VanityCursorPlayer>().HairDye = hairDyeCursor.Item.hairDye;
            }
        };
    }
}

internal sealed class LifeCursor() : HairDyeCursor(ItemID.LifeHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.LifeCursor.KEY;
}

internal sealed class ManaCursor() : HairDyeCursor(ItemID.ManaHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.ManaCursor.KEY;
}

internal sealed class DepthCursor() : HairDyeCursor(ItemID.DepthHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.DepthCursor.KEY;
}

internal sealed class MoneyCursor() : HairDyeCursor(ItemID.MoneyHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.MoneyCursor.KEY;
}

internal sealed class TimeCursor() : HairDyeCursor(ItemID.TimeHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.TimeCursor.KEY;
}

internal sealed class BiomeCursor() : HairDyeCursor(ItemID.BiomeHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.BiomeCursor.KEY;
}

internal sealed class PartyCursor() : HairDyeCursor(ItemID.PartyHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.PartyCursor.KEY;

    [OnLoad]
    private static void LoadHooks()
    {
        On_Player.UpdateVisibleAccessory += (orig, self, slot, item, modded) =>
        {
            orig(self, slot, item, modded);

            if (Main.netMode == NetmodeID.Server || Main.gamePaused || /*self.dead ||*/ self.ghost /*|| self.stoned || self.frozen*/)
            {
                return;
            }

            if (item.ModItem is PartyCursor)
            {
                SpawnDust(self);
            }

            return;

            // UpdateHairDyeDust
            static void SpawnDust(Player player)
            {
                if (Main.rand.NextBool(45))
                {
                    var type = Main.rand.Next(139, 143);
                    var num = Dust.NewDust(Main.MouseWorld, player.width, 8, type, 0f, 0f, 0, default(Color), 1.2f);
                    Main.dust[num].velocity.X *= 1f + Main.rand.Next(-50, 51) * 0.01f;
                    Main.dust[num].velocity.Y *= 1f + Main.rand.Next(-50, 51) * 0.01f;
                    Main.dust[num].velocity.X += Main.rand.Next(-50, 51) * 0.01f;
                    Main.dust[num].velocity.Y += Main.rand.Next(-50, 51) * 0.01f;
                    Main.dust[num].velocity.Y -= 1f;
                    Main.dust[num].scale *= 0.7f + Main.rand.Next(-30, 31) * 0.01f;
                    Main.dust[num].velocity += MouseVelocity() * 0.2f;
                }

                if (Main.rand.NextBool(225))
                {
                    var type2 = Main.rand.Next(276, 283);
                    var num2 = Gore.NewGore(new Vector2(Main.MouseWorld.X + Main.rand.Next(player.width), Main.MouseWorld.Y + Main.rand.Next(8)), MouseVelocity(), type2);
                    Main.gore[num2].velocity.X *= 1f + Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[num2].velocity.Y *= 1f + Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[num2].scale *= 1f + Main.rand.Next(-20, 21) * 0.01f;
                    Main.gore[num2].velocity.X += Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[num2].velocity.Y += Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[num2].velocity.Y -= 1f;
                    Main.gore[num2].velocity += MouseVelocity() * 0.2f;
                }
            }

            static Vector2 MouseVelocity()
            {
                return new Vector2(Main.mouseX - Main.lastMouseX, Main.mouseY - Main.lastMouseY);
            }
        };
    }
}

internal sealed class SpeedCursor() : HairDyeCursor(ItemID.SpeedHairDye)
{
    public override string Texture => Assets.Images.Items.Accessories.Cursors.SpeedCursor.KEY;
}