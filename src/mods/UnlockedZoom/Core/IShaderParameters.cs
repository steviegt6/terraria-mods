using Microsoft.Xna.Framework.Graphics;

namespace Tomat.TML.Mod.UnlockedZoom.Core;

internal interface IShaderParameters
{
    void Apply(EffectParameterCollection parameters);
}