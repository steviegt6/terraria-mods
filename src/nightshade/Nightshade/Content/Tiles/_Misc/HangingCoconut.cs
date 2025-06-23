using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nightshade.Content.Items._Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Nightshade.Content.Tiles._Misc;

public sealed class HangingCoconut : ModTile
{
	public override string Texture => Assets.Images.Tiles.Misc.HangingCoconut.KEY;

	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();

		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
 		Main.tileFrameImportant[Type] = true;

		TileID.Sets.MultiTileSway[Type] = true; 
		TileID.Sets.FallingBlockProjectile[Type] = new TileID.Sets.FallingBlockProjectileInfo(ProjectileID.MoonBoulder, 30);

		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
		TileObjectData.newTile.Width = 1;
		TileObjectData.newTile.Height = 1;
		TileObjectData.newTile.LavaDeath = true;
		TileObjectData.newTile.CoordinateWidth = 20;
		TileObjectData.newTile.CoordinateHeights = [24];
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.UsesCustomCanPlace = true;
		TileObjectData.newTile.DrawFlipHorizontal = true;
		TileObjectData.newTile.RandomStyleRange = 3;
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
		TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
		TileObjectData.newTile.DrawYOffset = -2;

		TileObjectData.addTile(Type);

		DustType = DustID.JunglePlants;
		AddMapEntry(new Color(107, 67, 34));
	}

	public override IEnumerable<Item> GetItemDrops(int i, int j) => [];

	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		DustType = DustID.JunglePlants;

		if (!fail && !effectOnly)
		{
			Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(),
				new Vector2(i * 16 + 8, j * 16 + 8),
				Vector2.Zero,
				ModContent.ProjectileType<FallingCoconutProjectile>(),
				TileID.Sets.FallingBlockProjectile[Type].FallingProjectileDamage,
				0f);
		}
	}

	public override void NumDust(int i, int j, bool fail, ref int num) => num = 2;

	public override void AdjustMultiTileVineParameters(int i, int j, ref float? overrideWindCycle, ref float windPushPowerX, ref float windPushPowerY, ref bool dontRotateTopTiles, ref float totalWindMultiplier, ref Texture2D glowTexture, ref Color glowColor)
	{
		base.AdjustMultiTileVineParameters(i, j, ref overrideWindCycle, ref windPushPowerX, ref windPushPowerY, ref dontRotateTopTiles, ref totalWindMultiplier, ref glowTexture, ref glowColor);
		DustType = DustID.JungleGrass;

		windPushPowerY = 0f;
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];

		if (TileObjectData.IsTopLeft(tile))
			Main.instance.TilesRenderer.AddSpecialPoint(i, j, TileDrawing.TileCounterType.MultiTileVine);

		return false;
	}
}

public sealed class FallingCoconutProjectile : ModProjectile
{
	public override string Texture => Assets.Images.Tiles.Misc.FallingCoconutProjectile.KEY;

	public override void SetDefaults()
	{
		base.SetDefaults();

		Projectile.aiStyle = -1;
		Projectile.width = 14;
		Projectile.height = 22;
		Projectile.hostile = true;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.tileCollide = true;
	}

	public override void AI()
	{
		base.AI();

		Projectile.velocity.Y += 0.4f;

		if (Projectile.velocity.Y > 20)
			Projectile.velocity.Y = 20;
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		base.OnHitPlayer(target, info);

		target.AddBuff(BuffID.Confused, 20);
		target.AddBuff(BuffID.Dazed, 120);

		Projectile.Kill();
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		base.OnHitNPC(target, hit, damageDone);

		target.AddBuff(BuffID.Confused, 20);
		target.AddBuff(BuffID.Dazed, 120);

		Projectile.Kill();
	}

	public override void OnKill(int timeLeft)
	{
		if (Main.netMode != NetmodeID.MultiplayerClient)
			Item.NewItem(Projectile.GetItemSource_FromThis(), Projectile.Center, ItemID.Coconut, noGrabDelay: true);

		for (int i = 0; i < 13; i++)
		{
			Color color = Main.rand.Next(ItemID.Sets.FoodParticleColors[ItemID.Coconut]);
			Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.FoodPiece, -Projectile.velocity.RotatedByRandom(2f) * Main.rand.NextFloat(0.5f), newColor: color, Scale: Main.rand.NextFloat());
		}

		SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

		int x = (int)(Projectile.Center.X / 16);
		int y = (int)(Projectile.Center.Y / 16) + 1;
		if (WorldGen.InWorld(x, y, 2))
		{
			for (int i = 0; i < 3; i++)
				WorldGen.KillTile_MakeTileDust(x, y, Main.tile[x, y]);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Rectangle frame = texture.Frame(1, 1, (int)Projectile.localAI[0]);
		Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, frame.Size() / 2, Projectile.scale, 0, 0);
		return false;
	}
}