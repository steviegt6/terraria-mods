using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;

namespace Nightshade.Common.Features;

internal sealed class PetSlot : EquipSlot
{
    public override bool CanBeToggled => true;

    public override bool IsEffectHidden => Main.LocalPlayer.hideMisc[0];

    public override int GetContext(EquipmentSlotContext slotCtx)
    {
        return 19;
    }

    public override void OnToggle()
    {
        base.OnToggle();

        Main.LocalPlayer.TogglePet();
    }
}

internal sealed class LightPetSlot : EquipSlot
{
    public override bool CanBeToggled => true;

    public override bool IsEffectHidden => Main.LocalPlayer.hideMisc[1];

    public override int GetContext(EquipmentSlotContext slotCtx)
    {
        return 20;
    }

    public override void OnToggle()
    {
        base.OnToggle();

        Main.LocalPlayer.ToggleLight();
    }
}

internal sealed class MinecartSlot : EquipSlot
{
    public override bool CanBeToggled => Main.LocalPlayer.unlockedSuperCart;

    public override int GetContext(EquipmentSlotContext slotCtx)
    {
        return 18;
    }

    public override void HandleToggle(ref Texture2D toggleButton, Rectangle toggleRect, Point mouseLoc, ref string? hoverText, ref bool toggleHovered)
    {
        // base.HandleToggle(ref toggleButton, toggleRect, mouseLoc, ref hoverText, ref toggleHovered);
        
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
                NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
            }
        }

        hoverText = Language.GetTextValue(Main.LocalPlayer.enabledSuperCart ? "GameUI.SuperCartEnabled" : "GameUI.SuperCartDisabled");
    }

    public override void DrawExtras(string? hoverText, Texture2D toggleButton, Rectangle toggleRect)
    {
        base.DrawExtras(hoverText, toggleButton, toggleRect);
        
        Main.spriteBatch.Draw(toggleButton, toggleRect.TopLeft(), Color.White);

        if (hoverText is null)
        {
            return;
        }
        Main.HoverItem = new Item();
        Main.hoverItemName = hoverText;
    }
}

internal sealed class MountSlot : EquipSlot
{
    public override int GetContext(EquipmentSlotContext slotCtx)
    {
        return 17;
    }
}

internal sealed class HookSlot : EquipSlot
{
    public override int GetContext(EquipmentSlotContext slotCtx)
    {
        return 16;
    }
}