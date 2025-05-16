using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Accessories;

internal sealed class FourLeafClover : ModItem
{
    [ReinitializeDuringResizeArrays]
    private sealed class FlcGlobalTile : GlobalTile
    {
        public static bool[] DropsFourLeafClover = TileID.Sets.Factory.CreateNamedSet("DropsFourLeafClover")
                                                         .RegisterBoolSet(TileID.Plants, TileID.Plants2);

        public override void Drop(int i, int j, int type)
        {
            base.Drop(i, j, type);

            if (!DropsFourLeafClover[type])
            {
                return;
            }

            if (!Main.rand.NextBool(200))
            {
                return;
            }

            Item.NewItem(
                WorldGen.GetItemSource_FromTileBreak(i, j),
                i * 16,
                j * 16,
                16,
                16,
                ModContent.ItemType<FourLeafClover>()
            );
        }
    }

    public sealed class FlcPlayer : ModPlayer
    {
        public bool HasLuck { get; set; }

        public override void ResetEffects()
        {
            base.ResetEffects();

            HasLuck = false;
        }

        public override void RefreshInfoAccessoriesFromTeamPlayers(Player otherPlayer)
        {
            base.RefreshInfoAccessoriesFromTeamPlayers(otherPlayer);

            if (otherPlayer.GetModPlayer<FlcPlayer>().HasLuck)
            {
                HasLuck = true;
            }

            // Because this code is normally ran right after
            // RefreshInfoAccsFromTeamPlayers.
            if (HasLuck)
            {
                Player.equipmentBasedLuckBonus += 0.05f;
            }
        }
    }

    public override string Texture => Assets.Images.Items.Accessories.FourLeafClover.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (20, 26);

        Item.accessory = true;

        Item.SetShopValues(
            ItemRarityColor.Blue1,
            Item.sellPrice(silver: 4, copper: 44)
        );
    }

    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);

        player.GetModPlayer<FlcPlayer>().HasLuck = true;
    }
}