using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoMod.Cil;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Nightshade.Common.Features;

internal sealed class MiscSlotLoader : ModSystem
{
    public static MiscSlot Pet { get; } = new PetSlot();

    public static MiscSlot LightPet { get; } = new LightPetSlot();

    public static MiscSlot Minecart { get; } = new MinecartSlot();

    public static MiscSlot Mount { get; } = new MountSlot();

    public static MiscSlot Hook { get; } = new HookSlot();

    private readonly List<MiscSlot> slots =
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
        var value = new Point(Main.mouseX, Main.mouseY);
        var r = new Rectangle(0, 0, (int)(TextureAssets.InventoryBack.Width() * Main.inventoryScale), (int)(TextureAssets.InventoryBack.Height() * Main.inventoryScale));
        Item[] inv = Main.LocalPlayer.miscEquips;
        var num23 = Main.screenWidth - 92;
        var num24 = Main.mH + 174;
        for (var l = 0; l < 2; l++)
        {
            inv = l switch
            {
                0 => Main.LocalPlayer.miscEquips,
                1 => Main.LocalPlayer.miscDyes,
                _ => inv,
            };

            r.X = num23 + l * -47;
            for (var m = 0; m < 5; m++)
            {
                var context = 0;
                var num25 = -1;
                var flag2 = false;
                switch (m)
                {
                    case 0:
                        context = 19;
                        num25 = 0;
                        break;

                    case 1:
                        context = 20;
                        num25 = 1;
                        break;

                    case 2:
                        context = 18;
                        flag2 = Main.LocalPlayer.unlockedSuperCart;
                        break;

                    case 3:
                        context = 17;
                        break;

                    case 4:
                        context = 16;
                        break;
                }

                if (l == 1)
                {
                    context = 33;
                    num25 = -1;
                    flag2 = false;
                }

                r.Y = num24 + m * 47;
                var flag3 = false;
                Texture2D value2 = TextureAssets.InventoryTickOn.Value;
                var r2 = new Rectangle(r.Left + 34, r.Top - 2, value2.Width, value2.Height);
                var num26 = 0;
                if (num25 != -1)
                {
                    if (Main.LocalPlayer.hideMisc[num25])
                    {
                        value2 = TextureAssets.InventoryTickOff.Value;
                    }

                    if (r2.Contains(value) && !PlayerInput.IgnoreMouseInterface)
                    {
                        Main.LocalPlayer.mouseInterface = true;
                        flag3 = true;
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            if (num25 == 0)
                            {
                                Main.LocalPlayer.TogglePet();
                            }

                            if (num25 == 1)
                            {
                                Main.LocalPlayer.ToggleLight();
                            }

                            Main.mouseLeftRelease = false;
                            SoundEngine.PlaySound(SoundID.MenuTick);
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
                            }
                        }

                        num26 = ((!Main.LocalPlayer.hideMisc[num25]) ? 1 : 2);
                    }
                }

                if (flag2)
                {
                    value2 = TextureAssets.Extra[255].Value;
                    if (!Main.LocalPlayer.enabledSuperCart)
                    {
                        value2 = TextureAssets.Extra[256].Value;
                    }

                    r2 = new Rectangle(r2.X + r2.Width / 2, r2.Y + r2.Height / 2, r2.Width, r2.Height);
                    r2.Offset(-r2.Width / 2, -r2.Height / 2);
                    if (r2.Contains(value) && !PlayerInput.IgnoreMouseInterface)
                    {
                        Main.LocalPlayer.mouseInterface = true;
                        flag3 = true;
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            Main.LocalPlayer.enabledSuperCart = !Main.LocalPlayer.enabledSuperCart;
                            Main.mouseLeftRelease = false;
                            SoundEngine.PlaySound(SoundID.MenuTick);
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
                            }
                        }

                        num26 = ((!Main.LocalPlayer.enabledSuperCart) ? 1 : 2);
                    }
                }

                if (r.Contains(value) && !flag3 && !PlayerInput.IgnoreMouseInterface)
                {
                    Main.LocalPlayer.mouseInterface = true;
                    Main.armorHide = true;
                    ItemSlot.Handle(inv, context, m);
                }

                ItemSlot.Draw(Main.spriteBatch, inv, context, m, r.TopLeft());
                if (num25 != -1)
                {
                    Main.spriteBatch.Draw(value2, r2.TopLeft(), Color.White * 0.7f);
                    if (num26 > 0)
                    {
                        Main.HoverItem = new Item();
                        Main.hoverItemName = Lang.inter[58 + num26].Value;
                    }
                }

                if (flag2)
                {
                    Main.spriteBatch.Draw(value2, r2.TopLeft(), Color.White);
                    if (num26 > 0)
                    {
                        Main.HoverItem = new Item();
                        Main.hoverItemName = Language.GetTextValue((num26 == 1) ? "GameUI.SuperCartDisabled" : "GameUI.SuperCartEnabled");
                    }
                }
            }
        }

        num24 += 247;
        num23 += 8;
        var num27 = -1;
        var num28 = 0;
        var num29 = 3;
        var num30 = 260;
        if (Main.screenHeight > 630 + num30 * (Main.mapStyle == 1).ToInt())
        {
            num29++;
        }

        if (Main.screenHeight > 680 + num30 * (Main.mapStyle == 1).ToInt())
        {
            num29++;
        }

        if (Main.screenHeight > 730 + num30 * (Main.mapStyle == 1).ToInt())
        {
            num29++;
        }

        var num31 = 46;
        for (var n = 0; n < Player.maxBuffs; n++)
        {
            if (Main.LocalPlayer.buffType[n] != 0)
            {
                var num32 = num28 / num29;
                var num33 = num28 % num29;
                var point = new Point(num23 + num32 * -num31, num24 + num33 * num31);
                num27 = Main.DrawBuffIcon(num27, n, point.X, point.Y);
                UILinkPointNavigator.SetPosition(9000 + num28, new Vector2(point.X + 30, point.Y + 30));
                num28++;
                if (Main.buffAlpha[n] < 0.65f)
                {
                    Main.buffAlpha[n] = 0.65f;
                }
            }
        }

        UILinkPointNavigator.Shortcuts.BUFFS_DRAWN = num28;
        UILinkPointNavigator.Shortcuts.BUFFS_PER_COLUMN = num29;
        if (num27 >= 0)
        {
            int num34 = Main.LocalPlayer.buffType[num27];
            if (num34 > 0)
            {
                var buffName = Lang.GetBuffName(num34);
                string buffTooltip = Main.GetBuffTooltip(Main.LocalPlayer, num34);
                if (num34 == 147)
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
                if (Main.meleeBuff[num34])
                {
                    rare = -10;
                }

                BuffLoader.ModifyBuffText(num34, ref buffName, ref buffTooltip, ref rare);
                Main.instance.MouseTextHackZoom(buffName, rare, diff: 0, buffTooltip);
            }
        }
    }
}