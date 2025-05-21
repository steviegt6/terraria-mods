using Terraria.ModLoader;

namespace Nightshade.Content.NPCs.Bosses.RaA;

public sealed class SlimeBoss : ModNPC
{
    public override string Texture => Assets.Images.NPCs.Bosses.RaA.SlimeBoss.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        NPC.lifeMax = 10000;
    }
}