using Microsoft.Xna.Framework.Graphics;

namespace Tomat.TML.Mod.VanillaNetworking.Core;

internal interface IShaderParameters
{
    void Apply(EffectParameterCollection parameters);
}