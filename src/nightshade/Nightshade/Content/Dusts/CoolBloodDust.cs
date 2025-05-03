using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace Nightshade.Content.Dusts;

internal sealed class CoolBloodDust : ModDust
{
    public override string Texture => Assets.Images.Dusts.CoolBloodDust.KEY;

    public override void OnSpawn(Dust dust)
    {
        base.OnSpawn(dust);

        dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
    }

    public override bool Update(Dust dust)
    {
        dust.position += dust.velocity;
        dust.velocity.X *= 0.98f;
        dust.velocity.Y += 0.2f;
        dust.rotation += dust.velocity.X / 5f;

        if (Collision.SolidCollision(dust.position - Vector2.One * 4f, 8, 8) && dust.fadeIn == 0f)
        {
            dust.velocity = Vector2.Zero;
            dust.scale -= 0.1f;
        }

        if (dust.scale < 0f)
        {
            dust.active = false;
        }

        return false;
    }

    public override Color? GetAlpha(Dust dust, Color lightColor)
    {
        float alpha = (float)(255 - dust.alpha) / 255f;
        return lightColor * alpha;
    }
}