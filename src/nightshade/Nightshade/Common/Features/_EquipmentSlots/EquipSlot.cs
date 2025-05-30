using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;

namespace Nightshade.Common.Features;

internal abstract class EquipSlot
{
    public virtual bool CanBeToggled => false;

    public virtual bool IsEffectHidden => false;

    public abstract int GetContext(EquipmentSlotContext slotCtx);

    public virtual void HandleToggle(ref Texture2D toggleButton, Rectangle toggleRect, Point mouseLoc, ref string? hoverText, ref bool toggleHovered)
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

    public virtual void OnToggle() { }

    public virtual void DrawExtras(string? hoverText, Texture2D toggleButton, Rectangle toggleRect)
    {
        Main.spriteBatch.Draw(toggleButton, toggleRect.TopLeft(), Color.White * 0.7f);
        if (hoverText is null)
        {
            return;
        }

        Main.HoverItem = new Item();
        Main.hoverItemName = hoverText;
    }
}