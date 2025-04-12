using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Projectiles;

internal abstract class StarFragmentProj : ModProjectile
{
    private sealed class StarFragmentSpawner : ModSystem
    {
        public override void PostUpdateWorld()
        {
            base.PostUpdateWorld();

            if (Main.dayTime && !Main.remixWorld)
            {
                return;
            }

            for (var i = 0; i < Main.worldEventUpdates; i++)
            {
                var num7 = Main.maxTilesX / 4200.0;
                num7 *= Star.starfallBoost;
                num7 *= 10000;

                if (!(Main.rand.Next(8000) < 10.0 * num7))
                {
                    continue;
                }

                const int num8 = 12;

                var num9 = Main.rand.Next(Main.maxTilesX - 50) + 100;
                num9 *= 16;

                var num10 = Main.rand.Next((int)(Main.maxTilesY * 0.05));
                num10 *= 16;

                var position = new Vector2(num9, num10);
                var num11    = -1;
                if (Main.expertMode && Main.rand.NextBool(15))
                {
                    int num12 = Player.FindClosest(position, 1, 1);
                    if (Main.player[num12].position.Y < Main.worldSurface * 16.0 && Main.player[num12].afkCounter < 3600)
                    {
                        var num13 = Main.rand.Next(1, 640);
                        position.X = Main.player[num12].position.X + Main.rand.Next(-num13, num13 + 1);
                        num11      = num12;
                    }
                }
                if (!Collision.SolidCollision(position, 16, 16))
                {
                    float num14 = Main.rand.Next(-100, 101);
                    float num15 = Main.rand.Next(200) + 100;
                    var   num16 = (float)Math.Sqrt(num14 * num14 + num15 * num15);
                    num16 =  num8 / num16;
                    num14 *= num16;
                    num15 *= num16;
                    Projectile.NewProjectile(
                        new EntitySource_Misc("FallingStar"),
                        position.X,
                        position.Y,
                        num14,
                        num15,
                        ModContent.ProjectileType<StarFragmentProjSpawner>(),
                        0,
                        0f,
                        Main.myPlayer,
                        0f,
                        num11
                    );
                }
            }
        }
    }

    private sealed class StarFragmentProj1 : StarFragmentProj
    {
        public override string Texture => Assets.Images.Items.Misc.StarFragment_1.KEY;
    }

    private sealed class StarFragmentProj2 : StarFragmentProj
    {
        public override string Texture => Assets.Images.Items.Misc.StarFragment_2.KEY;
    }

    private sealed class StarFragmentProj3 : StarFragmentProj
    {
        public override string Texture => Assets.Images.Items.Misc.StarFragment_3.KEY;
    }

    private sealed class StarFragmentProj4 : StarFragmentProj
    {
        public override string Texture => Assets.Images.Items.Misc.StarFragment_4.KEY;
    }

    private sealed class StarFragmentProj5 : StarFragmentProj
    {
        public override string Texture => Assets.Images.Items.Misc.StarFragment_5.KEY;
    }

    public abstract override string Texture { get; }

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Projectile.width, Projectile.height) = (12, 12);

        Projectile.friendly  = true;
        Projectile.penetrate = -1;
        if (Main.remixWorld)
        {
            Projectile.hostile = true;
        }

        Projectile.alpha = 50;
        Projectile.light = 1f;
    }

    public override void AI()
    {
        base.AI();

        if (!Main.remixWorld && Main.dayTime && Projectile.damage == 100)
        {
            Projectile.Kill();
        }

        if (Projectile.ai[1] == 0f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
        {
            Projectile.ai[1]     = 1f;
            Projectile.netUpdate = true;
        }

        if (Projectile.ai[1] != 0f)
        {
            Projectile.tileCollide = true;
        }

        if (Projectile.soundDelay == 0)
        {
            Projectile.soundDelay = 20 + Main.rand.Next(40);
            SoundEngine.PlaySound(in SoundID.Item9, Projectile.position);
        }

        if (Projectile.localAI[0] == 0f)
        {
            Projectile.localAI[0] = 1f;
        }

        Projectile.alpha += (int)(25f * Projectile.localAI[0]);

        if (Projectile.alpha > 200)
        {
            Projectile.alpha      = 200;
            Projectile.localAI[0] = -1f;
        }

        if (Projectile.alpha < 0)
        {
            Projectile.alpha      = 0;
            Projectile.localAI[0] = 1f;
        }

        Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.01f * Projectile.direction;

        var screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
        if (Projectile.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + screenSize / 2f, screenSize + new Vector2(400f))) && Main.rand.NextBool(6))
        {
            var starGore = Main.tenthAnniversaryWorld
                ? Utils.SelectRandom(Main.rand, 16, 16, 16, 17)
                : Utils.SelectRandom(Main.rand, 16, 17, 17, 17);

            Gore.NewGore(Projectile.position, Projectile.velocity * 0.2f, starGore);
        }

        Projectile.light = 0.9f;

        if (Main.rand.NextBool(20) || (Main.tenthAnniversaryWorld && Main.rand.NextBool(15)))
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Enchanted_Pink, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, default(Color), 1.2f);
        }
    }

    public override void OnKill(int timeLeft)
    {
        base.OnKill(timeLeft);

        SoundEngine.PlaySound(in SoundID.Item10, Projectile.position);

        var dustColor = Color.CornflowerBlue;
        if (Main.tenthAnniversaryWorld)
        {
            dustColor   =  Color.HotPink;
            dustColor.A /= 2;
        }

        for (var i = 0; i < 7; i++)
        {
            Dust.NewDust(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.Enchanted_Pink,
                Projectile.velocity.X * 0.1f,
                Projectile.velocity.Y * 0.1f,
                150,
                default(Color),
                0.8f
            );
        }
        for (var i = 0f; i < 1f; i += 0.125f)
        {
            Dust.NewDustPerfect(
                Projectile.Center,
                278,
                Vector2.UnitY.RotatedBy(i * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f),
                150,
                dustColor
            ).noGravity = true;
        }
        for (var i = 0f; i < 1f; i += 0.25f)
        {
            Dust.NewDustPerfect(
                Projectile.Center,
                278,
                Vector2.UnitY.RotatedBy(i * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f),
                150,
                Color.Gold
            ).noGravity = true;
        }

        var screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
        if (Projectile.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + screenSize / 2f, screenSize + new Vector2(400f))))
        {
            for (var i = 0; i < 7; i++)
            {
                Gore.NewGore(
                    Projectile.position,
                    Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * Projectile.velocity.Length(),
                    Utils.SelectRandom(Main.rand, 16, 17, 17, 17, 17, 17, 17, 17)
                );
            }
        }

        if (Projectile.damage == 100)
        {
            Item.NewItem(
                Projectile.GetItemSource_FromThis(),
                (int)Projectile.position.X,
                (int)Projectile.position.Y,
                Projectile.width,
                Projectile.height,
                ItemID.DirtBlock
            );
        }
    }

    public override void PostDraw(Color lightColor)
    {
        base.PostDraw(lightColor);

        Lighting.AddLight(
            (int)((Projectile.position.X + Projectile.width  / 2f) / 16f),
            (int)((Projectile.position.Y + Projectile.height / 2f) / 16f),
            Projectile.light * 0.9f,
            Projectile.light * 0.8f,
            Projectile.light * 0.1f
        );
    }

    public override Color? GetAlpha(Color lightColor)
    {
        return new Color(255, 255, 255, lightColor.A - Projectile.alpha);
    }

    public static int ProjType(int index)
    {
        return index switch
        {
            1 => ModContent.ProjectileType<StarFragmentProj1>(),
            2 => ModContent.ProjectileType<StarFragmentProj2>(),
            3 => ModContent.ProjectileType<StarFragmentProj3>(),
            4 => ModContent.ProjectileType<StarFragmentProj4>(),
            5 => ModContent.ProjectileType<StarFragmentProj5>(),
            _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
        };
    }
}

internal sealed class StarFragmentProjSpawner : ModProjectile
{
    public override string Texture => Assets.Images.Projectiles.StarFragmentSpawner.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Projectile.width, Projectile.height) = (16, 16);

        Projectile.aiStyle     = -1;
        Projectile.tileCollide = false;
        Projectile.penetrate   = -1;
        Projectile.alpha       = 255;
    }

    public override void AI()
    {
        base.AI();

        // Pick which star fragment to spawn.
        if (Projectile.ai[2] == 0f)
        {
            Projectile.ai[2]     = Main.rand.Next(0, 5) + 1;
            Projectile.netUpdate = true;
        }

        if (Main.dayTime && !Main.remixWorld)
        {
            Projectile.Kill();
            return;
        }

        if (Projectile.localAI[0] == 0f && Main.netMode != NetmodeID.Server)
        {
            Projectile.localAI[0] = 1f;

            if (Main.LocalPlayer.position.Y < Main.worldSurface * 16f)
            {
                Star.StarFall(Projectile.position.X);
            }
        }

        Projectile.ai[0] += (float)Main.desiredWorldEventsUpdateRate;

        if (Projectile.owner != Main.myPlayer || !(Projectile.ai[0] >= 180f))
        {
            return;
        }

        if (Projectile.ai[1] > -1f)
        {
            Projectile.velocity.X *= 0.35f;

            if (Projectile.Center.X < Main.player[(int)Projectile.ai[1]].Center.X)
            {
                Projectile.velocity.X = Math.Abs(Projectile.velocity.X);
            }
            else
            {
                Projectile.velocity.X = 0f - Math.Abs(Projectile.velocity.X);
            }
        }

        Projectile.NewProjectile(
            Projectile.GetProjectileSource_FromThis(),
            Projectile.position.X,
            Projectile.position.Y,
            Projectile.velocity.X,
            Projectile.velocity.Y,
            StarFragmentProj.ProjType((int)Projectile.ai[2]),
            100,
            5f,
            Main.myPlayer
        );
        Projectile.Kill();
    }
}