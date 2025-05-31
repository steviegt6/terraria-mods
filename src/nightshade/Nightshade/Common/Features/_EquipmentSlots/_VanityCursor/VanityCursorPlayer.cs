using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using Daybreak.Common.Features.Hooks;
using Daybreak.Common.Rendering;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.IO;

namespace Nightshade.Common.Features;

public sealed class VanityCursorPlayer : ModPlayer
{
    internal static class NetHandler
    {
        public static void SendSlot(int toWho, int plr, int slot, Item item)
        {
            var p = ModImpl.GetPacket(ModLoaderMod.AccessorySlotPacket);

            if (Main.netMode == NetmodeID.Server)
            {
                p.Write((byte)plr);
            }

            p.Write((byte)slot);

            ItemIO.Send(item, p, true);
            p.Send(toWho, plr);
        }

        private static void HandleSlot(BinaryReader r, int fromWho)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                fromWho = r.ReadByte();
            }

            var player = Main.player[fromWho].GetModPlayer<VanityCursorPlayer>();

            var slot = r.ReadByte();
            var item = ItemIO.Receive(r, true);

            SetSlot(slot, item, player);

            if (Main.netMode == NetmodeID.Server)
            {
                SendSlot(-1, fromWho, slot, item);
            }
        }

        public static void HandlePacket(BinaryReader r, int fromWho)
        {
            HandleSlot(r, fromWho);
        }

        public static void SetSlot(byte slot, Item item, VanityCursorPlayer player)
        {
            switch (slot)
            {
                case 0:
                    player.Cursor[0] = item;
                    break;

                case 1:
                    player.Cursor[1] = item;
                    break;

                case 2:
                    player.Trail[0] = item;
                    break;

                case 3:
                    player.Trail[1] = item;
                    break;
            }
        }
    }

    private const string cursor_key = "Cursor";
    private const string trail_key = "Trail";

    public Item?[] Cursor = new Item?[2];
    public Item?[] Trail = new Item?[2];

    public int HairDye { get; set; }

    public override void Load()
    {
        base.Load();

        On_Main.DrawThickCursor += DrawThickCursorWithEffects;
        On_Main.DrawCursor += DrawCursorWithEffects;
    }

    private static Vector2 DrawThickCursorWithEffects(On_Main.orig_DrawThickCursor orig, bool smart)
    {
        ApplyEffect();
        var res = orig(smart);
        UnapplyEffect();

        return res;
    }

    private static void DrawCursorWithEffects(On_Main.orig_DrawCursor orig, Vector2 bonus, bool smart)
    {
        ApplyEffect();
        orig(bonus, smart);
        UnapplyEffect();
    }

    private static Color origCursorColor;
    private static SpriteBatchSnapshot? snapshot;

    private static void ApplyEffect()
    {
        origCursorColor = Main.cursorColor;

        // Could be in the main menu, etc.
        if (!Main.LocalPlayer.TryGetModPlayer<VanityCursorPlayer>(out var player))
        {
            return;
        }

        var hasHairDye = player.HairDye > -1;
        var hasDye = player.Cursor[1] is { IsAir: false, dye: > 0 };

        if (hasHairDye || hasDye)
        {
            Main.spriteBatch.End(out var ss);
            {
                snapshot = ss;
            }

            Main.spriteBatch.Begin(snapshot.Value with { SortMode = SpriteSortMode.Immediate });
        }

        var fakeDrawData = new DrawData
        {
            sourceRect = new Rectangle(0, 0, 24, 24),
            position = Main.MouseScreen,
            texture = TextureAssets.Cursors[0].Value,
        };

        if (hasHairDye)
        {
            var oldHairColor = Main.LocalPlayer.hairColor;
            Main.LocalPlayer.hairColor = Color.White;
            Main.cursorColor = GameShaders.Hair.GetColor(player.HairDye, Main.LocalPlayer, Color.White);
            Main.LocalPlayer.hairColor = oldHairColor;
            GameShaders.Hair.Apply(player.HairDye, Main.LocalPlayer, fakeDrawData);
        }

        if (player.Cursor[1] is { IsAir: false, dye: > 0 } dyeItem)
        {
            GameShaders.Armor.Apply(dyeItem.dye, Main.LocalPlayer, fakeDrawData);
        }
    }

    private static void UnapplyEffect()
    {
        if (snapshot.HasValue)
        {
            Main.spriteBatch.Restart(snapshot.Value);
            snapshot = null;
        }

        Main.cursorColor = origCursorColor;
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }

    public override void Initialize()
    {
        base.Initialize();

        Cursor = [new Item(), new Item()];
        Trail = [new Item(), new Item()];

        HairDye = -1;
    }

    public override void ResetEffects()
    {
        base.ResetEffects();

        HairDye = -1;
    }

    public override void SaveData(TagCompound tag)
    {
        base.SaveData(tag);

        var cursor = Cursor.Select(ItemIO.Save).ToArray();
        tag.Add(cursor_key, cursor);

        var trail = Trail.Select(ItemIO.Save).ToArray();
        tag.Add(trail_key, trail);
    }

    public override void LoadData(TagCompound tag)
    {
        base.LoadData(tag);

        var cursor = tag.GetList<TagCompound>(cursor_key);
        for (var i = 0; i < cursor.Count; i++)
        {
            Debug.Assert(cursor.Count > i, "Cursor vanity length is less than the number of items in the tag");

            Cursor[i] = ItemIO.Load(cursor[i]);
        }

        var trail = tag.GetList<TagCompound>(trail_key);
        for (var i = 0; i < trail.Count; i++)
        {
            Debug.Assert(trail.Count > i, "Trail vanity length is less than the number of items in the tag");

            Trail[i] = ItemIO.Load(trail[i]);
        }
    }

    public override void UpdateVisibleAccessories()
    {
        base.UpdateVisibleAccessories();

        foreach (var item in GetItems())
        {
            Player.UpdateVisibleAccessory(0, item, modded: true);
        }
    }

    public override void UpdateVisibleVanityAccessories()
    {
        base.UpdateVisibleVanityAccessories();

        foreach (var item in GetItems())
        {
            if (!Player.ItemIsVisuallyIncompatible(item))
            {
                Player.UpdateVisibleAccessory(0, item, modded: true);
            }
        }
    }

    public override void UpdateDyes()
    {
        base.UpdateDyes();

        if (Cursor[0] is not null && Cursor[1] is { IsAir: false })
        {
            Player.UpdateItemDye(isNotInVanitySlot: true, isSetToHidden: false, Cursor[0], Cursor[1]);
        }

        if (Trail[0] is not null && Trail[1] is { IsAir: false })
        {
            Player.UpdateItemDye(isNotInVanitySlot: true, isSetToHidden: false, Trail[0], Trail[1]);
        }
    }

    public override void UpdateEquips()
    {
        base.UpdateEquips();

        foreach (var item in GetItems())
        {
            if (item.accessory)
            {
                Player.GrantPrefixBenefits(item);
            }

            Player.GrantArmorBenefits(item);
            Player.ApplyEquipFunctional(item, hideVisual: false);
            // Player.ApplyEquipVanity(item);
        }
    }

    public void DropItems(IEntitySource itemSource)
    {
        var pos = Player.position + Player.Size / 2f;
        Player.DropItem(itemSource, pos, ref Cursor[0]);
        Player.DropItem(itemSource, pos, ref Cursor[1]);
        Player.DropItem(itemSource, pos, ref Trail[0]);
        Player.DropItem(itemSource, pos, ref Trail[1]);
    }

    public override void CopyClientState(ModPlayer targetCopy)
    {
        base.CopyClientState(targetCopy);

        if (targetCopy is not VanityCursorPlayer defaultInv)
        {
            return;
        }

        for (var i = 0; i < 4; i++)
        {
            GetItem(i).CopyNetStateTo(defaultInv.GetItem(i));
        }
    }

    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
        base.SyncPlayer(toWho, fromWho, newPlayer);

        for (var i = 0; i < 4; i++)
        {
            NetHandler.SendSlot(toWho, Player.whoAmI, i, GetItem(i));
        }
    }

    public override void SendClientChanges(ModPlayer clientPlayer)
    {
        base.SendClientChanges(clientPlayer);
        
        if (clientPlayer is not VanityCursorPlayer clientInv)
        {
            return;
        }

        for (var i = 0; i < 4; i++)
        {
            if (GetItem(i).IsNetStateDifferent(clientInv.GetItem(i)))
            {
                NetHandler.SendSlot(-1, Player.whoAmI, i, GetItem(i));
            }
        }
    }

    private IEnumerable<Item> GetItems()
    {
        yield return Cursor[0]!;
        yield return Trail[0]!;
    }

    private IEnumerable<Item> GetAllItems()
    {
        yield return Cursor[0]!;
        yield return Cursor[1]!;
        yield return Trail[0]!;
        yield return Trail[1]!;
    }

    private ref Item GetItem(int i)
    {
        switch (i)
        {
            case 0:
                return ref Cursor[0]!;

            case 1:
                return ref Cursor[1]!;

            case 2:
                return ref Trail[0]!;

            case 3:
                return ref Trail[1]!;
        }

        throw new Exception();
    }

    [OnLoad]
    private static void HookDropItems()
    {
        MonoModHooks.Add(
            typeof(ModAccessorySlotPlayer).GetMethod(nameof(ModAccessorySlotPlayer.DropItems), BindingFlags.Public | BindingFlags.Instance),
            (Action<ModAccessorySlotPlayer, IEntitySource> orig, ModAccessorySlotPlayer self, IEntitySource itemSource) =>
            {
                orig(self, itemSource);

                self.Player.GetModPlayer<VanityCursorPlayer>().DropItems(itemSource);
            }
        );
    }
}