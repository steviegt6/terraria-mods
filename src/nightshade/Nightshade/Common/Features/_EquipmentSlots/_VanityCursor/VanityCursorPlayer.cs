using System.Diagnostics;
using System.Linq;

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Nightshade.Common.Features;

public sealed class VanityCursorPlayer : ModPlayer
{
    private const string cursor_key = "Cursor";
    private const string trail_key = "Trail";

    public Item?[] Cursor = new Item?[2];
    public Item?[] Trail = new Item?[2];

    public override void Initialize()
    {
        base.Initialize();

        Cursor = [new Item(), new Item()];
        Trail = [new Item(), new Item()];
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
}