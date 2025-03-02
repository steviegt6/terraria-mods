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

namespace Tomat.TML.Mod.NotQuiteNitrate.Patches;

/// <summary>
///     Parallelizes SceneMetrics.
/// </summary>
internal sealed class ParallelizeSceneMetrics : ModSystem
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

        // if (settings.ScanOreFinderData)
        // {
        //     self._oreFinderTileLocations.Clear();
        // }

        // TODO: Can we avoid allocating this every call?  Well, yes, we can,
        //       just use a CWT.  Can we do it an easier?
        var oreFinderTileLocations = settings.ScanOreFinderData ? new ConcurrentBag<Point>() : null;

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

            Parallel.For(
                // FastParallel.For(
                biomeScanArea.Left,
                biomeScanArea.Right,
                // (start, end, _) =>
                x =>
                {
                    // for (var x = start; x < end; x++)
                    for (var y = biomeScanArea.Top; y < biomeScanArea.Bottom; y++)
                    {
                        if (!biomeScanArea.Contains(x, y))
                        {
                            continue;
                        }

                        var tile = Main.tile[x, y];

                        if (!tile.HasTile)
                        {
                            if (tile.liquid > 0)
                            {
                                Interlocked.Increment(ref self._liquidCounts[tile.LiquidType]);
                            }

                            continue;
                        }

                        if (!TileID.Sets.isDesertBiomeSand[tile.type] || !WorldGen.oceanDepths(x, y))
                        {
                            Interlocked.Increment(ref self._tileCounts[tile.type]);
                        }

                        if (tile is { type: 215, frameY: < 36 })
                        {
                            // Already thread-safe.
                            self.HasCampfire = true;
                        }

                        if (tile is { type: 49, frameX: < 18 })
                        {
                            Interlocked.Increment(ref waterCandleCount);
                        }

                        if (tile is { type: 372, frameX: < 18 })
                        {
                            Interlocked.Increment(ref peaceCandleCount);
                        }

                        if (tile is { type: 646, frameX: < 18 })
                        {
                            Interlocked.Increment(ref shadowCandleCount);
                        }

                        if (tile is { type: 405, frameX: < 54 })
                        {
                            // Already thread-safe.
                            self.HasCampfire = true;
                        }

                        if (tile is { type: 506, frameX: < 72 })
                        {
                            // Already thread-safe.
                            self.HasCatBast = true;
                        }

                        if (tile is { type: 42, frameY: >= 324 and <= 358 })
                        {
                            // Already thread-safe.
                            self.HasHeartLantern = true;
                        }

                        if (tile is { type: 42, frameY: >= 252 and <= 286 })
                        {
                            // Already thread-safe.
                            self.HasStarInBottle = true;
                        }

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

                        // if (settings.ScanOreFinderData && Main.tileOreFinderPriority[tile.type] != 0)
                        // {
                        //     self._oreFinderTileLocations.Add(new Point(x, y));
                        // }

                        if (oreFinderTileLocations is not null && Main.tileOreFinderPriority[tile.type] != 0)
                        {
                            oreFinderTileLocations.Add(new Point(x, y));
                        }

                        // TileLoader.NearbyEffects(x, y, tile.type, false);
                    }
                }
            );

            // Run NearbyEffects outside the parallelized loop since we can't
            // guarantee modders will make it thread-safe.
            // for (var x = biomeScanArea.Left; x < biomeScanArea.Right; x++)
            // for (var y = biomeScanArea.Top; y < biomeScanArea.Bottom; y++)
            // {
            //     TileLoader.NearbyEffects(x, y, Main.tile[x, y].type, false);
            // }
        }

        if (settings.VisualScanArea.HasValue)
        {
            var visualScanArea = WorldUtils.ClampToWorld(settings.VisualScanArea.Value);

            Parallel.For(
                // FastParallel.For(
                visualScanArea.Left,
                visualScanArea.Right,
                // (start, end, _) =>
                x =>
                {
                    // for (var x = start; x < end; x++)
                    for (var y = visualScanArea.Top; y < visualScanArea.Bottom; y++)
                    {
                        var tile = Main.tile[x, y];

                        if (!tile.HasTile)
                        {
                            continue;
                        }

                        if (TileID.Sets.Clock[tile.type])
                        {
                            // Already thread-safe.
                            self.HasClock = true;
                        }

                        switch (tile.type)
                        {
                            case 139:
                                if (tile.frameX >= 36)
                                {
                                    // TODO
                                    self.ActiveMusicBox = tile.frameY / 36;
                                }
                                break;

                            case 207:
                                if (tile.frameY >= 72)
                                {
                                    // TODO
                                    var fountainIdx = tile.frameX / 36;
                                    self.ActiveFountainColor = fountainIdx >= 0 && fountainIdx < fountain_map.Length ? fountain_map[fountainIdx] : -1;
                                }
                                break;

                            case 410:
                                if (tile.frameY >= 56)
                                {
                                    // TODO
                                    var activeMonolithType = tile.frameX / 36;
                                    self.ActiveMonolithType = activeMonolithType;
                                }
                                break;

                            case 509:
                                if (tile.frameY >= 56)
                                {
                                    // TODO
                                    self.ActiveMonolithType = 4;
                                }
                                break;

                            case 480:
                                if (tile.frameY >= 54)
                                {
                                    // Already thread-safe.
                                    self.BloodMoonMonolith = true;
                                }
                                break;

                            case 657:
                                if (tile.frameY >= 54)
                                {
                                    // Already thread-safe.
                                    self.EchoMonolith = true;
                                }
                                break;

                            case 658:
                            {
                                // TODO
                                var shimmerMonolithState = tile.frameY / 54;
                                self.ShimmerMonolithState = shimmerMonolithState;
                                break;
                            }
                        }

                        // TODO
                        if (MusicLoader.tileToMusic.ContainsKey(tile.type) && MusicLoader.tileToMusic[tile.type].ContainsKey(tile.frameY) && tile.frameX == 36)
                        {
                            self.ActiveMusicBox = MusicLoader.tileToMusic[tile.type][tile.frameY];
                        }

                        // TileLoader.NearbyEffects(x, y, tile.type, true);
                    }
                }
            );

            // Run NearbyEffects outside the parallelized loop since we can't
            // guarantee modders will make it thread-safe.
            // for (var x = visualScanArea.Left; x < visualScanArea.Right; x++)
            // for (var y = visualScanArea.Top; y < visualScanArea.Bottom; y++)
            // {
            //     TileLoader.NearbyEffects(x, y, Main.tile[x, y].type, false);
            // }
        }

        self.WaterCandleCount  = waterCandleCount;
        self.PeaceCandleCount  = peaceCandleCount;
        self.ShadowCandleCount = shadowCandleCount;

        self.ExportTileCountsToMain();

        self.CanPlayCreditsRoll = self.ActiveMusicBox == 85;

        if (oreFinderTileLocations is not null)
        {
            // TODO: How optimal is this?
            self._oreFinderTileLocations.Clear();
            self._oreFinderTileLocations.AddRange(oreFinderTileLocations);
            self.UpdateOreFinderData();
        }

        SystemLoader.TileCountsAvailable(self._tileCounts);
    }
}