using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Nightshade.Content.Dusts;
public class ThawSparkles : ModDust
{
    public override void OnSpawn(Dust dust)
    {
        if (dust.customData == null)
            dust.frame = new Rectangle(0, 22 * Main.rand.Next(3), 22, 22);
        else if (dust.customData is int)
            dust.frame = new Rectangle(0, 22 * (int)dust.customData, 22, 22);

        dust.customData = (int)0;
    }
    public override bool Update(Dust dust)
    {
        Color lightColor = dust.color * 0.1f;
        Lighting.AddLight(dust.position, lightColor.R / 255f, lightColor.G / 255f, lightColor.B / 255f);
        dust.customData = (int)dust.customData + 1;
        dust.position += dust.velocity;
        dust.velocity *= 0.99f;

        if ((int)dust.customData % 4 == 0)
            dust.rotation = MathHelper.PiOver2;
        else if ((int)dust.customData % 4 == 2)
            dust.rotation = 0;

        if ((int)dust.customData % 5 == 0)
        {
            if (dust.frame.Y >= 8 * 22)
                dust.active = false;
            dust.frame.Y += 22;
        }
        return false;
    }
    public override bool PreDraw(Dust dust)
    {
        Main.EntitySpriteDraw(Texture2D.Value, dust.position - Main.screenPosition, dust.frame, dust.color, dust.rotation, dust.frame.Size() / 2, dust.scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
        return false;
    }
}
