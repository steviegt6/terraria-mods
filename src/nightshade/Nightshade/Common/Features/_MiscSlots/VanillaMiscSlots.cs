namespace Nightshade.Common.Features;

internal sealed class PetSlot : MiscSlot
{
    public override int GetContext()
    {
        return 19;
    }
}

internal sealed class LightPetSlot : MiscSlot
{
    public override int GetContext()
    {
        return 20;
    }
}

internal sealed class MinecartSlot : MiscSlot
{
    public override int GetContext()
    {
        return 18;
    }
}

internal sealed class MountSlot : MiscSlot
{
    public override int GetContext()
    {
        return 17;
    }
}

internal sealed class HookSlot : MiscSlot
{
    public override int GetContext()
    {
        return 16;
    }
}