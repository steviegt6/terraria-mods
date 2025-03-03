using System.Runtime.CompilerServices;

namespace Tomat.TML.Mod.NotQuiteNitrate.Utilities;

internal static class Bool
{
    // Theoretically, the JIT can optimize `value ? 1 : 0` expressions... but
    // better safe than sorry.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe byte ToByte(bool value)
    {
        return *(byte*)&value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe bool FromByte(byte value)
    {
        return *(bool*)&value;
    }
}