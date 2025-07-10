using Microsoft.Xna.Framework.Graphics;

namespace Tomat.TML.Mod.NotQuiteNitrate.Core;

internal interface IShaderParameters
{
    void Apply(EffectParameterCollection parameters);
}