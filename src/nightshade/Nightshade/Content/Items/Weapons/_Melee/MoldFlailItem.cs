using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Nightshade.Common.Loading;
using Steamworks;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Nightshade.Content.Items.Weapons;

internal sealed class MoldFlailItem : ModItem
{
    public sealed class MoldFlailProjectile : ModProjectile
    {
        private const float vertical_knockback_multiplier = 8f;

        private Player Owner => Main.player[Projectile.owner];

        private int FrameCount
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public Vector2 SavedEnemyVelocity
        {
            get => new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
            set
            {
                Projectile.localAI[0] = value.X;
                Projectile.localAI[1] = value.Y;
            }
        }

        private int TotalSwingTime => (int)(70f / Owner.GetTotalAttackSpeed(Projectile.DamageType));

        private int BeginningTimeOfSwingUpwards => 0;
        private int EndingTimeOfSwingUpwards => (int)(0.4f * TotalSwingTime);
        private int BeginningTimeOfSwingDownwards => EndingTimeOfSwingUpwards;
        private int EndingTimeOfSwingDownwards => TotalSwingTime;

        private bool IsSwingingDownwards => FrameCount > EndingTimeOfSwingUpwards;

        private int TimeUntilSwingUpwardsCanNotDealDamage => (int)(0.3f * TotalSwingTime);
        private int TimeUntilSwingDownwardsCanDealDamage => (int)(0.6f * TotalSwingTime);
        private int TimeUntilSwingDownwardsCanNotDealDamage => (int)(0.85f * TotalSwingTime);


        private static float InQuad(float t) => t * t;
        private static float OutQuad(float t) => 1 - InQuad(1 - t);

        private static float InBack(float t)
        {
            float s = 2.5923889015163f; //20%
            return t * t * ((s + 1) * t - s);
        }
        private static float InOutBack(float t)
        {
            if (t < 0.5) return InBack(t * 2) / 2;
            return 1 - InBack((1 - t) * 2) / 2;
        }


        public override string Texture => Assets.Images.Items.Weapons.MoldFlailItem.KEY;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            (Projectile.width, Projectile.height) = (56, 56);

            Projectile.netImportant = true;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);

            Projectile.spriteDirection = Main.MouseWorld.X > Owner.MountedCenter.X ? 1 : -1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);

            writer.Write((sbyte)Projectile.spriteDirection);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);

            Projectile.spriteDirection = reader.ReadSByte();
        }

        public override void AI()
        {
            base.AI();

            if (!Owner.active || Owner.dead || Owner.noItems || Owner.CCed)
            {
                Projectile.Kill();
            }

            if (FrameCount > TotalSwingTime)
            {
                Projectile.Kill();
                return;
            }

            float initialAngle = -MathHelper.PiOver2 - MathHelper.Pi * Projectile.spriteDirection;

            float angleChange;
            if (!IsSwingingDownwards)
            {
                float upwardsLerpValue = Utils.GetLerpValue(BeginningTimeOfSwingUpwards, EndingTimeOfSwingUpwards, FrameCount);
                upwardsLerpValue = OutQuad(upwardsLerpValue);
                angleChange = MathHelper.Lerp(MathHelper.Pi * -0.2f, MathHelper.Pi * 1.3f, upwardsLerpValue);
            }
            else
            {
                float downwardsLerpValue = Utils.GetLerpValue(BeginningTimeOfSwingDownwards, EndingTimeOfSwingDownwards, FrameCount);
                downwardsLerpValue = InOutBack(downwardsLerpValue);
                angleChange = MathHelper.Lerp(MathHelper.Pi * 1.3f, MathHelper.Pi * -0.2f, downwardsLerpValue);
            }
            angleChange *= Projectile.spriteDirection;
            Projectile.rotation = initialAngle - angleChange;

            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            //Owner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, (Projectile.rotation - MathHelper.PiOver2) * 1.1f);
            Vector2 playerCenter = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            playerCenter.Y += Owner.gfxOffY;
            Projectile.Center = playerCenter;
            Projectile.scale = 1.2f * Owner.GetAdjustedItemScale(Owner.HeldItem);

            Owner.heldProj = Projectile.whoAmI;
            Owner.SetDummyItemTime(2);

            if (FrameCount == EndingTimeOfSwingUpwards)
            {
                Projectile.ResetLocalNPCHitImmunity();
            }

            FrameCount++;
        }

        public override bool ShouldUpdatePosition() => false;

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Owner.MountedCenter;
            Vector2 end = start + Projectile.rotation.ToRotationVector2() * ((Projectile.Size.Length()) * Projectile.scale);
            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 15f * Projectile.scale, ref collisionPoint);
        }

        public override void CutTiles()
        {
            Vector2 start = Owner.MountedCenter;
            Vector2 end = start + Projectile.rotation.ToRotationVector2() * (Projectile.Size.Length() * Projectile.scale);
            Utils.PlotTileLine(start, end, 15 * Projectile.scale, DelegateMethods.CutTiles);
        }

        public override bool? CanDamage() => (FrameCount < TimeUntilSwingUpwardsCanNotDealDamage) || (FrameCount > TimeUntilSwingDownwardsCanDealDamage && FrameCount < TimeUntilSwingDownwardsCanNotDealDamage);

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);

            modifiers.HitDirectionOverride = Projectile.spriteDirection;
            SavedEnemyVelocity = target.velocity;
            if (IsSwingingDownwards)
            {
                modifiers.Knockback += 0.6f;
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            base.ModifyHitPlayer(target, ref modifiers);

            modifiers.HitDirectionOverride = Projectile.spriteDirection;
            SavedEnemyVelocity = target.velocity;
            if (IsSwingingDownwards)
            {
                modifiers.Knockback += 0.6f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (hit.Knockback <= 0)
            {
                return;
            }

            if (!IsSwingingDownwards)
            {
                target.velocity.X = 1f * hit.HitDirection;
                target.velocity.Y = -8f;
                if (target.noGravity)
                {
                    target.velocity.Y *= 0.75f;
                }
            }

            target.netUpdate = true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            base.OnHitPlayer(target, info);

            if (target.noKnockback)
            {
                return;
            }

            if (info.Knockback != 0f && info.HitDirection != 0 && (!target.mount.Active || !target.mount.Cart))
            {
                target.velocity.X = info.Knockback * info.HitDirection;
                target.velocity.Y = info.Knockback * vertical_knockback_multiplier;
                target.fallStart = (int)(target.position.Y / 16f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 origin;
            float rotationOffset;
            SpriteEffects effects;

            if (Projectile.spriteDirection > 0)
            {
                origin = new Vector2(0, Projectile.height);
                rotationOffset = MathHelper.ToRadians(45f);
                effects = SpriteEffects.None;
            }
            else
            {
                origin = new Vector2(Projectile.width, Projectile.height);
                rotationOffset = MathHelper.ToRadians(135f);
                effects = SpriteEffects.FlipHorizontally;
            }

            Vector2 playerCenter = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Draw(texture, playerCenter - Main.screenPosition, default, lightColor * Projectile.Opacity, Projectile.rotation + rotationOffset, origin, Projectile.scale, effects, 0);

            return false;
        }
    }

    public override string Texture => Assets.Images.Items.Weapons.MoldFlailItem.KEY;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (56, 56);

        Item.useTime = Item.useAnimation = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.UseSound = SoundID.Item1;

        Item.damage = 15;
        Item.DamageType = DamageClass.Melee;
        Item.knockBack = 12f;

        Item.noUseGraphic = true;
        Item.noMelee = true;

        Item.shoot = ModContent.ProjectileType<MoldFlailProjectile>();

        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(gold: 2);
    }

    public override bool MeleePrefix()
    {
        return true;
    }
}
