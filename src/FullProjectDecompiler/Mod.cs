using System.Reflection;

using Terraria.ModLoader;

namespace Tomat.TML.Mod.FullProjectDecompiler;

partial class Mod
{
    private sealed class JitFilter : PreJITFilter
    {
        public override bool ShouldJIT(MemberInfo member)
        {
            return false;
        }
    }

    public Mod()
    {
        PreJITFilter = new JitFilter();
    }
}