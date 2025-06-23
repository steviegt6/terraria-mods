using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Ammo;

internal sealed class ImpactBullet : ModItem
{
    private sealed class ImpactBulletProjectile : ModProjectile
    {
        public override string Texture => Assets.Images.Items.Ammo.ImpactBulletProjectile.KEY;

        public override void SetDefaults()
        {
            base.SetDefaults();

            (Projectile.width, Projectile.height) = (4, 4);

            Projectile.alpha = 255;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly   = true;

            Projectile.timeLeft             = 16;
            Projectile.penetrate            = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown  = -1;

            Projectile.extraUpdates = 16;
            Projectile.ignoreWater  = true;
        }

        public override void AI()
        {
            base.AI();

            if (Projectile.timeLeft != 16)
            {
                return;
            }

            for (var i = 0; i < 128; i++)
            {
                Dust.NewDustPerfect(
                    Projectile.position,
                    DustID.Torch,
                    Projectile.velocity.RotatedByRandom(0.2) * Main.rand.NextFloat(0.6f, 1.4f)
                );
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            base.ModifyDamageHitbox(ref hitbox);

            var size = (16 - Projectile.timeLeft) * 2;
            hitbox.Inflate(size, size);
        }
    }

    public override string Texture => Assets.Images.Items.Ammo.ImpactBullet.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (8, 16);

        Item.maxStack   = Item.CommonMaxStack;
        Item.damage     = 6;
        Item.DamageType = DamageClass.Ranged;
        Item.knockBack  = 1f;

        Item.consumable = true;
        Item.ammo       = AmmoID.Bullet;
        Item.shoot      = ModContent.ProjectileType<ImpactBulletProjectile>();
        Item.shootSpeed = 2f;

        Item.rare  = ItemRarityID.Lime;
        Item.value = Item.buyPrice(gold: 15);
    }
}