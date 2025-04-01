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
        public const int max_active_frame_count = 60 * 5;
        public const int min_active_frame_count_to_deal_damage = 60 * 3;
        public const int frame_count_difference_when_swinging = 1;
        public const int frame_count_difference_when_not_swinging = -5;
        public const float max_projectile_responsiveness = 0.5f;
        public const float distance_from_player = 35f;

        public Player Owner => Main.player[Projectile.owner];

        private int ActiveFrameCount
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public Vector2 PreviousAim
        {
            get => new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
            set
            {
                Projectile.localAI[0] = value.X;
                Projectile.localAI[1] = value.Y;
            }
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

            (Projectile.width, Projectile.height) = (60, 60);

            Projectile.netImportant = true;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI()
        {
            base.AI();

            if (!Owner.channel || !Owner.active || Owner.dead || Owner.noItems || Owner.CCed)
            {
                Projectile.Kill();
            }

            if (Projectile.owner == Main.myPlayer)
            {
                #region Handle projectile aim
                Vector2 normalizedAim = Vector2.Normalize(Main.MouseWorld - Owner.MountedCenter);
                if (normalizedAim.HasNaNs())
                {
                    normalizedAim = -Vector2.UnitY;
                }

                ActiveFrameCount += (normalizedAim != PreviousAim) ? frame_count_difference_when_swinging : frame_count_difference_when_not_swinging;
                ActiveFrameCount = MathHelper.Clamp(ActiveFrameCount, 0, max_active_frame_count);
                float aimLerpValue = Utils.Remap(ActiveFrameCount, 0, max_active_frame_count, 0f, max_projectile_responsiveness);
                PreviousAim = normalizedAim;
                //Main.NewText(ActiveFrameCount);

                Vector2 aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), normalizedAim, aimLerpValue));
                aim *= distance_from_player;

                if (aim != Projectile.velocity)
                {
                    Projectile.netUpdate = true;
                }
                Projectile.velocity = aim;
                #endregion
            }

            #region Update position in world
            Projectile.Center = Owner.MountedCenter;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.spriteDirection = Projectile.direction;
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.PiOver4;
            }
            else
            {
                Projectile.rotation -= MathHelper.PiOver4;
            }

            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;
            Owner.SetDummyItemTime(2);

            Owner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
            #endregion
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);

            float damageMult = Utils.Remap(ActiveFrameCount, min_active_frame_count_to_deal_damage, max_active_frame_count, 0f, 1f);
            modifiers.SourceDamage *= damageMult;
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

        Item.damage = 40;
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
