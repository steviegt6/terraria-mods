using Microsoft.Xna.Framework;
using Nightshade.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Projectiles.Enemy.Thaw;
public class ThawSlash : ModProjectile
{
    public override string Texture => Assets.Images.Projectiles.ThawSlash.KEY;
    public override void SetDefaults()
    {
        Projectile.Size = Vector2.One * 80;
        Projectile.tileCollide = false;
        Projectile.hostile = true;
        Projectile.friendly = false;
        Projectile.aiStyle = -1;
        Projectile.timeLeft = 500;
    }
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
        Lighting.AddLight(Projectile.Center, TorchID.Ice);
        if (Projectile.velocity.Length() > 4)
        {
            if (Projectile.timeLeft % 2 == 0)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch);
            else
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<ThawSparkles>(), newColor: Color.White);
        }
        Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.Length() * 2);
        if (Projectile.timeLeft > 460)
            Projectile.velocity *= 0.96f;
        else if (Projectile.timeLeft > 440)
        {
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(player.Center) * 20, 0.025f);
        }
        else
        {
            if (Projectile.velocity.Length() < 20)
                Projectile.velocity *= 1.04f;
        }
    }
}
