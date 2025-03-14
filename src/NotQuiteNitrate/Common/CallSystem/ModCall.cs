using System;
using System.Collections.Generic;

using Terraria.ModLoader;

namespace Tomat.TML.Mod.NotQuiteNitrate.Common.CallSystem;

internal abstract class ModCall : ModType
{
    /// <summary>
    ///     A collection of (interpreted-as-case-insensitive) call "commands".
    /// </summary>
    public abstract IEnumerable<string> CallCommands { get; }

    /// <summary>
    ///     The delegate to invoke.
    /// </summary>
    public abstract Delegate Delegate { get; }

    public sealed override void Register()
    {
        CallHandler.Register(Mod, this);
    }
}