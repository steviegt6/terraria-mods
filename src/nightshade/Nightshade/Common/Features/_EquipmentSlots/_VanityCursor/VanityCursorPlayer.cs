using System.Diagnostics;
using System.Linq;

using Daybreak.Common.Rendering;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Nightshade.Common.Features;

public sealed class VanityCursorPlayer : ModPlayer
{
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

    public override void UpdateVisibleVanityAccessories()
    {
        base.UpdateVisibleVanityAccessories();

        foreach (var item in new[] { Cursor[0], Trail[0] })
        {
            if (!Player.ItemIsVisuallyIncompatible(item))
            {
                Player.UpdateVisibleAccessory(0, item, modded: true);
            }
        }
    }
}