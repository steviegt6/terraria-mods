using Terraria.ModLoader;
using Terraria.ModLoader.UI;

namespace Daybreak.Common.Features.ModPanel;

/// <summary>
///     A <see cref="Mod"/> implementing this interface provides a singleton
///     object describing the behavior of their custom <see cref="UIModItem"/>
///     edits.
/// </summary>
public interface IHasModPanelStyle
{
    /// <summary>
    ///     The mod panel style of this mod.
    /// </summary>
    ModPanelStyle PanelStyle { get; }
}