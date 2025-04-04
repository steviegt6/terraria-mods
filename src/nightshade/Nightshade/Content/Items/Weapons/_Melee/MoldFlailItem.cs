using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Nightshade.Common.Loading;
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
        private const float vertical_knockback_multiplier = 3f;
        private Player Owner => Main.player[Projectile.owner];

        private float FrameCount
        {
            get => Projectile.ai[0];
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

        private float TotalSwingTime => 30f / Owner.GetTotalAttackSpeed(Projectile.DamageType);

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

            float initialAngle = -MathHelper.PiOver2 - (MathHelper.Pi * 0.85f) * Projectile.spriteDirection;
            float angleLerpValue = Utils.GetLerpValue(0, TotalSwingTime, FrameCount, true);
            angleLerpValue = MathF.Sin((angleLerpValue * MathF.PI) / 2f);
            float angleChange = MathHelper.Lerp(0, MathHelper.Pi * 1.2f, angleLerpValue) * Projectile.spriteDirection;
            Projectile.rotation = initialAngle - angleChange;

            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            Vector2 playerCenter = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            playerCenter.Y += Owner.gfxOffY;
            Projectile.Center = playerCenter;
            Projectile.scale = 1.2f * Owner.GetAdjustedItemScale(Owner.HeldItem);

            Owner.heldProj = Projectile.whoAmI;
            Owner.SetDummyItemTime(2);

            int spriteWidth = Assets.Images.Items.Weapons.MoldFlailItem.Asset.Width();
            int spriteHeight = Assets.Images.Items.Weapons.MoldFlailItem.Asset.Height();

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

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);

            modifiers.HitDirectionOverride = (Main.player[Projectile.owner].Center.X < target.Center.X).ToDirectionInt();
            SavedEnemyVelocity = target.velocity;

            if (!WorldUtils.Find(Projectile.Center.ToTileCoordinates(), Searches.Chain(new Searches.Down(12), Projectile._cachedConditions_notNull, Projectile._cachedConditions_solid), out var _))
            {
                modifiers.FinalDamage *= 1.5f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            target.velocity = SavedEnemyVelocity;
            float finalKnockback = hit.Knockback;

            if (finalKnockback <= 0) return;

#region Apply knockback scaling & critical hit increase
            if (finalKnockback > 8f)
            {
                float num4 = finalKnockback - 8f;
                num4 *= 0.9f;
                finalKnockback = 8f + num4;
            }
            if (finalKnockback > 10f)
            {
                float num5 = finalKnockback - 10f;
                num5 *= 0.8f;
                finalKnockback = 10f + num5;
            }
            if (finalKnockback > 12f)
            {
                float num6 = finalKnockback - 12f;
                num6 *= 0.7f;
                finalKnockback = 12f + num6;
            }
            if (finalKnockback > 14f)
            {
                float num7 = finalKnockback - 14f;
                num7 *= 0.6f;
                finalKnockback = 14f + num7;
            }
            if (finalKnockback > 16f)
            {
                finalKnockback = 16f;
            }
            if (hit.Crit)
            {
                finalKnockback *= 1.4f;
            }
#endregion

            int damageDoneBig = damageDone * (Main.expertMode ? 15 : 10);
            if (damageDoneBig > target.lifeMax)
            {
                #region Horizontal knockback done when damage is big
                if (hit.HitDirection < 0 && target.velocity.X > -finalKnockback)
                {
                    if (target.velocity.X > 0f)
                    {
                        target.velocity.X -= finalKnockback;
                    }
                    target.velocity.X -= finalKnockback;
                    if (target.velocity.X < -finalKnockback)
                    {
                        target.velocity.X = -finalKnockback;
                    }
                }
                else if (hit.HitDirection > 0 && target.velocity.X < finalKnockback)
                {
                    if (target.velocity.X < 0f)
                    {
                        target.velocity.X += finalKnockback;
                    }
                    target.velocity.X += finalKnockback;
                    if (target.velocity.X > finalKnockback)
                    {
                        target.velocity.X = finalKnockback;
                    }
                }
                #endregion

                #region Vertical knockback done when damage is big
                if (target.type == NPCID.SnowFlinx)
                {
                    finalKnockback *= 1.5f;
                }
                finalKnockback *= vertical_knockback_multiplier;
                finalKnockback = (target.noGravity ? (finalKnockback * -0.5f) : (finalKnockback * -0.75f));
                if (target.velocity.Y > finalKnockback)
                {
                    target.velocity.Y += finalKnockback;
                    if (target.velocity.Y < finalKnockback)
                    {
                        target.velocity.Y = finalKnockback;
                    }
                }
                #endregion
            }
            else
            {
                target.velocity.Y = finalKnockback * vertical_knockback_multiplier * (target.noGravity ? -0.5f : -0.75f) * target.knockBackResist;
                target.velocity.X = finalKnockback * hit.HitDirection * target.knockBackResist;
            }
            target.netUpdate = true;
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

            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
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

        Item.damage = 5;
        Item.DamageType = DamageClass.Melee;
        Item.knockBack = 6f;

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
