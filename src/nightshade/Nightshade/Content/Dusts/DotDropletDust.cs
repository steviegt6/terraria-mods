using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace Nightshade.Content.Dusts;

public class DotDropletDust : ModDust
{
    public override string Texture => Assets.Images.Dusts.DotDropletDust.KEY;

	public override bool Update(Dust dust)
	{
		bool noGrav = false;
		if (Collision.WetCollision(dust.position - new Vector2(6), 12, 12))
		{
			dust.velocity.Y *= 0.9f; 
			dust.velocity.Y -= 0.3f;
			dust.scale *= 0.95f;
			noGrav = true;
		}

		if (Collision.SolidCollision(dust.position - new Vector2(6), 12, 12))
		{
			dust.scale *= 0.5f;
			dust.velocity *= 0.1f;
		}

		if (!dust.noGravity && !noGrav)
		{
			dust.velocity.Y += 0.35f;

			if (dust.velocity.Y > 22f)
			{
				dust.velocity.Y = 22f;
			}
		}

		dust.rotation = dust.velocity.ToRotation();

		dust.scale *= 0.99f;

		if (dust.scale < 0.33f)
		{
			dust.scale *= 0.8f;
		}

		if (dust.scale < 0.1f)
		{
			dust.active = false;
		}

		dust.velocity.X *= 0.98f;

        if (!dust.noLightEmittence)
        {
	        Lighting.AddLight(dust.position, dust.color.ToVector3() * MathHelper.Clamp(dust.scale, 0f, 1f) * 0.2f);
        }

        dust.position += dust.velocity;

		return false;
    }

    public override Color? GetAlpha(Dust dust, Color lightColor) => dust.noLight ? dust.color : lightColor.MultiplyRGBA(dust.color);

	public override bool PreDraw(Dust dust)
    {
        var texture = Texture2D.Value;
        var lightColor = Lighting.GetColor((int)(dust.position.X / 16), (int)(dust.position.Y / 16));
        var stretch = new Vector2(0.7f + dust.velocity.Length() * 0.5f / (dust.scale + 1f), dust.scale * 1.5f);
        Main.EntitySpriteDraw(texture, dust.position - Main.screenPosition, texture.Frame(), dust.GetAlpha(lightColor), dust.rotation, texture.Size() / 2, stretch, 0);
        return false;
    }
}
