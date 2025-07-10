using Microsoft.Xna.Framework.Graphics;

namespace Tomat.TML.Mod.CrowsWhoMow.Core;

internal interface IShaderParameters
{
    void Apply(EffectParameterCollection parameters);
}