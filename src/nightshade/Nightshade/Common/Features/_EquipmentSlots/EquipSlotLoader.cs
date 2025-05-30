using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using MonoMod.Cil;

using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Nightshade.Common.Features;

internal sealed class EquipSlotLoader : ModSystem
{
    public static EquipSlot Pet { get; } = new PetSlot();

    public static EquipSlot LightPet { get; } = new LightPetSlot();

    public static EquipSlot Minecart { get; } = new MinecartSlot();

    public static EquipSlot Mount { get; } = new MountSlot();

    public static EquipSlot Hook { get; } = new HookSlot();

    internal static readonly List<EquipSlot> SLOTS =
    [
        Pet,
        LightPet,
        Minecart,
        Mount,
        Hook,
    ];

    public override void Load()
    {
        base.Load();

        IL_Main.DrawInventory += DrawInventory_ReplaceVanillaMiscSlotDrawing;
    }

    private static void DrawInventory_ReplaceVanillaMiscSlotDrawing(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(
            x => x.MatchLdsfld<Main>(nameof(Main.EquipPage)),
            x => x.MatchLdcI4((int)EquipmentPageId.Misc)
        );

        c.GotoNext(MoveType.Before, x => x.MatchLdloca(out _));
        c.EmitDelegate(ManuallyDrawMiscSlots);

        var pos = c.Index;

        // Find the label needed to escape the if-else chain by going to the
        // next, shorter clause.
        c.GotoNext(
            x => x.MatchLdsfld<Main>(nameof(Main.EquipPage)),
            x => x.MatchLdcI4(1)
        );

        ILLabel? label = null;
        c.GotoNext(x => x.MatchBr(out label));
        {
            Debug.Assert(label is not null);
        }

        c.Index = pos;

        c.EmitBr(label);
    }

    // Modified from Main::DrawInventory.
    private static void ManuallyDrawMiscSlots()
    {
        var mouseLoc = new Point(Main.mouseX, Main.mouseY);

        var backPanelSize = new Rectangle(0, 0, (int)(TextureAssets.InventoryBack.Width() * Main.inventoryScale), (int)(TextureAssets.InventoryBack.Height() * Main.inventoryScale));

        var xPos = Main.screenWidth - 92;
        var yPos = Main.mH + 174;

        for (var i = 0; i < 2; i++)
        {
            backPanelSize.X = xPos + i * -47;

            for (var slot = 0; slot < SLOTS.Count; slot++)
            {
                var context = SLOTS[slot].GetContext();
                var canBeToggled = SLOTS[slot].CanBeToggled;

                ref var item = ref SLOTS[slot].GetItem(i == 1);

                if (i == 1)
                {
                    context = ItemSlot.Context.EquipMiscDye;
                    canBeToggled = false;
                }

                backPanelSize.Y = yPos + slot * 47;
                var toggleButton = TextureAssets.InventoryTickOn.Value;
                var toggleRect = new Rectangle(backPanelSize.Left + 34, backPanelSize.Top - 2, toggleButton.Width, toggleButton.Height);
                var toggleHovered = false;

                var hoverText = default(string);
                if (canBeToggled)
                {
                    SLOTS[slot].HandleToggle(ref toggleButton, toggleRect, mouseLoc, ref hoverText, ref toggleHovered);
                }

                if (backPanelSize.Contains(mouseLoc) && !toggleHovered && !PlayerInput.IgnoreMouseInterface)
                {
                    Main.LocalPlayer.mouseInterface = true;
                    Main.armorHide = true;
                    ItemSlot.Handle(ref item, context);
                }

                ItemSlot.Draw(Main.spriteBatch, ref item, context, backPanelSize.TopLeft());

                if (canBeToggled)
                {
                    SLOTS[slot].DrawToggle(hoverText, toggleButton, toggleRect);
                }
            }
        }

        yPos += 47 * SLOTS.Count + 12;
        xPos += 8;

        var buffsDrawn = 0;
        var buffsPerColumn = 3;

        const int offset_or_smth = 260;

        if (Main.screenHeight > 630 + offset_or_smth * (Main.mapStyle == 1).ToInt())
        {
            buffsPerColumn++;
        }

        if (Main.screenHeight > 680 + offset_or_smth * (Main.mapStyle == 1).ToInt())
        {
            buffsPerColumn++;
        }

        if (Main.screenHeight > 730 + offset_or_smth * (Main.mapStyle == 1).ToInt())
        {
            buffsPerColumn++;
        }

        const int buff_size = 46;

        var hoveredBuff = -1;
        for (var i = 0; i < Player.maxBuffs; i++)
        {
            if (Main.LocalPlayer.buffType[i] == 0)
            {
                continue;
            }

            var xOff = buffsDrawn / buffsPerColumn;
            var yOff = buffsDrawn % buffsPerColumn;
            var buffDrawPos = new Point(xPos + xOff * -buff_size, yPos + yOff * buff_size);
            hoveredBuff = Main.DrawBuffIcon(hoveredBuff, i, buffDrawPos.X, buffDrawPos.Y);
            UILinkPointNavigator.SetPosition(9000 + buffsDrawn, new Vector2(buffDrawPos.X + 30, buffDrawPos.Y + 30));
            buffsDrawn++;

            if (Main.buffAlpha[i] < 0.65f)
            {
                Main.buffAlpha[i] = 0.65f;
            }
        }

        UILinkPointNavigator.Shortcuts.BUFFS_DRAWN = buffsDrawn;
        UILinkPointNavigator.Shortcuts.BUFFS_PER_COLUMN = buffsPerColumn;

        if (hoveredBuff < 0)
        {
            return;
        }

        var buff = Main.LocalPlayer.buffType[hoveredBuff];
        if (buff <= 0)
        {
            return;
        }

        var buffName = Lang.GetBuffName(buff);
        var buffTooltip = Main.GetBuffTooltip(Main.LocalPlayer, buff);
        if (buff == 147)
        {
            Main.bannerMouseOver = true;
        }

        /*
        if (meleeBuff[num34])
            MouseTextHackZoom(buffName, -10, 0, buffTooltip);
        else
            MouseTextHackZoom(buffName, buffTooltip);
        */
        var rare = 0;
        if (Main.meleeBuff[buff])
        {
            rare = -10;
        }

        BuffLoader.ModifyBuffText(buff, ref buffName, ref buffTooltip, ref rare);
        Main.instance.MouseTextHackZoom(buffName, rare, diff: 0, buffTooltip);
    }
}