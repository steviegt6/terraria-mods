using Microsoft.Xna.Framework.Graphics;

namespace Nightshade.Core;

internal interface IShaderParameters
{
    void Apply(EffectParameterCollection parameters);
}