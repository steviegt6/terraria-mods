using Terraria.ModLoader;

namespace Nightshade.Common.Features;

internal abstract class CustomItemSlot : ModType
{
    public int Type { get; internal set; }

    protected sealed override void Register()
    {
        ItemSlotLoader.Register(this);
    }

    protected sealed override void InitTemplateInstance()
    {
        base.InitTemplateInstance();
    }

    public sealed override void SetupContent()
    {
        base.SetupContent();

        SetStaticDefaults();
    }
}