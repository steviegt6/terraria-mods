using Daybreak.Common.Features.ModPanel;

using Nightshade.Content.VisualTweaks.UI;

using Terraria.ModLoader;

namespace Nightshade;

public partial class ModImpl : IHasModPanelStyle
{
    public ModPanelStyle PanelStyle => ModContent.GetInstance<NightshadePanelStyle>();
}