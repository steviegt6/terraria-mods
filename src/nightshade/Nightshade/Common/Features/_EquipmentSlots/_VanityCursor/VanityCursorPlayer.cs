using Daybreak.Common.Features.Hooks;
using Daybreak.Common.Rendering;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nightshade.Common.Rendering;
using Nightshade.Core.Attributes;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
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
    private const string functional_visibility_key = "FunctionalVisibility";
    private const string dye_visibility_key = "DyeVisibility";

    public Item?[] Cursor = new Item?[2];
    public Item?[] Trail = new Item?[2];

    public int HairDye { get; set; }

    public CursorVisibility FunctionalVisibility { get; set; } = CursorVisibility.Cursor;

    public CursorVisibility DyeVisibility { get; set; } = CursorVisibility.Both;

    public override void Load()
    {
        base.Load();

        Main.QueueMainThreadAction(
            () =>
            {
                var gd = Main.instance.GraphicsDevice;
                cursorTarget = new RenderTarget2D(gd, 60, 60, false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
                thickCursorTarget = new RenderTarget2D(gd, 60, 60, false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            }
        );

        On_Main.DrawThickCursor += DrawThickCursor_WithEffects;
        On_Main.DrawCursor += DrawCursor_WithEffects;
    }

    [InitializedInLoad]
    private static RenderTarget2D? cursorTarget;

    [InitializedInLoad]
    private static RenderTarget2D? thickCursorTarget;

    private static void DrawCursorTarget(GraphicsDevice device, SpriteBatch spriteBatch, bool smart, Color color)
    {
        var bindings = RtContentPreserver.GetAndPreserveMainRTs();

        device.SetRenderTarget(cursorTarget);
        device.Clear(Color.Transparent);

        var asset = TextureAssets.Cursors[smart ? 1 : 0].Value;
        spriteBatch.End(out var ss);
        spriteBatch.Begin(ss with { SortMode = SpriteSortMode.Deferred });
        spriteBatch.Draw(asset, Vector2.Zero, color);
        spriteBatch.End();

        device.SetRenderTargets(bindings);
        spriteBatch.Begin(ss);
    }

    private static void DrawThickCursorTarget(GraphicsDevice device, SpriteBatch spriteBatch, bool smart, Color color)
    {
        var bindings = RtContentPreserver.GetAndPreserveMainRTs();

        device.SetRenderTarget(thickCursorTarget);
        device.Clear(Color.Transparent);

        var asset = TextureAssets.Cursors[smart ? 12 : 11].Value;
        spriteBatch.End(out var ss);
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.SamplerStateForCursor, DepthStencilState.None, Main.Rasterizer, null);
        spriteBatch.Draw(asset, Vector2.Zero, color);
        spriteBatch.End();

        device.SetRenderTargets(bindings);
        spriteBatch.Begin(ss);
    }

    private static Vector2 DrawThickCursor_WithEffects(On_Main.orig_DrawThickCursor orig, bool smart)
    {
        if (Main.gameMenu)
        {
            return orig(smart);
        }

        if (!Main.ThickMouse || PlayerInput.SettingsForUI.ShowGamepadCursor)
        {
            return orig(smart);
        }

        try
        {
            if (!ApplyEffect(CursorVisibility.Outline, out var shader))
            {
                return orig(smart);
            }

            if (Main.gameMenu && Main.alreadyGrabbingSunOrMoon)
            {
                return Vector2.Zero;
            }

            var mouseBorderColor = Main.MouseBorderColor;

            var player = Main.LocalPlayer.GetModPlayer<VanityCursorPlayer>();
            var effectApplies = player.FunctionalVisibility is CursorVisibility.Outline or CursorVisibility.Both;
            
            if (Main.LocalPlayer.hasRainbowCursor && player.FunctionalVisibility is CursorVisibility.Outline or CursorVisibility.Both)
            {
                mouseBorderColor = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.25f % 1f, 1f, 0.5f);
            }

            if (shader.HasValue)
            {
                PlayerDrawHelper.UnpackShader(shader.Value, out var localShaderIndex, out var shaderType);
                if (shaderType == PlayerDrawHelper.ShaderConfiguration.HairShader && player.FunctionalVisibility is CursorVisibility.Outline or CursorVisibility.Both)
                {
                    var oldHairColor = Main.LocalPlayer.hairColor;
                    Main.LocalPlayer.hairColor = Color.White;
                    mouseBorderColor = GameShaders.Hair.GetColor(localShaderIndex, Main.LocalPlayer, Color.White);
                    Main.LocalPlayer.hairColor = oldHairColor;
                }
            }

            DrawThickCursorTarget(Main.instance.GraphicsDevice, Main.spriteBatch, smart, mouseBorderColor);

            for (var i = 0; i < 4; i++)
            {
                var offset = i switch
                {
                    0 => new Vector2(0f, 1f),
                    1 => new Vector2(1f, 0f),
                    2 => new Vector2(0f, -1f),
                    3 => new Vector2(-1f, 0f),
                    _ => Vector2.Zero,
                };

                // offset *= 1f;
                offset += Vector2.One * 2f;

                var origin = new Vector2(2f);
                var sourceRectangle = default(Rectangle?);
                var scale = Main.cursorScale * 1.1f;

                var cursorAsset = thickCursorTarget;
                
                var shaderValue = shader ?? 0;
                Draw(
                    new DrawData(
                        cursorAsset,
                        new Vector2(Main.mouseX, Main.mouseY) + offset,
                        sourceRectangle,
                        Color.White,
                        0f,
                        origin,
                        scale,
                        SpriteEffects.None
                    )
                    {
                        shader = shaderValue,
                    }
                );
            }
            return Vector2.One * 2f;
        }
        finally
        {
            UnapplyEffect();
        }
    }

    private static void DrawCursor_WithEffects(On_Main.orig_DrawCursor orig, Vector2 bonus, bool smart)
    {
        if (Main.gameMenu)
        {
            orig(bonus, smart);
            return;
        }

        if (PlayerInput.SettingsForUI.ShowGamepadCursor)
        {
            orig(bonus, smart);
            return;
        }

        try
        {
            if (!ApplyEffect(CursorVisibility.Cursor, out var shader))
            {
                orig(bonus, smart);
                return;
            }

            if (Main.LocalPlayer.dead || Main.LocalPlayer.mouseInterface)
            {
                Main.ClearSmartInteract();
                Main.TileInteractionLX = -1;
                Main.TileInteractionHX = -1;
                Main.TileInteractionLY = -1;
                Main.TileInteractionHY = -1;
            }

            var color = Main.cursorColor;

            var player = Main.LocalPlayer.GetModPlayer<VanityCursorPlayer>();
            if (Main.LocalPlayer.hasRainbowCursor && player.FunctionalVisibility is CursorVisibility.Cursor or CursorVisibility.Both)
            {
                color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.25f % 1f, 1f, 0.5f);
            }

            if (shader.HasValue)
            {
                PlayerDrawHelper.UnpackShader(shader.Value, out var localShaderIndex, out var shaderType);
                if (shaderType == PlayerDrawHelper.ShaderConfiguration.HairShader && player.FunctionalVisibility is CursorVisibility.Cursor or CursorVisibility.Both)
                {
                    var oldHairColor = Main.LocalPlayer.hairColor;
                    Main.LocalPlayer.hairColor = Color.White;
                    color = GameShaders.Hair.GetColor(localShaderIndex, Main.LocalPlayer, Color.White);
                    Main.LocalPlayer.hairColor = oldHairColor;
                }
            }

            DrawCursorTarget(Main.instance.GraphicsDevice, Main.spriteBatch, smart, color);

            var cursorTargetAsset = cursorTarget;

            var shaderValue = shader ?? 0;
            Draw(
                new DrawData(
                    cursorTargetAsset,
                    new Vector2(Main.mouseX, Main.mouseY) + bonus + Vector2.One,
                    null,
                    Color.White,
                    0f,
                    default(Vector2),
                    Main.cursorScale * 1.1f,
                    SpriteEffects.None
                )
                {
                    shader = shaderValue,
                }
            );

            Draw(
                new DrawData(
                    cursorTargetAsset,
                    new Vector2(Main.mouseX, Main.mouseY) + bonus,
                    null,
                    Color.White,
                    0f,
                    default(Vector2),
                    Main.cursorScale,
                    SpriteEffects.None
                )
                {
                    shader = shaderValue,
                }
            );
        }
        finally
        {
            UnapplyEffect();
        }
    }

    private static void Draw(DrawData drawData)
    {
        var dd = drawData;
        dd.sourceRect ??= dd.texture.Frame();

        var oldHead = Main.LocalPlayer.head;
        Main.LocalPlayer.head = Main.LocalPlayer.head != 0 ? Main.LocalPlayer.head : 1;
        {
            PlayerDrawHelper.SetShaderForData(Main.LocalPlayer, 0, ref dd);
        }
        Main.LocalPlayer.head = oldHead;

        if (dd.texture is not null)
        {
            dd.Draw(Main.spriteBatch);
        }
    }

    // private static DrawData ApplyFlatColoredData()

    private static SpriteBatchSnapshot? snapshot;

    private static bool ApplyEffect(CursorVisibility mode, out int? shader)
    {
        // Could be in the main menu, etc.
        if (!Main.LocalPlayer.TryGetModPlayer<VanityCursorPlayer>(out var player))
        {
            shader = null;
            return false;
        }

        var hasHairDye = player.HairDye > -1;
        // var hasDye = player.Cursor[1] is { IsAir: false, dye: > 0 };

        Main.spriteBatch.End(out var ss);
        {
            snapshot = ss;
        }

        Main.spriteBatch.Begin(snapshot.Value with { SortMode = SpriteSortMode.Immediate });

        /*var fakeDrawData = new DrawData
        {
            sourceRect = new Rectangle(0, 0, 24, 24),
            position = Main.MouseScreen,
            texture = TextureAssets.Cursors[0].Value,
        };*/

        // TODO: Merge with dye-stacking system when we make it.

        if (hasHairDye && (player.FunctionalVisibility == mode || player.FunctionalVisibility == CursorVisibility.Both))
        {
            shader = PlayerDrawHelper.PackShader(player.HairDye, PlayerDrawHelper.ShaderConfiguration.HairShader);
        }
        else if (player.Cursor[1] is { IsAir: false, dye: > 0 } dyeItem && (player.DyeVisibility == mode || player.DyeVisibility == CursorVisibility.Both))
        {
            shader = PlayerDrawHelper.PackShader(dyeItem.dye, PlayerDrawHelper.ShaderConfiguration.ArmorShader);
        }
        else
        {
            shader = null;
        }

        return true;
    }

    private static void UnapplyEffect()
    {
        if (snapshot.HasValue)
        {
            Main.spriteBatch.Restart(snapshot.Value);
            snapshot = null;
        }

        // Main.cursorColor = origCursorColor;
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }

    public override void Initialize()
    {
        base.Initialize();

        Cursor = [new Item(), new Item()];
        Trail = [new Item(), new Item()];

        HairDye = -1;

        FunctionalVisibility = CursorVisibility.Cursor;
        DyeVisibility = CursorVisibility.Both;
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

        tag.Add(functional_visibility_key, (byte)FunctionalVisibility);
        tag.Add(dye_visibility_key, (byte)DyeVisibility);
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

        FunctionalVisibility = (CursorVisibility)tag.GetByte(functional_visibility_key);
        DyeVisibility = (CursorVisibility)tag.GetByte(dye_visibility_key);
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