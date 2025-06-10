using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace Nightshade.Content.Tiles;

// TODO: Dust, name, other properties.

internal sealed class CoconutChestTile : AbstractChest
{
    public override string Texture => Assets.Images.Tiles.Furniture.CoconutChestTile.KEY;

	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();

		Main.tileShine2[Type] = false;

		AddMapEntry(new Color(221, 205, 201), Language.GetOrRegister($"Tiles.{GetType().Name}"));
	}
}