using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Nightshade.Common.Features;

internal sealed class VanityCursorSlot : EquipSlot
{
    private sealed class VanityCursorSlotContext : CustomItemSlot
    {
        private const int vanilla_context = ItemSlot.Context.EquipGrapple;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            ItemSlot.canShareAt[Type] = true;
        }

        public override bool PreOverrideHover(Item item, ref int context)
        {
            context = vanilla_context;

            return base.PreOverrideHover(item, ref context);
        }

        public override bool PreLeftClick(Item item, ref int context)
        {
            context = vanilla_context;

            return base.PreLeftClick(item, ref context);
        }

        public override string? PreGetOverrideInstructions(Item item, ref int context)
        {
            context = vanilla_context;

            return base.PreGetOverrideInstructions(item, ref context);
        }

        public override int? PrePickItemMovementAction(Item item, ref int context, Item checkItem)
        {
            // TODO
            // context = vanilla_context;

            if (checkItem.type == ItemID.None || VanityCursorSets.IsVanityCursor[checkItem.type])
            {
                return 1;
            }

            return base.PrePickItemMovementAction(item, ref context, checkItem);
        }

        public override bool PreSwapVanityEquip(Item item, ref int context, Player player)
        {
            context = vanilla_context;

            return base.PreSwapVanityEquip(item, ref context, player);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Item item, ref int context, Vector2 position, Color lightColor)
        {
            context = vanilla_context;

            return base.PreDraw(spriteBatch, item, ref context, position, lightColor);
        }

        public override bool ModifyIcon(SpriteBatch spriteBatch, ref Texture2D texture, ref Vector2 position, ref Rectangle? sourceRectangle, ref Color color, ref float rotation, ref Vector2 origin, ref float scale, ref SpriteEffects effects)
        {
            texture = Assets.Images.UI.Cursor_Slot.Asset.Value;
            sourceRectangle = null;

            return base.ModifyIcon(spriteBatch, ref texture, ref position, ref sourceRectangle, ref color, ref rotation, ref origin, ref scale, ref effects);
        }

        public override bool PreMouseHover(Item item, ref int context)
        {
            context = vanilla_context;

            return base.PreMouseHover(item, ref context);
        }

        public override void PostMouseHover(Item item, int context)
        {
            base.PostMouseHover(item, context);

            Main.hoverItemName = Mods.Nightshade.UI.VanityCursor.GetTextValue();
        }

        public override bool TryHandleSwap(ref Item item, int incomingContext, Player player)
        {
            if (!VanityCursorSets.IsVanityCursor[item.type])
            {
                return false;
            }

            item = ItemSlot.EquipSwap(item, Main.LocalPlayer.GetModPlayer<VanityCursorPlayer>().Cursor, 0, out var success);
            if (success)
            {
                Main.EquipPageSelected = 2;
                AchievementsHelper.HandleOnEquip(player, item, Type);
            }

            return base.TryHandleSwap(ref item, incomingContext, player);
        }
    }

    /*private sealed class VanityCursorDyeSlotContext : CustomItemSlot
    {
        private const int vanilla_context = ItemSlot.Context.EquipMiscDye;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            ItemSlot.canShareAt[Type] = true;
        }

        public override bool PreOverrideHover(Item item, ref int context)
        {
            context = vanilla_context;

            return base.PreOverrideHover(item, ref context);
        }

        public override bool PreLeftClick(Item item, ref int context)
        {
            context = vanilla_context;

            return base.PreLeftClick(item, ref context);
        }

        public override int? PrePickItemMovementAction(Item item, ref int context, Item checkItem)
        {
            context = vanilla_context;

            return base.PrePickItemMovementAction(item, ref context, checkItem);
        }

        override
    }*/

    public override int GetContext(EquipSlotKind kind)
    {
        return kind switch
        {
            EquipSlotKind.Functional => ModContent.GetInstance<VanityCursorSlotContext>().Type,
            EquipSlotKind.Dye => ItemSlot.Context.EquipMiscDye,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };
    }

    public override ref Item GetItem(EquipSlotKind kind)
    {
        var vanityCursorPlayer = Main.LocalPlayer.GetModPlayer<VanityCursorPlayer>();

        switch (kind)
        {
            case EquipSlotKind.Functional:
                return ref vanityCursorPlayer.Cursor[0]!;

            case EquipSlotKind.Dye:
                return ref vanityCursorPlayer.Cursor[1]!;

            default:
                throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
        }
    }

    public override bool CanBeToggled(EquipSlotKind kind)
    {
        return true;
    }

    public override void HandleToggle(ref Texture2D toggleButton, Rectangle toggleRect, Point mouseLoc, ref string? hoverText, ref bool toggleHovered, EquipSlotKind kind)
    {
        if (kind != EquipSlotKind.Functional && kind != EquipSlotKind.Dye)
        {
            return;
        }

        toggleButton = Assets.Images.UI.Cursor_Visibility.Asset.Value;

        if (!toggleRect.Contains(mouseLoc) || PlayerInput.IgnoreMouseInterface)
        {
            return;
        }

        Main.LocalPlayer.mouseInterface = true;
        toggleHovered = true;

        var player = Main.LocalPlayer.GetModPlayer<VanityCursorPlayer>();
        var visibility = GetVisibility(player, kind);
        if (Main.mouseLeft && Main.mouseLeftRelease)
        {
            SetVisibility(player, kind, IncrementVisibility(visibility));

            Main.mouseLeftRelease = false;
            SoundEngine.PlaySound(SoundID.MenuTick);

            // No need to sync (for now?).
            /*
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, Main.myPlayer);
            }
            */
        }
        else if (Main.mouseRight && Main.mouseRightRelease)
        {
            SetVisibility(player, kind, DecrementVisibility(visibility));

            Main.mouseRightRelease = false;
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        hoverText = Mods.Nightshade.UI.VanityCursorAppliesTo.GetChildTextValue(visibility.ToString());
    }

    public override void DrawToggle(string? hoverText, Texture2D toggleButton, Rectangle toggleRect, EquipSlotKind kind)
    {
        if (kind != EquipSlotKind.Functional && kind != EquipSlotKind.Dye)
        {
            return;
        }

        var player = Main.LocalPlayer.GetModPlayer<VanityCursorPlayer>();
        var visibility = GetVisibility(player, kind);

        var frame = toggleButton.Frame(1, 4, 0, (int)visibility);
        var pos = toggleRect.TopLeft();
        pos.X -= 4;
        pos.Y -= 2;
        Main.spriteBatch.Draw(toggleButton, pos, frame, Color.White);

        if (hoverText is null)
        {
            return;
        }
        Main.HoverItem = new Item();
        Main.hoverItemName = hoverText;
    }

    private static CursorVisibility GetVisibility(VanityCursorPlayer player, EquipSlotKind kind)
    {
        return kind switch
        {
            EquipSlotKind.Functional => player.FunctionalVisibility,
            EquipSlotKind.Dye => player.DyeVisibility,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };
    }

    private static void SetVisibility(VanityCursorPlayer player, EquipSlotKind kind, CursorVisibility visibility)
    {
        switch (kind)
        {
            case EquipSlotKind.Functional:
                player.FunctionalVisibility = visibility;
                break;

            case EquipSlotKind.Dye:
                player.DyeVisibility = visibility;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
        }
    }

    private static CursorVisibility IncrementVisibility(CursorVisibility visibility)
    {
        if (visibility >= CursorVisibility.Both)
        {
            visibility = CursorVisibility.None;
        }
        else
        {
            visibility++;
        }

        return visibility;
    }
    
    private static CursorVisibility DecrementVisibility(CursorVisibility visibility)
    {
        if (visibility <= CursorVisibility.None)
        {
            visibility = CursorVisibility.Both;
        }
        else
        {
            visibility--;
        }

        return visibility;
    }
}