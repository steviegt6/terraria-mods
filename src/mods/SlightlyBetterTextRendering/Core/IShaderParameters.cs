using Microsoft.Xna.Framework.Graphics;

namespace Tomat.TML.Mod.SlightlyBetterTextRendering.Core;

internal interface IShaderParameters
{
    void Apply(EffectParameterCollection parameters);
}