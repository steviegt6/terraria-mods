using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent.Achievements;
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
            // context = vanilla_context;

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

            return base.PrePickItemMovementAction(item, ref context, checkItem);
        }

        public override bool PreSwapVanityEquip(Item item, ref int context, Player player)
        {
            context = vanilla_context;

            return base.PreSwapVanityEquip(item, ref context, player);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Item item, ref int context, Vector2 position, Color lightColor)
        {
            return base.PreDraw(spriteBatch, item, ref context, position, lightColor);
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

        public override bool PreSwapEquip(Item item, ref int context)
        {
            var player = Main.LocalPlayer;

            if (ItemSlot.isEquipLocked(item.type) || item.IsAir)
            {
                return false;
            }

            item = ItemSlot.EquipSwap(item, player.GetModPlayer<VanityCursorPlayer>().Vanity, 0, out var success);
            if (success)
            {
                Main.EquipPageSelected = 2;
                AchievementsHelper.HandleOnEquip(player, item, Type);
            }

            Recipe.FindRecipes();
            return false;
        }
    }

    public override ref Item GetItem(bool dye)
    {
        var vanityCursorPlayer = Main.LocalPlayer.GetModPlayer<VanityCursorPlayer>();
        return ref vanityCursorPlayer.Vanity[dye ? 2 : 0]!;
    }

    public override int GetContext()
    {
        return ModContent.GetInstance<VanityCursorSlotContext>().Type;
    }
}