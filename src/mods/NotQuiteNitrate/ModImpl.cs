using Tomat.TML.Library.DynamicModCalls;

namespace Tomat.TML.Mod.NotQuiteNitrate;

partial class ModImpl
{
    public override object? Call(params object?[]? args)
    {
        return CallHandler.HandleCall(this, args);
    }
}