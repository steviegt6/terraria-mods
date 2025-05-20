using Microsoft.Xna.Framework;

using Nightshade.Common.Compat;

using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.NPCs.Bosses.RaA;

public class TestNpc : ModNPC
{
    public override string Texture => "ModLoader/UnloadedItem";

    public override void SetDefaults()
    {
        base.SetDefaults();

        NPC.width = 16;
        NPC.height = 16;

        NPC.aiStyle = NPCAIStyleID.FaceClosestPlayer;
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);

        var profile = new CalamityFablesCompat.CardProfile(
            BossName: "Reductio ad Astra",
            BossTitle: "Sleeping Star from the Darkest Expanse",
            AnimationDuration: 60 * 3,
            Flip: false,
            BorderColor: new Color(255, 54, 74),
            BossTitleColor: new Color(255, 255, 237),
            BossNameChromaA: new Color(255, 0, 51),
            BossNameChromaB: new Color(255, 0, 51)
        );
        CalamityFablesCompat.DisplayBossCard(profile);
    }
}