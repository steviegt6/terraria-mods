using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using ReLogic.Threading;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

using Tomat.TML.Mod.NotQuiteNitrate.Utilities;

namespace Tomat.TML.Mod.NotQuiteNitrate.Patches;

/// <summary>
///     Removes branches where possible in scanning to improve CPU prediction
///     speeds.
/// </summary>
internal sealed class BranchlessSceneMetrics : ModSystem
{
    private static readonly int[] fountain_map =
    [
        0,
        12,
        3,
        5,
        2,
        10,
        4,
        9,
        8,
        6,
    ];

    public override void Load()
    {
        base.Load();

        On_SceneMetrics.ScanAndExportToMain += ScanAndExportToMain;
    }

    private static void ScanAndExportToMain(
        On_SceneMetrics.orig_ScanAndExportToMain orig,
        SceneMetrics                             self,
        SceneMetricsScanSettings                 settings
    )
    {
        self.Reset();

        var waterCandleCount  = 0;
        var peaceCandleCount  = 0;
        var shadowCandleCount = 0;

        if (settings.ScanOreFinderData)
        {
            self._oreFinderTileLocations.Clear();
        }

        SystemLoader.ResetNearbyTileEffects();

        if (settings.BiomeScanCenterPositionInWorld.HasValue)
        {
            var biomeScanCenter = settings.BiomeScanCenterPositionInWorld.Value.ToTileCoordinates();

            var biomeScanArea = new Rectangle(
                biomeScanCenter.X - Main.buffScanAreaWidth  / 2,
                biomeScanCenter.Y - Main.buffScanAreaHeight / 2,
                Main.buffScanAreaWidth,
                Main.buffScanAreaHeight
            );
            {
                biomeScanArea = WorldUtils.ClampToWorld(biomeScanArea);
            }

            for (var x = biomeScanArea.Left; x < biomeScanArea.Right; x++)
            for (var y = biomeScanArea.Top; y < biomeScanArea.Bottom; y++)
            {
                // if (!biomeScanArea.Contains(x, y))
                // {
                //     continue;
                // }

                var tile = Main.tile[x, y];

                // self._liquidCounts[tile.LiquidType] += Bool.ToByte(!tile.HasTile) & Bool.ToByte(tile.liquid > 0);

                if (!tile.HasTile)
                {
                    if (tile.liquid > 0)
                    {
                        self._liquidCounts[tile.LiquidType]++;
                    }

                    continue;
                }

                self._tileCounts[tile.type] += Bool.ToByte(!TileID.Sets.isDesertBiomeSand[tile.type]) | Bool.ToByte(!WorldGen.oceanDepths(x, y));

                self.HasCampfire |= tile is { type: 215, frameY: < 36 };

                waterCandleCount += Bool.ToByte(tile is { type: 49, frameX: < 18 });

                peaceCandleCount += Bool.ToByte(tile is { type: 372, frameX: < 18 });

                shadowCandleCount += Bool.ToByte(tile is { type: 646, frameX: < 18 });

                self.HasCampfire |= tile is { type: 405, frameX: < 54 };

                self.HasCatBast |= tile is { type: 506, frameX: < 72 };

                self.HasHeartLantern |= tile is { type: 42, frameY: >= 324 and <= 358 };

                self.HasStarInBottle |= tile is { type: 42, frameY: >= 252 and <= 286 };

                if (tile.type == TileID.Banners && (tile.frameX >= 396 || tile.frameY >= 54))
                {
                    var bannerFrame = tile.frameX / 18 - 21;
                    for (int i = tile.frameY; i >= 54; i -= 54)
                    {
                        bannerFrame += 90;
                        bannerFrame += 21;
                    }

                    var bannerItemId = Item.BannerToItem(bannerFrame);
                    if (ItemID.Sets.BannerStrength.IndexInRange(bannerItemId) && ItemID.Sets.BannerStrength[bannerItemId].Enabled)
                    {
                        // Already thread-safe.
                        self.NPCBannerBuff[bannerFrame] = true;

                        // Already thread-safe.
                        self.hasBanner = true;
                    }
                }

                if (settings.ScanOreFinderData && Main.tileOreFinderPriority[tile.type] != 0)
                {
                    self._oreFinderTileLocations.Add(new Point(x, y));
                }

                TileLoader.NearbyEffects(x, y, tile.type, false);
            }
        }

        if (settings.VisualScanArea.HasValue)
        {
            var visualScanArea = WorldUtils.ClampToWorld(settings.VisualScanArea.Value);

            for (var x = visualScanArea.Left; x < visualScanArea.Right; x++)
            for (var y = visualScanArea.Top; y < visualScanArea.Bottom; y++)
            {
                var tile = Main.tile[x, y];

                if (!tile.HasTile)
                {
                    continue;
                }

                self.HasClock |= TileID.Sets.Clock[tile.type];

                // var musicBoxCondition = Bool.ToByte(tile is { type: 139, frameX: >= 36 });
                // self.ActiveMusicBox = musicBoxCondition * (tile.frameY / 36) + (1 - musicBoxCondition) * self.ActiveMusicBox;
                var musicBoxCond = Bool.ToByte(tile.type == 139) & Bool.ToByte(tile.frameX >= 36);
                {
                    self.ActiveMusicBox = musicBoxCond * (tile.frameY / 36) + (1 - musicBoxCond) * self.ActiveMusicBox;
                }

                var fountainCond = Bool.ToByte(tile.type == 207) & Bool.ToByte(tile.frameY >= 72);
                var fountainIdx  = tile.frameX / 36;
                {
                    self.ActiveFountainColor = (fountainCond & Bool.ToByte(fountainIdx >= 0) & Bool.ToByte(fountainIdx < fountain_map.Length)) * fountain_map[fountainIdx * fountainCond]
                                             + (~fountainCond & self.ActiveFountainColor)
                                             + (fountainCond & ~(Bool.ToByte(fountainIdx >= 0) & Bool.ToByte(fountainIdx < fountain_map.Length))) * -1;
                }

                var monolithCond410 = Bool.ToByte(tile.type == 410) & Bool.ToByte(tile.frameY >= 56);
                var monolithCond509 = Bool.ToByte(tile.type == 509) & Bool.ToByte(tile.frameY >= 56);
                var monolithCond480 = Bool.ToByte(tile.type == 480) & Bool.ToByte(tile.frameY >= 54);
                var monolithCond657 = Bool.ToByte(tile.type == 657) & Bool.ToByte(tile.frameY >= 54);
                {
                    self.ActiveMonolithType = monolithCond410 * (tile.frameX / 36)
                                            + monolithCond509 * 4
                                            + monolithCond480 * 0
                                            + monolithCond657 * 0
                                            + ~(monolithCond410
                                              | monolithCond509
                                              | monolithCond480
                                              | monolithCond657) * self.ActiveMonolithType;
                }

                // self.BloodMoonMonolith = (monolithCond480 * 1) + (~monolithCond480 * self.BloodMoonMonolith);

                // self.EchoMonolith = (monolithCond657 * 1) + (~monolithCond657 * self.EchoMonolith);

                self.ShimmerMonolithState = Bool.ToByte(tile.type == 658) * (tile.frameY / 54) + Bool.ToByte(tile.type != 658) * self.ShimmerMonolithState;

                self.BloodMoonMonolith |= tile is { type: 480, frameY: >= 54 };

                self.EchoMonolith |= tile is { type: 657, frameY: >= 54 };

                // self.ShimmerMonolithState = tile is { type: 658, }

                if (MusicLoader.tileToMusic.ContainsKey(tile.type) && MusicLoader.tileToMusic[tile.type].ContainsKey(tile.frameY) && tile.frameX == 36)
                {
                    self.ActiveMusicBox = MusicLoader.tileToMusic[tile.type][tile.frameY];
                }

                TileLoader.NearbyEffects(x, y, tile.type, true);
            }
        }

        self.WaterCandleCount  = waterCandleCount;
        self.PeaceCandleCount  = peaceCandleCount;
        self.ShadowCandleCount = shadowCandleCount;

        self.ExportTileCountsToMain();

        self.CanPlayCreditsRoll = self.ActiveMusicBox == 85;

        self.UpdateOreFinderData();

        SystemLoader.TileCountsAvailable(self._tileCounts);
    }
}