using Microsoft.Xna.Framework.Graphics;

namespace Tomat.TML.Mod.PrefixGrammar.Core;

internal interface IShaderParameters
{
    void Apply(EffectParameterCollection parameters);
}