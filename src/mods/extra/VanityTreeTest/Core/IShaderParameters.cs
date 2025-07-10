using Microsoft.Xna.Framework.Graphics;

namespace Tomat.TML.Mod.VanityTreeTest.Core;

internal interface IShaderParameters
{
    void Apply(EffectParameterCollection parameters);
}