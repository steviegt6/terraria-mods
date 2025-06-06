using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Projectiles.Held
{
    internal class PreDigesterHeldProj : ModProjectile
    {
        public override string Texture => Assets.Images.Projectiles.PreDigesterHeldProjSheet.KEY;

        private Texture2D? tex;
        private Player? player;
        private float sackScale;
        private float sackRot;

        public override void SetDefaults()
        {
            tex = ModContent.Request<Texture2D>(Assets.Images.Projectiles.PreDigesterHeldProjSheet.KEY).Value;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1000;
            Projectile.rotation = 1f * Main.player[Projectile.owner].direction;
            Projectile.Opacity = 0f;
            sackScale = 1f;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void AI()
        {
            player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;

            var holdCenter = player.RotatedRelativePoint(player.MountedCenter);
            Projectile.Center = new Vector2((int)holdCenter.X, (int)holdCenter.Y) + new Vector2(7 * player.direction, 7);

            if (player.itemAnimation == 1)
            {
                Projectile.ai[1] = 1;
                Projectile.ai[0] = 0;
                SoundEngine.PlaySound(SoundID.NPCHit31, holdCenter);
            }
            if (!player.channel)
            {
                Projectile.ai[1] = 2;
            }

            //Rotate Up
            if (Projectile.ai[1] == 0)
            {
                Projectile.rotation = MathHelper.Lerp(Projectile.rotation, 0.2f, 0.1f);
                sackScale = MathHelper.Lerp(sackScale, 1f, 0.1f);
                sackRot = MathHelper.Lerp(sackRot, 0f, 0.1f);
                Projectile.Opacity += 0.2f;
            }
            //Go Down and Spew
            else if (Projectile.ai[1] == 1)
            {
                Projectile.rotation = MathHelper.Lerp(Projectile.rotation, 1f, 0.1f);
                sackScale = MathHelper.Lerp(sackScale, 0.6f, 0.1f);
                sackRot = MathHelper.Lerp(sackRot, -1f, 0.1f);
                if (Projectile.ai[0] >= 5)
                {
                    Projectile.ai[1] = 0;
                }
            }
            //Go Down and Disappear
            else
            {
                Projectile.rotation = MathHelper.Lerp(Projectile.rotation, 1f, 0.1f);
                sackScale = MathHelper.Lerp(sackScale, 1f, 0.1f);
                sackRot = MathHelper.Lerp(sackRot, 1f, 0.1f);
                Projectile.Opacity -= 0.2f;
                if (Projectile.Opacity <= 0f)
                {
                    Projectile.Kill();
                }
            }

            Projectile.ai[0]++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (player is null)
            {
                return false;
            }

            //Sack
            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition + new Vector2(12 * player.direction, 0),
                new Rectangle(4, 28, 22, 20),
                lightColor * 0.6f * sackScale * Projectile.Opacity,
                (Projectile.rotation + sackRot) * player.direction,
                new Vector2(player.direction == 1 ? 14 : 8, 4),
                Projectile.scale * sackScale,
                player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally
            );
            
            //Top Body
            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                new Rectangle(Projectile.ai[1] == 1 ? 28 : 0, 0, 26, 26),
                lightColor * Projectile.Opacity,
                Projectile.rotation * player.direction,
                new Vector2(player.direction == 1 ? 6 : 20, 24),
                Projectile.scale,
                player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally
            );

            return false;
        }
    }
}