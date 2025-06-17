using System.Collections.Generic;

using MonoMod.Cil;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace Nightshade.Common.Features;

// Nightshade rewrites the way developer armor is handled to:
// - only give one set per bag at max,
//   - fixes tModLoader possibly granting you an additional set on top of the
//     developer set,
//   - means all set drops are distributed uniformly,
// - allow for us to add our own developer armor sets,
// - and allow for players to obtain sets in pre-hardmode
//   - while filtering out Hardmode-only items.

internal sealed class DevArmorImpl : ModSystem
{
    private static readonly List<DevArmorDefinition> dev_armor_sets = [];

    private static readonly DevArmorDefinition[] vanilla_dev_sets =
    [
        new(
            // Red (Redigit)
            DevArmorKind.VanillaDev,
            new DevArmorItem(666),
            new DevArmorItem(667),
            new DevArmorItem(668),
            new DevArmorItem(665, Hardmode: true),
            new DevArmorItem(3287, Hardmode: true)
        ),
        new(
            // Cenx
            DevArmorKind.VanillaDev,
            new DevArmorItem(1554),
            new DevArmorItem(1555),
            new DevArmorItem(1556),
            new DevArmorItem(1586, Hardmode: true)
        ),
        new(
            // Cenx (dress)
            DevArmorKind.VanillaDev,
            new DevArmorItem(1554),
            new DevArmorItem(1587),
            new DevArmorItem(1588),
            new DevArmorItem(1586, Hardmode: true)
        ),
        new(
            // Crowno
            DevArmorKind.VanillaDev,
            new DevArmorItem(1557),
            new DevArmorItem(1558),
            new DevArmorItem(1559),
            new DevArmorItem(1585)
        ),
        new(
            // Wil
            DevArmorKind.VanillaDev,
            new DevArmorItem(1560),
            new DevArmorItem(1561),
            new DevArmorItem(1562),
            new DevArmorItem(1584, Hardmode: true)
        ),
        new(
            // Jim
            DevArmorKind.VanillaDev,
            new DevArmorItem(1563),
            new DevArmorItem(1564),
            new DevArmorItem(1565),
            new DevArmorItem(3582, Hardmode: true)
        ),
        new(
            // Aaron
            DevArmorKind.VanillaDev,
            new DevArmorItem(1566),
            new DevArmorItem(1567),
            new DevArmorItem(1568)
        ),
        new(
            // D-Town
            DevArmorKind.VanillaDev,
            new DevArmorItem(1580),
            new DevArmorItem(1581),
            new DevArmorItem(1582),
            new DevArmorItem(1583, Hardmode: true)
        ),
        new(
            // Lazure (& Valkyrie Yo-Yo)
            DevArmorKind.VanillaDev,
            new DevArmorItem(3226),
            new DevArmorItem(3227),
            new DevArmorItem(3228, Hardmode: true),
            new DevArmorItem(3288, Hardmode: true)
        ),
        new(
            // Yoraiz0r (hi!!)
            DevArmorKind.VanillaDev,
            new DevArmorItem(3583, Hardmode: true),
            new DevArmorItem(3581),
            new DevArmorItem(3578),
            new DevArmorItem(3579),
            new DevArmorItem(3580, Hardmode: true)
        ),
        new(
            // Skiph
            DevArmorKind.VanillaDev,
            new DevArmorItem(3585),
            new DevArmorItem(3586),
            new DevArmorItem(3587),
            new DevArmorItem(3588),
            new DevArmorItem(3024, Stack: 4)
        ),
        new(
            // Loki
            DevArmorKind.VanillaDev,
            new DevArmorItem(3589),
            new DevArmorItem(3590),
            new DevArmorItem(3591),
            new DevArmorItem(3592, Hardmode: true),
            new DevArmorItem(3599, Stack: 4)
        ),
        new(
            // Arkhalis
            DevArmorKind.VanillaDev,
            new DevArmorItem(3368, Hardmode: true),
            new DevArmorItem(3921),
            new DevArmorItem(3922),
            new DevArmorItem(3923),
            new DevArmorItem(3924, Hardmode: true)
        ),
        new(
            // Leinfors †
            DevArmorKind.VanillaDev,
            new DevArmorItem(3925),
            new DevArmorItem(3926),
            new DevArmorItem(3927),
            new DevArmorItem(3928, Hardmode: true),
            new DevArmorItem(3929)
        ),
        new(
            // Ghostar
            DevArmorKind.VanillaDev,
            new DevArmorItem(4732),
            new DevArmorItem(4733),
            new DevArmorItem(4734),
            new DevArmorItem(4730, Hardmode: true)
        ),
        new(
            // Safeman
            DevArmorKind.VanillaDev,
            new DevArmorItem(4747),
            new DevArmorItem(4748),
            new DevArmorItem(4749),
            new DevArmorItem(4746, Hardmode: true)
        ),
        new(
            // FoodBarbarian
            DevArmorKind.VanillaDev,
            new DevArmorItem(4751),
            new DevArmorItem(4752),
            new DevArmorItem(4753),
            new DevArmorItem(4750, Hardmode: true)
        ),
        new(
            // Grox
            DevArmorKind.VanillaDev,
            new DevArmorItem(4755),
            new DevArmorItem(4756),
            new DevArmorItem(4757),
            new DevArmorItem(4754, Hardmode: true)
        ),
    ];

    internal static void AddNightshadeDevSet(params DevArmorItem[] items)
    {
        dev_armor_sets.Add(new DevArmorDefinition(DevArmorKind.NightshadeDev, items));
    }

    public override void Load()
    {
        base.Load();

        IL_Player.OpenBossBag += OpenBossBag_AlwaysTryForDevItems;
        On_Player.TryGettingDevArmor += TryGettingDevArmor_OnlyPullFromOurItems;
    }

    private static void OpenBossBag_AlwaysTryForDevItems(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(MoveType.After, x => x.MatchLdelemU1());

        c.EmitPop();
        c.EmitLdcI4(0); // false
    }

    private static void TryGettingDevArmor_OnlyPullFromOurItems(On_Player.orig_TryGettingDevArmor orig, Player self, IEntitySource source)
    {
        if (!Main.rand.NextBool(Main.tenthAnniversaryWorld ? 8 : 16))
        {
            return;
        }

        var hardmode = source is EntitySource_ItemOpen itemOpenSource && !ItemID.Sets.PreHardmodeLikeBossBag[itemOpenSource.ItemType];
        var devSet = dev_armor_sets[Main.rand.Next(dev_armor_sets.Count)];

        foreach (var item in devSet.Items)
        {
            if (item.Hardmode && !hardmode)
            {
                continue; // Skip hardmode items in pre-hardmode.
            }

            self.QuickSpawnItem(source, item.ItemType, item.Stack);
        }
    }

    public override void PostSetupContent()
    {
        base.PostSetupContent();

        dev_armor_sets.AddRange(vanilla_dev_sets);
        dev_armor_sets.AddRange(MakeSetsFromModItems(DevArmorKind.ModLoaderPatreon, ModLoaderMod.PatronSets));
        dev_armor_sets.AddRange(MakeSetsFromModItems(DevArmorKind.ModLoaderDev, ModLoaderMod.DeveloperSets));

        return;

        static IEnumerable<DevArmorDefinition> MakeSetsFromModItems<T>(DevArmorKind kind, T[][] sets) where T : ModItem
        {
            foreach (var set in sets)
            {
                var items = new List<DevArmorItem>();
                foreach (var item in set)
                {
                    var staticItem = ContentSamples.ItemsByType[item.Type];

                    // TODO: Support stack sizes when tML makes use of them.
                    items.Add(new DevArmorItem(item.Type, /*item.Item.stack*/ 1, IsHardmodeItem(staticItem)));
                }

                yield return new DevArmorDefinition(kind, items.ToArray());
            }
        }

        static bool IsHardmodeItem(Item item)
        {
            // wingSlot defaults to -1 but the game only considers > 0.
            // Similar case for damage.
            return item.wingSlot > 0 || item.damage > 0;
        }
    }
}