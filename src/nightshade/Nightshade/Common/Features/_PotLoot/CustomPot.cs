using Terraria;

namespace Nightshade.Common.Features;

public abstract class CustomPot
{
    public abstract void PlayBreakSound(int i, int j, int style);

    public abstract void SpawnGore(int i, int j, int style);

    public abstract bool ShouldTryForLoot(int i, int j, int style);

    public abstract void ModifyTorchType(
        int i,
        int j,
        int style,
        Player player,
        ref int torchType,
        ref int glowstickType,
        ref int itemStack
    );

    // Ropes or bombs (or scarab bombs)
    public abstract bool TryGetUtilityItem(
        int i,
        int j,
        int style,
        bool aboveUnderworldLayer,
        out int utilityType,
        out int utilityStack
    );
}