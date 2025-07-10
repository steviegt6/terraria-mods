using System.Reflection;

using Terraria.ModLoader;

namespace Tomat.TML.Mod.FullProjectDecompiler;

partial class ModImpl
{
    private sealed class JitFilter : PreJITFilter
    {
        public override bool ShouldJIT(MemberInfo member)
        {
            return false;
        }
    }

    public ModImpl()
    {
        PreJITFilter = new JitFilter();
    }
}