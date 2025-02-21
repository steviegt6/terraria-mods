using System;
using System.Reflection;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;

using MonoMod.Cil;

using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Light;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Tomat.Terraria.TML.UnlockedZoom;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
public sealed class UzMod : Mod;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class ZoomAndScreenParamOverride : ModSystem
{
    private int oldMaxScreenW;
    private int oldMaxScreenH;

    private int oldRenderTargetMaxSize;
    // private float oldMinimumZoomComparerX;
    // private float oldMinimumZoomComparerY;

    public override void Load()
    {
        base.Load();

        const int   res_i = 8192  * 2;
        const float res_f = 8192f * 2;

        // Override a lot of fields controlling screen and zoom limits.

        oldMaxScreenW = Main.maxScreenW;
        oldMaxScreenH = Main.maxScreenH;
        {
            Main.maxScreenW = res_i;
            Main.maxScreenH = res_i;
        }

        oldRenderTargetMaxSize = Main._renderTargetMaxSize;
        {
            Main._renderTargetMaxSize = res_i;
        }

        // Use tML settings.
        // oldMinimumZoomComparerX = Main.MinimumZoomComparerX;
        // oldMinimumZoomComparerY = Main.MinimumZoomComparerY;
        // {
        //     Main.MinimumZoomComparerX = res_f;
        //     Main.MinimumZoomComparerY = res_f;
        // }

        // Support display sizes based on zoom comparer instead of max screen
        // size.
        IL_Main.CacheSupportedDisplaySizes += il =>
        {
            var c = new ILCursor(il);

            while (c.TryGotoNext(MoveType.After, x => x.MatchLdsfld<Main>("maxScreenW")))
            {
                c.EmitPop();
                c.EmitLdsfld(typeof(Main).GetField(nameof(Main.MinimumZoomComparerX), BindingFlags.Public | BindingFlags.Static)!);
            }
            c.Index = 0;

            while (c.TryGotoNext(MoveType.After, x => x.MatchLdsfld<Main>("maxScreenH")))
            {
                c.EmitPop();
                c.EmitLdsfld(typeof(Main).GetField(nameof(Main.MinimumZoomComparerY), BindingFlags.Public | BindingFlags.Static)!);
            }
        };
    }

    public override void Unload()
    {
        base.Unload();

        Main.maxScreenW           = oldMaxScreenW;
        Main.maxScreenH           = oldMaxScreenH;
        Main._renderTargetMaxSize = oldRenderTargetMaxSize;
        // Main.MinimumZoomComparerX = oldMinimumZoomComparerX;
        // Main.MinimumZoomComparerY = oldMinimumZoomComparerY;
    }
}

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class LightMapEdit : ModSystem
{
    public override void Load()
    {
        base.Load();

        IL_LightMap.ctor += il =>
        {
            var c = new ILCursor(il);

            const int width  = LightMap.DEFAULT_WIDTH;
            const int height = LightMap.DEFAULT_HEIGHT;
            const int size   = width * height;

            while (c.TryGotoNext(MoveType.After, x => x.MatchLdcI4(width)))
            {
                c.EmitPop();
                c.EmitLdcI4(width * 2);
            }
            c.Index = 0;

            // Height is the same as width, but eh.
            while (c.TryGotoNext(MoveType.After, x => x.MatchLdcI4(height)))
            {
                c.EmitPop();
                c.EmitLdcI4(height * 2);
            }

            while (c.TryGotoNext(MoveType.After, x => x.MatchLdcI4(size)))
            {
                c.EmitPop();
                c.EmitLdcI4(size);
            }
        };
    }

    public override void PostSetupContent()
    {
        base.PostSetupContent();

        Lighting.Initialize();
        Lighting.LegacyEngine._lightMap     = new LightMap();
        Lighting.NewEngine._activeLightMap  = new LightMap();
        Lighting.NewEngine._workingLightMap = new LightMap();
    }
}

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class ExpandClampedWorldRectangles : ModSystem
{
    public override void Load()
    {
        base.Load();

        On_WorldUtils.ClampToWorld += (orig, rectangle) => orig(ClampRectangle(rectangle));

        return;

        static Rectangle ClampRectangle(Rectangle rect)
        {
            rect.Inflate(
                Math.Min(0, 1920 - rect.Width),
                Math.Min(0, 1080 - rect.Height)
            );
            return rect;
        }
    }
}

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class ExpandScreenDrawAreaBasedOnZoom : ModSystem
{
    public override void Load()
    {
        base.Load();

        On_TileDrawing.GetScreenDrawArea += (
            On_TileDrawing.orig_GetScreenDrawArea _, // orig
            TileDrawing                           _, // self
            Vector2                               screenPosition,
            Vector2                               offSet,
            out int                               firstTileX,
            out int                               lastTileX,
            out int                               firstTileY,
            out int                               lastTileY
        ) =>
        {
            var realWidth  = (int)(Main.screenWidth  / Main.GameViewMatrix.Zoom.X);
            var realHeight = (int)(Main.screenHeight / Main.GameViewMatrix.Zoom.Y);

            firstTileX = (int)((screenPosition.X - offSet.X)              / 16f - 1f);
            lastTileX  = (int)((screenPosition.X + realWidth + offSet.X)  / 16f) + 2;
            firstTileY = (int)((screenPosition.Y - offSet.Y)              / 16f - 1f);
            lastTileY  = (int)((screenPosition.Y + realHeight + offSet.Y) / 16f) + 5;

            // Guards against drawing outside the world.
            {
                if (firstTileX < 4)
                {
                    firstTileX = 4;
                }

                if (lastTileX > Main.maxTilesX - 4)
                {
                    lastTileX = Main.maxTilesX - 4;
                }

                if (firstTileY < 4)
                {
                    firstTileY = 4;
                }

                if (lastTileY > Main.maxTilesY - 4)
                {
                    lastTileY = Main.maxTilesY - 4;
                }
            }

            if (Main.sectionManager.AnyUnfinishedSections)
            {
                TimeLogger.DetailedDrawReset();
                WorldGen.SectionTileFrameWithCheck(firstTileX, firstTileY, lastTileX, lastTileY);
                TimeLogger.DetailedDrawTime(5);
            }

            if (Main.sectionManager.AnyNeedRefresh)
            {
                WorldGen.RefreshSections(firstTileX, firstTileY, lastTileX, lastTileY);
            }
        };
    }
}