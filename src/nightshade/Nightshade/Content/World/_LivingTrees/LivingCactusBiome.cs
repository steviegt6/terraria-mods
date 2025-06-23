using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Nightshade.Common.Utilities;
using Nightshade.Content.Items;
using Nightshade.Content.Tiles;
using Nightshade.Content.Tiles.Furniture;
using Nightshade.Content.Walls;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Nightshade.Content.World;

internal sealed class LivingCactusBiome : MicroBiome
{
    private static ushort CactusType => (ushort)ModContent.TileType<LivingCactus>();

    private static ushort WoodType => (ushort)ModContent.TileType<LivingCactusWood>();

    private static ushort PlatformType => (ushort)ModContent.TileType<CactusWoodPlatform>();

    private static ushort WallType => (ushort)ModContent.WallType<LivingCactusWoodWall>();

	private static ushort PotType => (ushort)ModContent.TileType<LivingCactusPot>();

    public bool Round { get; set; }

    public bool WithWater { get; set; } = true;

    public override bool Place(Point origin, StructureMap? structures)
    {
        var height = WorldGen.genRand.Next(40, 55);
        var width = height / 2;

        if (Round)
        {
            height = WorldGen.genRand.Next(18, 22);
            width = (int)(height * WorldGen.genRand.NextFloat(0.8f, 1f));
		}

		if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(100), new Conditions.IsSolid().AreaAnd(6, 2), new Conditions.IsTile(TileID.Sand, TileID.HardenedSand, TileID.Sandstone)), out origin))
			return false;

		if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Up(50), new NightshadeGenUtil.Conditions.IsNotTile(TileID.Sand, TileID.HardenedSand, TileID.Sandstone)), out _))
			return false;

        if (GetSandCount(origin, 50, 50) < 250)
            return false;

		var cactusBounds = new Rectangle(origin.X - width / 2 - 5, origin.Y - height / 2 - 5, width + 10, height + 10);

        if (!structures?.CanPlace(cactusBounds) ?? false)
        {
            return false;
        }

        if (Round)
        {
            GenerateRoundLivingCactus(origin.X, origin.Y, width, height, WithWater);
        }
        else
        {
            GenerateTallLivingCactus(origin.X, origin.Y, height);
        }

        structures?.AddProtectedStructure(cactusBounds, 10);

        return true;
    }

    public static int GetSandCount(Point origin, int width, int height)
    {
        int sandCount = 0;

		for (int j = origin.Y - height; j < origin.Y + height; j++)
		{
			for (int i = origin.X - width; i < origin.X + width; i++)
			{
				if (!WorldGen.InWorld(i, j))
					continue;

                int tileType = Main.tile[i, j].TileType;

                if (TileID.Sets.isDesertBiomeSand[tileType])
                    sandCount++;
			}
		}

        return sandCount;
	}

    private static bool CanPlaceCactusOutline(int x, int y, int thickness, bool checkCorners)
    {
		var onOutside = Main.tile[x, y].WallType != WallType && Main.tile[x, y].TileType != WoodType;
        var onEdge = false;
        for (var i = x - thickness; i <= x + thickness; i++)
        {
            for (var j = y - thickness; j <= y + thickness; j++)
            {
                if (i == x && y == j)
                {
                    continue;
                }

                if (!WorldGen.InWorld(i, j))
                {
                    continue;
                }

                if (!checkCorners)
                {
                    if ((i == x - thickness && j == y - thickness) ||
                        (i == x + thickness && j == y - thickness) ||
                        (i == x - thickness && j == y + thickness) ||
                        (i == x + thickness && j == y + thickness))
                    {
                        continue;
                    }
                }

                if (Main.tile[i, j].TileType == WoodType)
                {
                    onEdge = true;
                }
            }
        }
        return onOutside && onEdge;
    }

    private static void GenerateRoundLivingCactus(int x, int y, int width, int height, bool withWater)
	{
		var curvature = WorldGen.genRand.Next(8, 15); // Higher is less curved
		var curveDir = WorldGen.genRand.NextBool().ToDirectionInt();
		var offsetX = 0;
		var stalkXOffsets = new int[height];
		var halfWidth = width / 2;

		for (var j = 0; j < height; j++)
		{
			if (j % curvature == 0 && j > height / 4 && j < height / 2)
			{
				offsetX += WorldGen.genRand.Next(0, 2) * curveDir;
			}

			stalkXOffsets[j] = offsetX;
		}

		for (var j = 0; j < height; j++)
		{
            var lowerCurveProgress = Utils.GetLerpValue(height * 0.7f, 0, j, true);

			for (var i = -halfWidth; i < halfWidth; i++)
			{
                var offX = stalkXOffsets[Math.Clamp(j - Math.Sign(i) * curveDir * (curvature / 3 + 1), 0, stalkXOffsets.Length - 1)];
				var centerX = x;
                var centerY = y - height / 2 + (int)MathHelper.Lerp(0, height / 8, Utils.GetLerpValue(height / 2, 0, j, true));

				if (!WorldGen.InWorld(x + i + offX, y - j))
				{
					continue;
				}

				var place = i <= -halfWidth || i >= halfWidth - 1;

                var normI = i + centerX - x;
                var normJ = j + centerY - y;
				double distance = MathF.Sqrt(normI * normI + normJ * normJ);
				var withinBound = distance < halfWidth;
				if (withinBound)
				{
					place = distance > halfWidth - 1.4;
				}
				else
				{
					continue;
				}

				if (place)
				{
					Main.tile[x + i + offX, y - j].ResetToType(WoodType);
				}
				else
				{
					Main.tile[x + i + offX, y - j].ClearTile();
				}

				Main.tile[x + i + offX, y - j].WallType = WallType;
			}
		}

		const int outer_thick = 2;
		for (var j = 0; j < height + outer_thick; j++)
		{
			for (var i = -halfWidth - outer_thick; i < halfWidth + outer_thick; i++)
			{
				var offX = stalkXOffsets[Math.Clamp(j - Math.Sign(i) * curveDir, 0, stalkXOffsets.Length - 1)];

				if (!WorldGen.InWorld(x + i + offX, y - j))
				{
					continue;
				}

				if (CanPlaceCactusOutline(x + i + offX, y - j, outer_thick, false))
				{
					Main.tile[x + i + offX, y - j].ResetToType(CactusType);
				}
			}
		}

		for (var i = -2; i < 3; i++)
		{
			GenerateRoot(x + i, y - 2, 2.6, 11.0, new Vector2(i, 3.5f / (Math.Abs(i) + 1)));
		}

        if (withWater)
        {
            WorldUtils.Gen(new Point(x, y - 5), new Shapes.HalfCircle(4), new Actions.SetLiquid());
        }

        PlaceLootChest(x, NightshadeGenUtil.GetNearestSurface(x, y - 5, 5), 2);
		PlacePotsEverywhere(x, y - height / 5, width * 2);
    }

	private static void GenerateTallLivingCactus(int x, int y, int height)
    {
        const int stalk_half_width = 4;
        var mainStalkCurvature = WorldGen.genRand.Next(5, 8); // Higher is less curved
        var stalkCurveDir = WorldGen.genRand.NextBool().ToDirectionInt();

        var offsetX = 0;
		var stalkXOffsets = new int[height];

        var timeUntilArm = WorldGen.genRand.Next(5, 20);
        var lastArmDirection = WorldGen.genRand.NextBool().ToDirectionInt();
		var arms = new List<Point>();

        for (var j = 0; j < height; j++)
        {
            if (j % mainStalkCurvature == 0 && j > 0 && j < height * 3 / 4)
            {
                offsetX += WorldGen.genRand.Next(0, 2) * stalkCurveDir;
            }

            stalkXOffsets[j] = offsetX;
        }

        for (var j = 0; j < height; j++)
        {
            timeUntilArm--;
            if (timeUntilArm == 0 && arms.Count < 2)
            {
                var armX = lastArmDirection > 0 ? x : x - 1;
                arms.Add(new Point(armX + stalkXOffsets[j] + WorldGen.genRand.Next(9, 14) * lastArmDirection, y - j));
                lastArmDirection *= -1;

                if (WorldGen.genRand.NextBool(3))
                {
                    timeUntilArm = WorldGen.genRand.Next(3, height / 3);
                }
            }

            for (var i = -stalk_half_width; i < stalk_half_width; i++)
            {
                var offX = stalkXOffsets[Math.Clamp(j - Math.Sign(i) * stalkCurveDir * (mainStalkCurvature / 3 + 1), 0, stalkXOffsets.Length - 1)];

                if (!WorldGen.InWorld(x + i + offX, y - j))
                {
                    continue;
                }

                var placeWide = offX != stalkXOffsets[j];

                var place = i is <= -stalk_half_width or >= stalk_half_width - 1;

                if (j > height - stalk_half_width)
                {
                    var normJ = j - height + stalk_half_width;
                    double distance = MathF.Sqrt((i + 1) * i + normJ * normJ);
                    var withinCap = distance < stalk_half_width - 0.5;
                    if (withinCap)
                    {
                        place = distance > stalk_half_width - 1.75;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (place)
                {
                    Main.tile[x + i + offX, y - j].ResetToType(WoodType);
                }
                else
                {
                    Main.tile[x + i + offX, y - j].ClearTile();
                }

                Main.tile[x + i + offX, y - j].WallType = WallType;

                if (placeWide)
                {
                    if (Main.tile[x + i + stalkXOffsets[j] - Math.Sign(i), y - j].TileType != WoodType)
                    {
                        Main.tile[x + i + stalkXOffsets[j] - Math.Sign(i), y - j].ClearTile();
                    }

                    Main.tile[x + i + stalkXOffsets[j] - Math.Sign(i), y - j].WallType = WallType;
                }
            }
        }

        PlaceLootChest(x + offsetX, y - height + stalk_half_width * 2, 4, true);

        const int stalk_outer_thick = 2;
        for (var j = 0; j < height + stalk_outer_thick; j++)
        {
            for (var i = -stalk_half_width - stalk_outer_thick; i < stalk_half_width + stalk_outer_thick; i++)
            {
                var offX = stalkXOffsets[Math.Clamp(j - Math.Sign(i) * stalkCurveDir, 0, stalkXOffsets.Length - 1)];

                if (!WorldGen.InWorld(x + i + offX, y - j))
                {
                    continue;
                }

                if (CanPlaceCactusOutline(x + i + offX, y - j, stalk_outer_thick, false))
                {
                    Main.tile[x + i + offX, y - j].ResetToType(CactusType);
                }
            }
        }

        foreach (var point in arms)
        {
            var armDirection = Math.Sign(point.X - x);
            var passageLength = Math.Abs(point.X - x);
            GenerateArm(point.X, point.Y, armDirection, passageLength, WorldGen.genRand.Next(15, 20));
        }

        for (var i = -2; i < 3; i++)
        {
            GenerateRoot(x + i, y, 2.3, 13.0, new Vector2(i, 2.5f / (Math.Abs(i) + 1)));
        }

        WorldUtils.Gen(new Point(x, y - 1), new Shapes.HalfCircle(4), new Actions.SetLiquid());

        PlacePotsEverywhere(x, y - height / 5, 32);
    }

    private static void GenerateArm(int x, int y, int direction, int passageLength, int armHeight)
    {
        var armCurveDir = WorldGen.genRand.NextBool().ToDirectionInt();
        var armCurvature = WorldGen.genRand.Next(4, 8);
        var offsetX = 0;
		var armXOffsets = new int[armHeight];

        const int arm_half_width = 3;
        const int passage_height = 5;

        for (var j = 0; j < armHeight; j++)
        {
            if (j % armCurvature == 0 && j > passage_height + 2 && j < armHeight * 3 / 4)
            {
                offsetX += WorldGen.genRand.Next(0, 2) * armCurveDir;
            }

            armXOffsets[j] = offsetX;
        }

        for (var j = 0; j < armHeight; j++)
        {
            if (j < passage_height)
            {
                for (var i = 1; i < passageLength - 1; i++)
                {
                    if (!WorldGen.InWorld(x - i * direction, y - j))
                    {
                        continue;
                    }

					var place = Math.Abs(j - passage_height / 2) > 1;
                    if (place && Main.tile[x - i * direction, y - j].WallType != WallType)
                    {
                        Main.tile[x - i * direction, y - j].ResetToType(WoodType);
                    }
                    else
                    {
                        if (!place)
                        {
                            Main.tile[x - i * direction, y - j].ClearTile();
                        }

                        Main.tile[x - i * direction, y - j].WallType = WallType;
                    }
                }
            }

            for (var i = -arm_half_width; i < arm_half_width; i++)
            {
                var offX = armXOffsets[Math.Clamp(j + 2 * Math.Sign(i) * armCurveDir, 0, armXOffsets.Length - 1)];

                if (!WorldGen.InWorld(x + i + offX, y - j))
                {
                    continue;
                }

				var placeWide = offX != armXOffsets[j];

				var place = i is <= -arm_half_width or >= arm_half_width - 1;

                if (j < arm_half_width)
                {
                    var normJ = j - arm_half_width;
                    double distance = MathF.Sqrt((i + 1) * i + normJ * normJ);
					var withinCap = distance < arm_half_width - 0.5;
                    if (withinCap)
                    {
                        place = distance > arm_half_width - 1.5;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (j > armHeight - arm_half_width)
                {
                    var normJ = j - armHeight + arm_half_width;
                    double distance = MathF.Sqrt((i + 1) * i + normJ * normJ);
                    var withinCap = distance < arm_half_width - 0.5;
                    if (withinCap)
                    {
                        place = distance > arm_half_width - 1.5;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (place && Main.tile[x + i + offX, y - j].WallType != WallType)
                {
                    Main.tile[x + i + offX, y - j].ResetToType(WoodType);
                }
                else
                {
                    Main.tile[x + i + offX, y - j].ClearTile();
                }

                Main.tile[x + i + offX, y - j].WallType = WallType;

                if (placeWide)
                {
                    if (Main.tile[x + i + armXOffsets[j], y - j].TileType != WoodType)
                    {
                        Main.tile[x + i + armXOffsets[j], y - j].ClearTile();
                    }

                    Main.tile[x + i + armXOffsets[j], y - j].WallType = WallType;
                }
            }
        }

        if (!WorldGen.genRand.NextBool(10))
        {
            TryPlacePlatform(x + offsetX, y - armHeight + arm_half_width * 2, -direction, arm_half_width);
            TryPlacePlatform(x + offsetX, y - armHeight + arm_half_width * 2, direction, arm_half_width);
            WorldGen.PlaceTile(x + offsetX - 1, y - armHeight + arm_half_width * 2 - 1, PotType, true);
        }

        const int arm_outer_thick = 2;
        for (var j = -arm_outer_thick; j < armHeight + arm_outer_thick; j++)
        {
            if (j < passage_height + arm_outer_thick)
            {
                for (var i = -1; i < passageLength; i++)
                {
                    if (!WorldGen.InWorld(x - i * direction, y - j))
                    {
                        continue;
                    }

                    if (CanPlaceCactusOutline(x - i * direction, y - j, arm_outer_thick, false))
                    {
                        Main.tile[x - i * direction, y - j].ResetToType(CactusType);
                    }
                }
            }

            for (var i = -arm_half_width - arm_outer_thick; i < arm_half_width + arm_outer_thick; i++)
            {
                var offX = armXOffsets[Math.Clamp(j + 2 * Math.Sign(i) * armCurveDir, 0, armXOffsets.Length - 1)];

                if (!WorldGen.InWorld(x + i + offX, y - j))
                {
                    continue;
                }

                if (CanPlaceCactusOutline(x + i + offX, y - j, arm_outer_thick, false))
                {
                    Main.tile[x + i + offX, y - j].ResetToType(CactusType);
                }
            }
        }

        // 8 = 2 * stalkHalfWidth, should be fine for varied size
        TryPlacePlatform(x - passageLength / 2 * direction, y, -direction);
    }

    private static void GenerateRoot(int x, int y, double size, double distance, Vector2 direction)
    {
        var xi = x - size / 2;
        var yi = y - size / 2;
        for (var k = 0; k < (int)distance; k++)
        {
            var thickness = (int)Math.Ceiling(Utils.Lerp(size, 0, Math.Sqrt(k / distance)));
            var thickX = thickness + (int)Math.Abs(direction.X);
            var thickY = thickness + (int)Math.Abs(direction.Y);
            for (var i = 0; i < thickX; i++)
            {
                for (var j = 0; j < thickY; j++)
                {
                    Main.tile[(int)xi + i, (int)yi + j].ResetToType(WoodType);

                    if (WorldGen.genRand.NextBool(10) && size > 1.0 && k > distance / 3.0)
                    {
                        GenerateRoot((int)xi + i, (int)yi + j, size / 3.0, distance / 3, new Vector2(direction.X * 0.5f, direction.Y));
                    }
                }
            }

            direction = direction.RotatedBy(WorldGen.genRand.NextFloat(-0.15f, 0.15f));

            xi += direction.X;
            yi += direction.Y;

            direction.Y *= 0.8f + 0.3f * (float)Math.Sqrt(k / distance);
        }
    }

    private static void TryPlacePlatform(int x, int y, int direction, int max = 8)
    {
        var valid = true;
        var solidCount = 0;
        var validCount = 0;
        var r = 0;
        while (true)
        {
            if (!Main.tile[x + r * direction, y].HasTile && Main.tile[x + r * direction, y].WallType == WallType)
            {
                validCount++;
            }
            else if (Main.tile[x + r * direction, y].HasTile && validCount < 1)
            {
                solidCount++;
            }
            else
            {
                break;
            }

            r++;
            if (validCount > max)
            {
                valid = false;
                break;
            }
        }

        if (!valid)
        {
            return;
        }

        for (var i = 0; i < validCount; i++)
        {
            if (!WorldGen.SolidTile(x + (i + solidCount) * direction, y))
            {
                Main.tile[x + (i + solidCount) * direction, y].ResetToType(PlatformType);
                WorldGen.SquareTileFrame(x + (i + solidCount) * direction, y);
            }
        }
    }

    private static void PlaceLootChest(int x, int y, int width = 4, bool platform = false)
    {
        if (platform)
        {
			TryPlacePlatform(x, y, 1, width / 2 + 1);
			TryPlacePlatform(x, y, -1, width / 2 + 1);
		}

		int index = WorldGen.PlaceChest(x - 1 + WorldGen.genRand.Next(-width / 2 + 1, width / 2), y - 1, style: 10);
        if (index < 0)
        {
            return;
        }

        List<Item> loot = new List<Item>();

        if (WorldGen.genRand.NextBool(4))
        {
            loot.Add(new Item(ModContent.ItemType<PreDigester>()));
        }
        else if (WorldGen.genRand.NextBool(10))
        {
            loot.Add(new Item(ItemID.Extractinator));
        }

        loot.Add(new Item(ModContent.ItemType<CactusSplashJug>(), WorldGen.genRand.Next(30, 50)));

        if (WorldGen.genRand.NextBool(10))
        {
            loot.Add(new Item(ItemID.CatBast));
        }

        if (WorldGen.genRand.NextBool())
        {
            if (GenVars.gold == 8)
            {
                loot.Add(new Item(ItemID.GoldBar, WorldGen.genRand.Next(5, 10)));
            }
            else if (GenVars.gold == 169)
            {
                loot.Add(new Item(ItemID.PlatinumBar, WorldGen.genRand.Next(5, 10)));
            }
            else // If we get here, then some mod adds a third ore alt. Cool!
            {
                loot.Add(new Item(ItemID.Diamond, WorldGen.genRand.Next(5, 10)));
            }
        }
        
        loot.Add(new Item(ItemID.Grenade, WorldGen.genRand.Next(30, 50)));

		if (WorldGen.genRand.NextBool())
        {
            loot.Add(new Item(ItemID.ScarabBomb, WorldGen.genRand.Next(5, 20)));
        }

        if (WorldGen.genRand.NextBool(5))
        {
            loot.Add(new Item(ItemID.ThornsPotion, WorldGen.genRand.Next(1, 4)));
        }
        else
        {
            loot.Add(new Item(ItemID.RecallPotion, WorldGen.genRand.Next(1, 5)));
        }

        if (WorldGen.genRand.NextBool())
        {
            loot.Add(new Item(ItemID.SilverCoin, WorldGen.genRand.Next(25, 90)));
        }

        NightshadeGenUtil.AddLootToChest(ref Main.chest[index], loot.ToArray());
	}

	private static void PlacePotsEverywhere(int x, int y, int radius)
    {
        for (var i = x - radius; i < x + radius; i++)
        {
            for (var j = y - radius; j < y + radius; j++)
            {
                if (WorldGen.genRand.NextBool(5))
                {
                    WorldGen.PlacePot(i, j, PotType);
                }
            }
        }
    }
}