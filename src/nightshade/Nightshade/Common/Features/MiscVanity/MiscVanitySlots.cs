using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;

using MonoMod.Cil;

using Nightshade.Common.Loading;

using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace Nightshade.Common.Features.MiscVanity;

internal sealed class MiscVanitySlots : IInitializer
{
    // Responsible for saving the items in the vanity slots.
    private sealed class MiscVanitySlotPlayer : ModPlayer
    {
        private const string misc_vanity_key = "MiscVanity";

        public Item?[] MiscVanity = new Item?[5];

        public override void Initialize()
        {
            base.Initialize();

            MiscVanity = [new Item(), new Item(), new Item(), new Item(), new Item()];
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);

            var miscVanity = MiscVanity.Select(ItemIO.Save).ToList();
            tag.Add(misc_vanity_key, miscVanity);
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);

            var miscVanity = tag.GetList<TagCompound>(misc_vanity_key);
            for (var i = 0; i < miscVanity.Count; i++)
            {
                Debug.Assert(MiscVanity.Length > i, "MiscVanity length is less than the number of items in the tag.");

                MiscVanity[i] = ItemIO.Load(miscVanity[i]);
            }
        }
    }

    private enum VanitySlotId
    {
        Pet      = 0,
        LightPet = 1,
        Minecart = 2,
        Mount    = 3,
        Hook     = 4,
    }

    private enum EquipPageId
    {
        Misc = 2,
    }

#region Implementations
    private sealed class Pet : IInitializer
    {
        void IInitializer.Load() { }
    }

    private sealed class LightPet : IInitializer
    {
        void IInitializer.Load() { }
    }

    private sealed class Minecart : IInitializer
    {
        void IInitializer.Load() { }
    }

    private sealed class Mount : IInitializer
    {
        void IInitializer.Load() { }
    }

    private sealed class Hook : IInitializer
    {
        void IInitializer.Load()
        {
            On_Main.DrawProjDirect += (orig, self, proj) =>
            {
                // TODO: Does this account for Fables' hooks?
                if (proj.aiStyle != ProjAIStyleID.Hook)
                {
                    orig(self, proj);
                    return;
                }

                var player   = Main.player[proj.owner];
                var vanity   = player.GetModPlayer<MiscVanitySlotPlayer>();
                var hookItem = vanity.MiscVanity[(int)VanitySlotId.Hook];

                if (hookItem?.IsAir ?? true)
                {
                    orig(self, proj);
                    return;
                }

                var vanityType = hookItem.shoot;
                if (proj.type == vanityType)
                {
                    orig(self, proj);
                    return;
                }

                var oldType = proj.type;

                // Doesn't work with custom rendering among other things.
                try
                {
                    proj.type = vanityType;
                    orig(self, proj);
                }
                catch
                {
                    // Ignore any errors and just try to draw the original hook.
                    proj.type = oldType;
                    orig(self, proj);
                }
                finally
                {
                    proj.type = oldType;
                }
            };
        }
    }
#endregion

    void IInitializer.Load()
    {
        // Technically, we should be using ModSystem::ModifyInterfaceLayers to
        // render these UI slots.  HOWEVER: I don't care.  It's a hassle and I
        // want to ensure these are drawn as close to the inventory as possible.
        // Sorry, not sorry.
        On_Main.DrawInventory += DrawMiscVanitySlots;
        IL_Main.DrawInventory += il =>
        {
            var c = new ILCursor(il);

            var l = -1;
            c.GotoNext(MoveType.After, x => x.MatchLdcI4(-47));
            c.GotoPrev(x => x.MatchLdloc(out l));

            c.GotoNext(MoveType.After, x => x.MatchMul());
            c.EmitLdloc(l);
            c.EmitDelegate(
                (int lVal) => lVal == 1 ? 47 : 0
            );
            c.EmitSub();
        };
    }

    private static void DrawMiscVanitySlots(On_Main.orig_DrawInventory orig, Main self)
    {
        orig(self);

        if (Main.EquipPage != (int)EquipPageId.Misc)
        {
            return;
        }

        var oldScale = Main.inventoryScale;
        Main.inventoryScale = 0.85f;

        var mousePos = Main.MouseScreen.ToPoint();
        var panelRect = new Rectangle(
            0,
            0,
            (int)(TextureAssets.InventoryBack.Width()  * Main.inventoryScale),
            (int)(TextureAssets.InventoryBack.Height() * Main.inventoryScale)
        );

        var inv   = Main.LocalPlayer.GetModPlayer<MiscVanitySlotPlayer>().MiscVanity;
        var drawX = Main.screenWidth - 92;
        var drawY = Main.mH          + 174;

        panelRect.X = drawX - 47;

        for (var i = 0; i < 5; i++)
        {
            var vanitySlotId = (VanitySlotId)i;
            if (!HasMiscSlot(vanitySlotId))
            {
                continue;
            }

            var context = GetContextFromSlot(vanitySlotId);

            panelRect.Y = drawY + i * 47;

            if (panelRect.Contains(mousePos) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                Main.armorHide                  = true;

                ItemSlot.Handle(inv, context, i);
            }

            ItemSlot.Draw(Main.spriteBatch, inv, context, i, panelRect.TopLeft());
        }

        Main.inventoryScale = oldScale;
    }

    private static bool HasMiscSlot(VanitySlotId vanitySlotId)
    {
        return vanitySlotId switch
        {
            VanitySlotId.Pet      => false,
            VanitySlotId.LightPet => false,
            VanitySlotId.Minecart => true,
            VanitySlotId.Mount    => false,
            VanitySlotId.Hook     => true,
            _                     => false,
        };
    }

    private static int GetContextFromSlot(VanitySlotId vanitySlotId)
    {
        return vanitySlotId switch
        {
            VanitySlotId.Pet      => 19,
            VanitySlotId.LightPet => 20,
            VanitySlotId.Minecart => 18,
            VanitySlotId.Mount    => 17,
            VanitySlotId.Hook     => 16,
            _                     => 19,
        };
    }
}