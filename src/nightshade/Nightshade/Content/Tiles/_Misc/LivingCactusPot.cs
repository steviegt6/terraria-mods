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
    private sealed class PotImpl : CustomPot
    {
        public override void PlayBreakSound(int i, int j, int style) { }

        public override void SpawnGore(int i, int j, int style)
        {
            var goreAmt = Main.rand.Next(1, 2 + 1);

            for (var k = 0; k < goreAmt; k++)
            for (var l = 1; l < 3; l++)
            {
                Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, Main.rand.NextVector2CircularEdge(3f, 3f), ModContent.GetInstance<ModImpl>().Find<ModGore>($"LivingCactusPotGore{l}").Type);
            }
        }

        public override bool ShouldTryForLoot(int i, int j, int style)
        {
            return true;
        }

        public override void ModifyTorchType(int i, int j, int style, Player player, ref int torchType, ref int glowstickType, ref int itemStack)
        {
            PotLootImpl.VANILLA_POT.ModifyTorchType(i, j, VanillaPot.POT_34_UNDERGROUND_DESERT, player, ref torchType, ref glowstickType, ref itemStack);
        }

        public override bool TryGetUtilityItem(int i, int j, int style, bool aboveUnderworldLayer, out int utilityType, out int utilityStack)
        {
            utilityType = ModContent.ItemType<CactusSplashJug>();
            utilityStack = Main.rand.Next(5, 10);
            return true;
        }

        public override void ModifyCoinMultiplier(int i, int j, int style, ref float multiplier)
        {
            PotLootImpl.VANILLA_POT.ModifyCoinMultiplier(i, j, VanillaPot.POT_34_UNDERGROUND_DESERT, ref multiplier);
        }
    }

    public override string Texture => Assets.Images.Tiles.Misc.LivingCactusPot.KEY;

    public override CustomPot Pot { get; } = new PotImpl();

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        AddMapEntry(new Color(47, 79, 79), Language.GetText("MapObject.Pot")); // dark slate gray
        DustType = 29;
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