using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Nightshade.Common.Features;

// X for forcing sorting
internal sealed class XCursorTrailSlot : EquipSlot
{
    private sealed class CursorTrailSlotContext : CustomItemSlot
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

            if (checkItem.type == ItemID.None || VanityCursorSets.IsCursorTrail[checkItem.type])
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
            texture = Assets.Images.UI.CursorTrail_Slot.Asset.Value;
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

            Main.hoverItemName = Mods.Nightshade.UI.VanityCursorTrail.GetTextValue();
        }

        public override bool TryHandleSwap(ref Item item, int incomingContext, Player player)
        {
            if (!VanityCursorSets.IsCursorTrail[item.type])
            {
                return false;
            }
            
            item = ItemSlot.EquipSwap(item, Main.LocalPlayer.GetModPlayer<VanityCursorPlayer>().Trail, 0, out var success);
            if (success)
            {
                Main.EquipPageSelected = 2;
                AchievementsHelper.HandleOnEquip(player, item, Type);
            }

            return base.TryHandleSwap(ref item, incomingContext, player);
        }
    }

    public override ref Item GetItem(bool dye)
    {
        var vanityCursorPlayer = Main.LocalPlayer.GetModPlayer<VanityCursorPlayer>();
        return ref vanityCursorPlayer.Trail[dye ? 1 : 0]!;
    }

    public override int GetContext()
    {
        return ModContent.GetInstance<CursorTrailSlotContext>().Type;
    }
}