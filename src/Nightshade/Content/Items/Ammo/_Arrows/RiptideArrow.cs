using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Tomat.TML.Mod.Nightshade.Common.Hooks.ProjectileReflecting;

namespace Tomat.TML.Mod.Nightshade.Content.Items.Ammo;

internal sealed class RiptideArrow : ModItem
{
    internal sealed class ImpactBulletProjectile : ModProjectile, INPCReflectable
    {
        public override string Texture => $"{Mod.Name}/Assets/Images/Items/Ammo/ImpactBulletProjectile";

        public override void SetDefaults()
        {
            base.SetDefaults();

            (Projectile.width, Projectile.height) = (10, 10);

            Projectile.arrow = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 1200;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            base.AI();


        }

        public bool? CanBeReflected() => true;
    }

    public override string Texture => $"{Mod.Name}/Assets/Images/Items/Ammo/RiptideArrow";

    public override void SetDefaults()
    {
        base.SetDefaults();

        (Item.width, Item.height) = (10, 28);

        Item.maxStack = Item.CommonMaxStack;
        Item.damage = 6;
        Item.DamageType = DamageClass.Ranged;
        Item.knockBack = 1f;

        Item.consumable = true;
        Item.ammo = AmmoID.Arrow;
        Item.shoot = ModContent.ProjectileType<ImpactBulletProjectile>();
        Item.shootSpeed = 2f;

        Item.rare = ItemRarityID.Lime;
        Item.value = Item.sellPrice(gold: 15);
    }
}
