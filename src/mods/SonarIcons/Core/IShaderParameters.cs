using Microsoft.Xna.Framework.Graphics;

namespace Tomat.TML.Mod.SonarIcons.Core;

internal interface IShaderParameters
{
    void Apply(EffectParameterCollection parameters);
}