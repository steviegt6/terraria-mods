using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Nightshade.Content.Items.Weapons;

internal sealed class MoldFlailItem : ModItem
{
    public sealed class MoldFlailProjectile : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];

        private float FrameCount
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override string Texture => Assets.Images.Items.Weapons.MoldFlailProjectile.KEY;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            (Projectile.width, Projectile.height) = (20, 20);

            Projectile.netImportant = true;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            base.AI();

            FrameCount++;

#region Update position in world
            Projectile.Center = Owner.MountedCenter;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.spriteDirection = Projectile.direction;

            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;

            Owner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
#endregion

            if (Projectile.owner == Main.myPlayer)
            {
                if (!Owner.channel || !Owner.active || Owner.dead || Owner.noItems || Owner.CCed)
                {
                    Projectile.Kill();
                    return;
                }
                else
                {
#region Handle projectile aim
                    Vector2 aim = Vector2.Normalize(Main.MouseWorld - Owner.MountedCenter);
                    if (aim.HasNaNs())
                    {
                        aim = -Vector2.UnitY;
                    }

                    aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, 0.1f));
                    aim *= 5f;

                    if (aim != Projectile.velocity)
                    {
                        Projectile.netUpdate = true;
                    }
                    Projectile.velocity = aim;
#endregion
                }
            }
        }
    }

    public override string Texture => Assets.Images.Items.Weapons.MoldFlailItem.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (52, 58);

        Item.useTime = 10;
        Item.useAnimation = 10;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.reuseDelay = 5;
        Item.channel = true;
        Item.UseSound = SoundID.Item1;

        Item.damage = 15;
        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.knockBack = 4.6f;

        Item.noUseGraphic = true;
        Item.noMelee = true;

        Item.shoot = ModContent.ProjectileType<MoldFlailProjectile>();
        Item.shootSpeed = 11f;

        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(gold: 2);
    }

    public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
}
