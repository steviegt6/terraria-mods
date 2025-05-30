using System.Collections.Generic;
using System.Diagnostics;

using MonoMod.Cil;

using Terraria;
using Terraria.ModLoader;

namespace Nightshade.Common.Features;

internal sealed class MiscSlotLoader : ModSystem
{
    public static MiscSlot Pet { get; } = new PetSlot();

    public static MiscSlot LightPet { get; } = new LightPetSlot();

    public static MiscSlot Minecart { get; } = new MinecartSlot();

    public static MiscSlot Mount { get; } = new MountSlot();

    public static MiscSlot Hook { get; } = new HookSlot();

    private readonly List<MiscSlot> slots =
    [
        Pet,
        LightPet,
        Minecart,
        Mount,
        Hook,
    ];

    public override void Load()
    {
        base.Load();

        IL_Main.DrawInventory += DrawInventory_ReplaceVanillaMiscSlotDrawing;
    }

    private static void DrawInventory_ReplaceVanillaMiscSlotDrawing(ILContext il)
    {
        var c = new ILCursor(il);

        c.GotoNext(
            x => x.MatchLdsfld<Main>(nameof(Main.EquipPage)),
            x => x.MatchLdcI4((int)EquipmentPageId.Misc)
        );

        c.GotoNext(MoveType.Before, x => x.MatchLdloca(out _));
        c.EmitDelegate(ManuallyDrawMiscSlots);

        var pos = c.Index;

        // Find the label needed to escape the if-else chain by going to the
        // next, shorter clause.
        c.GotoNext(
            x => x.MatchLdsfld<Main>(nameof(Main.EquipPage)),
            x => x.MatchLdcI4(1)
        );

        ILLabel? label = null;
        c.GotoNext(x => x.MatchBr(out label));
        {
            Debug.Assert(label is not null);
        }

        c.Index = pos;

        c.EmitBr(label);
    }

    private static void ManuallyDrawMiscSlots() { }
}