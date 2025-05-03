using Nightshade.Common.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace Nightshade.Content.Tiles;

[Autoload(false)]
internal sealed class MonsterBannerItem : ModItem
{
    [Autoload(false)]
    private sealed class MonsterBannerTile : ModBannerTile
    {
        private readonly string _texture;
        private readonly string _name;

        public override string Texture => _texture;
        public override string Name => _name;

        public MonsterBannerTile(string name, string texture)
        {
            _name = name;
            _texture = texture;
        }
    }

    public override bool CloneNewInstances => true;

    [CloneByReference]
    private readonly string _texture;
    [CloneByReference]
    private readonly string _name;
    [CloneByReference]
    private readonly string _tileName;
    [CloneByReference]
    private readonly int _npcType;
    [CloneByReference]
    private readonly int _tileID;


    public override string Texture => _texture;
    public override string Name => _name;
    public override LocalizedText DisplayName => Language.GetText($"Mods.Nightshade.NPCs.{ModContent.GetModNPC(_npcType)?.Name}.BannerDisplayName");
    public override LocalizedText Tooltip => Language.GetText("Mods.Nightshade.Items.Banners.TooltipDefault").WithFormatArgs(ModContent.GetModNPC(_npcType)?.DisplayName);

    public MonsterBannerItem(string name, string tileName, int npcType, string texture, int tileID)
    {
        _name = name;
        _tileName = tileName;
        _npcType = npcType;
        _texture = texture;
        _tileID = tileID;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(_tileID);
        Item.width = 10;
        Item.height = 24;
        Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
    }

    public static int RegisterMonsterBanner(Mod mod, string name, int npcType, string itemTexture, string tileTexture)
    {
        int bannerTileType = TileLoader.TileCount;
        int bannerItemType = ItemLoader.ItemCount;

        mod.AddContent(new MonsterBannerItem(name + "Item", name + "Tile", npcType, itemTexture, bannerTileType));
        mod.AddContent(new MonsterBannerTile(name + "Tile", tileTexture));

        return bannerItemType;
    }
}

/*
public interface IHasMonsterBanner
{
    string ItemTexture { get; }
}

internal sealed class MonsterBannerInitializer : IInitializer
{
    void IInitializer.Load()
    {
        Mod mod = ModLoader.GetMod("NIGHTSHADE");
        IEnumerable<Type> types = AssemblyManager.GetLoadableTypes(mod.Code)
            .Where(t => t.IsAssignableTo(typeof(IHasMonsterBanner)));
        foreach (Type type in types)
        {
            
        }
    }
}
*/