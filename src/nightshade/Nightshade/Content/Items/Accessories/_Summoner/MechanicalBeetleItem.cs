using MonoMod.Cil;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Accessories;

internal sealed class MechanicalBeetleItem : ModItem
{
    private sealed class MechanicalBeetlePlayer : ModPlayer
    {
        public bool IsEnabled { get; set; }

        public override void Load()
        {
            base.Load();

            IL_Projectile.Damage += il =>
            {
                var c = new ILCursor(il);

                c.GotoNext(x => x.MatchLdsfld(typeof(ProjectileID.Sets), nameof(ProjectileID.Sets.SummonTagDamageMultiplier)));
                c.GotoNext(MoveType.Before, x => x.MatchStloc(out _));
                c.EmitLdarg0(); // this
                c.EmitDelegate(
                    (float multiplier, Projectile self) =>
                    {
                        var player = Main.player[self.owner];
                        if (player.GetModPlayer<MechanicalBeetlePlayer>().IsEnabled)
                        {
                            multiplier += 0.2f;
                        }

                        return multiplier;
                    }
                );
            };
        }

        public override void ResetEffects()
        {
            base.ResetEffects();

            IsEnabled = false;
        }
    }

    public override string Texture => Assets.Images.Items.Accessories.MechanicalBeetle.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (42, 38);

        Item.accessory = true;

        Item.rare  = ItemRarityID.Lime;
        Item.value = Item.buyPrice(gold: 15);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.GetModPlayer<MechanicalBeetlePlayer>().IsEnabled = true;

        player.GetKnockback(DamageClass.Summon).Base += 2f;
        player.GetDamage(DamageClass.Summon)         *= 1.2f;
    }

    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
           .AddIngredient(ItemID.AvengerEmblem)
           .AddIngredient(ItemID.HerculesBeetle)
           .AddTile(TileID.TinkerersWorkbench)
           .Register();
    }
}