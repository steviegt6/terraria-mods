using Terraria.ModLoader;

namespace Tomat.TML.Mod.FullProjectDecompiler.Common;

public interface IInitializer : ILoadable
{
    void ILoadable.Unload() { }
}