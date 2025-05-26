using Microsoft.Xna.Framework;
using Nightshade.Common.Features;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Daybreak.Common.Rendering;
using Nightshade.Core;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.VisualTweaks;

[Autoload(Side = ModSide.Client)]
internal class FlailChainTweak : GlobalProjectile
{
    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
    {
        return lateInstantiation && entity.aiStyle == ProjAIStyleID.Flail;
    }

    public override void Load()
    {
        base.Load();

        On_Main.DrawProj_FlailChains += (orig, projectile, mountedCenter) =>
        {
            
        };
    }
}

