using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Nightshade.Common.Features;

[Autoload(false)]
internal abstract class VanillaEquipSlot : EquipSlot
{
    public override void HandleToggle(ref Texture2D toggleButton, Rectangle toggleRect, Point mouseLoc, ref string? hoverText, ref bool toggleHovered)
    {
        if (IsEffectHidden)
        {
            toggleButton = TextureAssets.InventoryTickOff.Value;
        }

        if (!toggleRect.Contains(mouseLoc) || PlayerInput.IgnoreMouseInterface)
        {
            return;
        }

        Main.LocalPlayer.mouseInterface = true;
        toggleHovered = true;
        if (Main.mouseLeft && Main.mouseLeftRelease)
        {
            OnToggle();

            Main.mouseLeftRelease = false;
            SoundEngine.PlaySound(SoundID.MenuTick);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, Main.myPlayer);
            }
        }

        var inter = !IsEffectHidden ? 1 : 2;
        hoverText = Lang.inter[58 + inter].Value;
    }

    public override void DrawToggle(string? hoverText, Texture2D toggleButton, Rectangle toggleRect)
    {
        Main.spriteBatch.Draw(toggleButton, toggleRect.TopLeft(), Color.White * 0.7f);
        if (hoverText is null)
        {
            return;
        }

        Main.HoverItem = new Item();
        Main.hoverItemName = hoverText;
    }

    protected virtual void OnToggle() { }
}

internal sealed class PetSlot : VanillaEquipSlot
{
    public override bool CanBeToggled => true;

    public override bool IsEffectHidden => Main.LocalPlayer.hideMisc[0];

    public override int GetContext()
    {
        return ItemSlot.Context.EquipPet;
    }

    public override ref Item GetItem(bool dye)
    {
        return ref dye ? ref Main.LocalPlayer.miscDyes[0] : ref Main.LocalPlayer.miscEquips[0];
    }

    protected override void OnToggle()
    {
        base.OnToggle();

        Main.LocalPlayer.TogglePet();
    }
}

internal sealed class LightPetSlot : VanillaEquipSlot
{
    public override bool CanBeToggled => true;

    public override bool IsEffectHidden => Main.LocalPlayer.hideMisc[1];

    public override int GetContext()
    {
        return ItemSlot.Context.EquipLight;
    }

    public override ref Item GetItem(bool dye)
    {
        return ref dye ? ref Main.LocalPlayer.miscDyes[1] : ref Main.LocalPlayer.miscEquips[1];
    }

    protected override void OnToggle()
    {
        base.OnToggle();

        Main.LocalPlayer.ToggleLight();
    }
}

internal sealed class MinecartSlot : VanillaEquipSlot
{
    public override bool CanBeToggled => Main.LocalPlayer.unlockedSuperCart;

    public override int GetContext()
    {
        return ItemSlot.Context.EquipMinecart;
    }

    public override ref Item GetItem(bool dye)
    {
        return ref dye ? ref Main.LocalPlayer.miscDyes[2] : ref Main.LocalPlayer.miscEquips[2];
    }

    public override void HandleToggle(ref Texture2D toggleButton, Rectangle toggleRect, Point mouseLoc, ref string? hoverText, ref bool toggleHovered)
    {
        toggleButton = TextureAssets.Extra[255].Value;
        if (!Main.LocalPlayer.enabledSuperCart)
        {
            toggleButton = TextureAssets.Extra[256].Value;
        }

        toggleRect = new Rectangle(toggleRect.X + toggleRect.Width / 2, toggleRect.Y + toggleRect.Height / 2, toggleRect.Width, toggleRect.Height);
        toggleRect.Offset(-toggleRect.Width / 2, -toggleRect.Height / 2);
        if (!toggleRect.Contains(mouseLoc) || PlayerInput.IgnoreMouseInterface)
        {
            return;
        }

        Main.LocalPlayer.mouseInterface = true;
        toggleHovered = true;
        if (Main.mouseLeft && Main.mouseLeftRelease)
        {
            Main.LocalPlayer.enabledSuperCart = !Main.LocalPlayer.enabledSuperCart;
            Main.NewText(Main.LocalPlayer.enabledSuperCart);
            Main.mouseLeftRelease = false;
            SoundEngine.PlaySound(SoundID.MenuTick);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, Main.myPlayer);
            }
        }

        hoverText = Language.GetTextValue(Main.LocalPlayer.enabledSuperCart ? "GameUI.SuperCartEnabled" : "GameUI.SuperCartDisabled");
    }

    public override void DrawToggle(string? hoverText, Texture2D toggleButton, Rectangle toggleRect)
    {
        Main.spriteBatch.Draw(toggleButton, toggleRect.TopLeft(), Color.White);

        if (hoverText is null)
        {
            return;
        }
        Main.HoverItem = new Item();
        Main.hoverItemName = hoverText;
    }
}

internal sealed class MountSlot : VanillaEquipSlot
{
    public override int GetContext()
    {
        return ItemSlot.Context.EquipMount;
    }

    public override ref Item GetItem(bool dye)
    {
        return ref dye ? ref Main.LocalPlayer.miscDyes[3] : ref Main.LocalPlayer.miscEquips[3];
    }
}

internal sealed class HookSlot : VanillaEquipSlot
{
    public override int GetContext()
    {
        return ItemSlot.Context.EquipGrapple;
    }

    public override ref Item GetItem(bool dye)
    {
        return ref dye ? ref Main.LocalPlayer.miscDyes[4] : ref Main.LocalPlayer.miscEquips[4];
    }
}