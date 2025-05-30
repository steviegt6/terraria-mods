namespace Nightshade.Common.Features;

public abstract class CustomItemSlotContext
{
    public abstract int VanillaContext { get; }

    public static CustomItemSlotContext CreateVanillaContext(int context)
    {
        return new VanillaItemSlotContext(context);
    }
}

internal sealed class VanillaItemSlotContext(int context) : CustomItemSlotContext
{
    public override int VanillaContext => context;
}