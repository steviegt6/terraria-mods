using Microsoft.Xna.Framework.Graphics;

using Nightshade.Common.Rendering;
using Nightshade.Core.Attributes;

using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace Nightshade.Content.VisualTweaks;

internal sealed class VanillaVertexStripTweak : ModSystem
{
    [InitializedInLoad]
    private static ManagedRenderTarget? managedRt;

    public override void Load()
    {
        base.Load();

        Main.QueueMainThreadAction(
            () =>
            {
                var effect = ModContent.Request<Effect>(Assets.Shaders.Misc.VanillaVertexStripShader.KEY);
                GameShaders.Misc["MagicMissile"] = new MiscShaderData(effect, "MagicMissile").UseProjectionMatrix(doUse: true);
                GameShaders.Misc["MagicMissile"].UseImage0("Images/Extra_" + (short)192);
                GameShaders.Misc["MagicMissile"].UseImage1("Images/Extra_" + (short)194);
                GameShaders.Misc["MagicMissile"].UseImage2("Images/Extra_" + (short)193);
                GameShaders.Misc["FlameLash"] = new MiscShaderData(effect, "MagicMissile").UseProjectionMatrix(doUse: true);
                GameShaders.Misc["FlameLash"].UseImage0("Images/Extra_" + (short)191);
                GameShaders.Misc["FlameLash"].UseImage1("Images/Extra_" + (short)189);
                GameShaders.Misc["FlameLash"].UseImage2("Images/Extra_" + (short)190);
                GameShaders.Misc["RainbowRod"] = new MiscShaderData(effect, "MagicMissile").UseProjectionMatrix(doUse: true);
                GameShaders.Misc["RainbowRod"].UseImage0("Images/Extra_" + (short)195);
                GameShaders.Misc["RainbowRod"].UseImage1("Images/Extra_" + (short)197);
                GameShaders.Misc["RainbowRod"].UseImage2("Images/Extra_" + (short)196);
                GameShaders.Misc["FinalFractal"] = new MiscShaderData(effect, "FinalFractalVertex").UseProjectionMatrix(doUse: true);
                GameShaders.Misc["FinalFractal"].UseImage0("Images/Extra_" + (short)195);
                GameShaders.Misc["FinalFractal"].UseImage1("Images/Extra_" + (short)197);
                GameShaders.Misc["EmpressBlade"] = new MiscShaderData(effect, "FinalFractalVertex").UseProjectionMatrix(doUse: true);
                GameShaders.Misc["EmpressBlade"].UseImage0("Images/Extra_" + (short)209);
                GameShaders.Misc["EmpressBlade"].UseImage1("Images/Extra_" + (short)210);
                GameShaders.Misc["LightDisc"] = new MiscShaderData(effect, "MagicMissile").UseProjectionMatrix(doUse: true);
                GameShaders.Misc["LightDisc"].UseImage0("Images/Extra_" + (short)195);
                GameShaders.Misc["LightDisc"].UseImage1("Images/Extra_" + (short)195);
                GameShaders.Misc["LightDisc"].UseImage2("Images/Extra_" + (short)252);
            }
        );

        managedRt = new ManagedRenderTarget(reinitOnResolutionChange: true);

        Main.QueueMainThreadAction(
            () =>
            {
                managedRt.Initialize(Main.screenWidth, Main.screenHeight);
            }
        );
    }
}