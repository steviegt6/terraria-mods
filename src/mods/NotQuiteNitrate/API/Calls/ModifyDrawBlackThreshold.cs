using System;
using System.Collections.Generic;

using Tomat.TML.Library.DynamicModCalls;
using NotQuiteNitrate.Patches;

namespace NotQuiteNitrate.API.Calls;

internal sealed class ModifyDrawBlackThreshold : ModCall
{
    public override IEnumerable<string> CallCommands
    {
        get { yield return "ModifyDrawBlackThreshold"; }
    }

    public override Delegate Delegate { get; } = Impl;

    private static void Impl(Func<float, float> func)
    {
        FasterRenderBlack.CALLBACKS.Add(func);
    }
}