using Tomat.TML.Mod.NotQuiteNitrate.Common.CallSystem;

namespace Tomat.TML.Mod.NotQuiteNitrate;

partial class Mod
{
    public override object? Call(params object?[]? args)
    {
        return CallHandler.HandleCall(this, args);
    }
}