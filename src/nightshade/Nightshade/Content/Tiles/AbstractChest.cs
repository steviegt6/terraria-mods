using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Nightshade.Content.Tiles;

// TODO: Chest name stuff.

internal abstract class AbstractChest : ModTile
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.tileSpelunker[Type] = true;
        Main.tileContainer[Type] = true;
        Main.tileShine2[Type] = true;
        Main.tileShine[Type] = 1200;
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileOreFinderPriority[Type] = 500;
        TileID.Sets.HasOutlines[Type] = true;
        TileID.Sets.BasicChest[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;
        TileID.Sets.AvoidedByNPCs[Type] = true;
        TileID.Sets.InteractibleByNPCs[Type] = true;
        TileID.Sets.IsAContainer[Type] = true;
        TileID.Sets.FriendlyFairyCanLureTo[Type] = true;
        TileID.Sets.GeneralPlacementTiles[Type] = false;

        AdjTiles = [TileID.Containers];

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.Origin = new Point16(0, 1);
        TileObjectData.newTile.CoordinateHeights = [16, 18];
        TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
        TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
        TileObjectData.newTile.AnchorInvalidTiles =
        [
            TileID.MagicalIceBlock,
            TileID.Boulder,
            TileID.BouncyBoulder,
            TileID.LifeCrystalBoulder,
            TileID.RollingCactus,
        ];
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.LavaDeath = false;
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
        TileObjectData.addTile(Type);
    }

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
    {
        return true;
    }

    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        base.NumDust(i, j, fail, ref num);

        num = 1;
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        base.KillMultiTile(i, j, frameX, frameY);

        Chest.DestroyChest(i, j);
    }

    public override LocalizedText DefaultContainerName(int frameX, int frameY) => Mod.GetLocalization($"Tiles.{GetType().Name}", PrettyPrintName);

    public override bool RightClick(int i, int j)
    {
        var player = Main.LocalPlayer;
        var tile = Main.tile[i, j];
        Main.mouseRightRelease = false;
        var left = i;
        var top = j;
        if (tile.TileFrameX % 36 != 0)
        {
            left--;
        }

        if (tile.TileFrameY != 0)
        {
            top--;
        }

        player.CloseSign();
        player.SetTalkNPC(-1);
        Main.npcChatCornerItem = 0;
        Main.npcChatText = "";
        if (Main.editChest)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            Main.editChest = false;
            Main.npcChatText = string.Empty;
        }

        if (player.editedChestName)
        {
            NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f);
            player.editedChestName = false;
        }

        var isLocked = Chest.IsLocked(left, top);
        if (Main.netMode == NetmodeID.MultiplayerClient && !isLocked)
        {
            if (left == player.chestX && top == player.chestY && player.chest != -1)
            {
                player.chest = -1;
                Recipe.FindRecipes();
                SoundEngine.PlaySound(SoundID.MenuClose);
            }
            else
            {
                NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, top);
                Main.stackSplit = 600;
            }
        }
        else
        {
            if (isLocked)
            {
                // TODO: Hook
            }
            else
            {
                var chest = Chest.FindChest(left, top);
                if (chest == -1)
                {
                    return true;
                }

                Main.stackSplit = 600;
                if (chest == player.chest)
                {
                    player.chest = -1;
                    SoundEngine.PlaySound(SoundID.MenuClose);
                }
                else
                {
                    SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
                    player.OpenChest(left, top, chest);
                }

                Recipe.FindRecipes();
            }
        }

        return true;
    }
}