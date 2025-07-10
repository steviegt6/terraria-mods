using Microsoft.Xna.Framework.Graphics;

namespace Tomat.TML.Mod.FullProjectDecompiler.Core;

internal interface IShaderParameters
{
    void Apply(EffectParameterCollection parameters);
}