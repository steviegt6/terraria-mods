using Microsoft.Xna.Framework.Graphics;

namespace Tomat.TML.Mod.ChitterChatter.Core;

internal interface IShaderParameters
{
    void Apply(EffectParameterCollection parameters);
}