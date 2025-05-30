using System.Diagnostics;
using System.Linq;

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Nightshade.Common.Features;

public sealed class VanityCursorPlayer : ModPlayer
{
    private const string vanity_cursor_key = "VanityCursor";

    public Item?[] Vanity = new Item?[3];

    public override void Initialize()
    {
        base.Initialize();

        Vanity = [new Item(), new Item(), new Item()];
    }

    public override void SaveData(TagCompound tag)
    {
        base.SaveData(tag);

        var vanity = Vanity.Select(ItemIO.Save).ToArray();
        tag.Add(vanity_cursor_key, vanity);
    }

    public override void LoadData(TagCompound tag)
    {
        base.LoadData(tag);

        var vanity = tag.GetList<TagCompound>(vanity_cursor_key);
        for (var i = 0; i < vanity.Count; i++)
        {
            Debug.Assert(vanity.Count > i, "Cursor vanity length is less than the number of items in the tag");
            
            Vanity[i] = ItemIO.Load(vanity[i]);
        }
    }
}