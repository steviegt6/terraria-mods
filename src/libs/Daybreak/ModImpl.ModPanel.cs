using Daybreak.Common.Features.ModPanel;

using Terraria.ModLoader;

namespace Daybreak;

public partial class ModImpl : IHasModPanelStyle
{
    public ModPanelStyle PanelStyle => ModContent.GetInstance<DaybreakPanelStyle>();
}