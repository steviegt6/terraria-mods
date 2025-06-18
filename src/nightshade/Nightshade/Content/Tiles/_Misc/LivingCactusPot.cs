using System;

using Microsoft.Xna.Framework;

using Nightshade.Common.Features;
using Nightshade.Content.Gores;
using Nightshade.Content.Items;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Nightshade.Content.Tiles;

internal sealed class LivingCactusPot : AbstractPot
{
    private sealed class LivingCactusPotBehavior : PotBehavior
    {
        public override void SpawnGore(PotBreakContext ctx)
        {
            var goreAmt = Main.rand.Next(1, 2 + 1);

            for (var i = 0; i < goreAmt; i++)
            for (var type = 1; type < 3; type++)
            {
                Gore.NewGore(
                    new EntitySource_TileBreak(ctx.X, ctx.Y),
                    new Vector2(ctx.X, ctx.Y) * 16,
                    Main.rand.NextVector2CircularEdge(3f, 3f),
                    ModContent.GetInstance<ModImpl>().Find<ModGore>($"LivingCactusPotGore{type}").Type
                );
            }
        }

        public override float GetInitialCoinMult(PotLootContext ctx)
        {
            return PotLootImpl.POT_BEHAVIOR_VANILLA.GetInitialCoinMult(
                ctx with { Style = (int)VanillaPotStyle.UndergroundDesert34 }
            );
        }

        public override void SpawnTorches(PotLootContextWithCoinMult ctx, Player player)
        {
            // base.SpawnTorches(ctx, player);

            PotLootImpl.POT_BEHAVIOR_VANILLA.SpawnTorches(
                ctx with { Style = (int)VanillaPotStyle.UndergroundDesert34 },
                player
            );
        }

        protected override void ModifyTorchType(
            PotLootContextWithCoinMult ctx,
            Player player,
            ref int torchType,
            ref int glowstickType,
            ref int itemStack
        )
        {
            throw new InvalidOperationException();
        }

        protected override bool TryGetUtilityItem(
            PotLootContextWithCoinMult ctx,
            out int utilityType,
            out int utilityStack
        )
        {
            utilityType = ModContent.ItemType<CactusSplashJug>();
            utilityStack = Main.rand.Next(5, 10);
            return true;
        }
    }

    public override string Texture => Assets.Images.Tiles.Misc.LivingCactusPot.KEY;

    public override PotBehavior Behavior { get; } = new LivingCactusPotBehavior();

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        AddMapEntry(new Color(47, 79, 79), Language.GetText("MapObject.Pot")); // dark slate gray
        DustType = 29;
    }
}

internal sealed class LivingCactusPotPlacer : ModItem
{
    public override string Texture => Assets.Images.Items.Misc.LivingPalmWoodBlock.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<LivingCactusPot>());

        Item.value = 100;
        Item.rare = ItemRarityID.Green;
    }
}

internal sealed class PotGoreLoader : ILoadable
{
    public void Load(Mod mod)
    {
        mod.AddContent(new GenericGore("LivingCactusPotGore1", Assets.Images.Gores.Misc.CactusPotGore1.KEY));
        mod.AddContent(new GenericGore("LivingCactusPotGore2", Assets.Images.Gores.Misc.CactusPotGore2.KEY));
    }

    public void Unload() { }
}