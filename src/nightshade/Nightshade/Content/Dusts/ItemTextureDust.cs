using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Nightshade.Content.Dusts
{
    //Set Custom Data of this Dust to any ItemID to copy its texture.
    public class ItemTextureDust : ModDust
    {

        public override void OnSpawn(Dust dust)
        {

        }

        public override bool Update(Dust dust)
        {
            dust.fadeIn += 0.02f;
            dust.position += dust.velocity;
            dust.velocity.X *= 0.98f;
            dust.velocity.Y += 0.2f;
            dust.rotation += dust.velocity.X / 5f;

            if (dust.fadeIn >= 1f) dust.active = false;

            return false;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return lightColor;
        }

        public override bool PreDraw(Dust dust)
        {
            if (dust.customData is int data)
            {
                Texture2D Tex = TextureAssets.Item[data].Value;
                Main.EntitySpriteDraw(Tex, dust.position - Main.screenPosition, Tex.Bounds, Lighting.GetColor(dust.position.ToTileCoordinates()) * (1f - dust.fadeIn), dust.rotation, Tex.Size() / 2, dust.scale, SpriteEffects.None);
            }

            return false;
        }
    }
}
