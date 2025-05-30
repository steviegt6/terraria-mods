using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Common.Features;

internal sealed class RainbowCursorImpl : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.RainbowCursor;
    }

    public override void SetDefaults(Item entity)
    {
        base.SetDefaults(entity);

        entity.accessory = false;
    }
}