namespace Daybreak.Common.Features.Hooks;

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedType.Global
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// Hooks to generate for 'Terraria.ModLoader.GlobalItem':
//     System.Void Terraria.ModLoader.GlobalItem::OnCreated(Terraria.Item,Terraria.DataStructures.ItemCreationContext)
//     System.Void Terraria.ModLoader.GlobalItem::OnSpawn(Terraria.Item,Terraria.DataStructures.IEntitySource)
//     System.Int32 Terraria.ModLoader.GlobalItem::ChoosePrefix(Terraria.Item,Terraria.Utilities.UnifiedRandom)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalItem::PrefixChance(Terraria.Item,System.Int32,Terraria.Utilities.UnifiedRandom)
//     System.Boolean Terraria.ModLoader.GlobalItem::AllowPrefix(Terraria.Item,System.Int32)
//     System.Boolean Terraria.ModLoader.GlobalItem::CanUseItem(Terraria.Item,Terraria.Player)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalItem::CanAutoReuseItem(Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::UseStyle(Terraria.Item,Terraria.Player,Microsoft.Xna.Framework.Rectangle)
//     System.Void Terraria.ModLoader.GlobalItem::HoldStyle(Terraria.Item,Terraria.Player,Microsoft.Xna.Framework.Rectangle)
//     System.Void Terraria.ModLoader.GlobalItem::HoldItem(Terraria.Item,Terraria.Player)
//     System.Single Terraria.ModLoader.GlobalItem::UseTimeMultiplier(Terraria.Item,Terraria.Player)
//     System.Single Terraria.ModLoader.GlobalItem::UseAnimationMultiplier(Terraria.Item,Terraria.Player)
//     System.Single Terraria.ModLoader.GlobalItem::UseSpeedMultiplier(Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::GetHealLife(Terraria.Item,Terraria.Player,System.Boolean,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalItem::GetHealMana(Terraria.Item,Terraria.Player,System.Boolean,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalItem::ModifyManaCost(Terraria.Item,Terraria.Player,System.Single&,System.Single&)
//     System.Void Terraria.ModLoader.GlobalItem::OnMissingMana(Terraria.Item,Terraria.Player,System.Int32)
//     System.Void Terraria.ModLoader.GlobalItem::OnConsumeMana(Terraria.Item,Terraria.Player,System.Int32)
//     System.Void Terraria.ModLoader.GlobalItem::ModifyWeaponDamage(Terraria.Item,Terraria.Player,Terraria.ModLoader.StatModifier&)
//     System.Void Terraria.ModLoader.GlobalItem::ModifyResearchSorting(Terraria.Item,Terraria.ID.ContentSamples/CreativeHelper/ItemGroup&)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalItem::CanConsumeBait(Terraria.Player,Terraria.Item)
//     System.Boolean Terraria.ModLoader.GlobalItem::CanResearch(Terraria.Item)
//     System.Void Terraria.ModLoader.GlobalItem::OnResearched(Terraria.Item,System.Boolean)
//     System.Void Terraria.ModLoader.GlobalItem::ModifyWeaponKnockback(Terraria.Item,Terraria.Player,Terraria.ModLoader.StatModifier&)
//     System.Void Terraria.ModLoader.GlobalItem::ModifyWeaponCrit(Terraria.Item,Terraria.Player,System.Single&)
//     System.Boolean Terraria.ModLoader.GlobalItem::NeedsAmmo(Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::PickAmmo(Terraria.Item,Terraria.Item,Terraria.Player,System.Int32&,System.Single&,Terraria.ModLoader.StatModifier&,System.Single&)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalItem::CanChooseAmmo(Terraria.Item,Terraria.Item,Terraria.Player)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalItem::CanBeChosenAsAmmo(Terraria.Item,Terraria.Item,Terraria.Player)
//     System.Boolean Terraria.ModLoader.GlobalItem::CanConsumeAmmo(Terraria.Item,Terraria.Item,Terraria.Player)
//     System.Boolean Terraria.ModLoader.GlobalItem::CanBeConsumedAsAmmo(Terraria.Item,Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::OnConsumeAmmo(Terraria.Item,Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::OnConsumedAsAmmo(Terraria.Item,Terraria.Item,Terraria.Player)
//     System.Boolean Terraria.ModLoader.GlobalItem::CanShoot(Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::ModifyShootStats(Terraria.Item,Terraria.Player,Microsoft.Xna.Framework.Vector2&,Microsoft.Xna.Framework.Vector2&,System.Int32&,System.Int32&,System.Single&)
//     System.Boolean Terraria.ModLoader.GlobalItem::Shoot(Terraria.Item,Terraria.Player,Terraria.DataStructures.EntitySource_ItemUse_WithAmmo,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Vector2,System.Int32,System.Int32,System.Single)
//     System.Void Terraria.ModLoader.GlobalItem::UseItemHitbox(Terraria.Item,Terraria.Player,Microsoft.Xna.Framework.Rectangle&,System.Boolean&)
//     System.Void Terraria.ModLoader.GlobalItem::MeleeEffects(Terraria.Item,Terraria.Player,Microsoft.Xna.Framework.Rectangle)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalItem::CanCatchNPC(Terraria.Item,Terraria.NPC,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::OnCatchNPC(Terraria.Item,Terraria.NPC,Terraria.Player,System.Boolean)
//     System.Void Terraria.ModLoader.GlobalItem::ModifyItemScale(Terraria.Item,Terraria.Player,System.Single&)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalItem::CanHitNPC(Terraria.Item,Terraria.Player,Terraria.NPC)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalItem::CanMeleeAttackCollideWithNPC(Terraria.Item,Microsoft.Xna.Framework.Rectangle,Terraria.Player,Terraria.NPC)
//     System.Void Terraria.ModLoader.GlobalItem::ModifyHitNPC(Terraria.Item,Terraria.Player,Terraria.NPC,Terraria.NPC/HitModifiers&)
//     System.Void Terraria.ModLoader.GlobalItem::OnHitNPC(Terraria.Item,Terraria.Player,Terraria.NPC,Terraria.NPC/HitInfo,System.Int32)
//     System.Boolean Terraria.ModLoader.GlobalItem::CanHitPvp(Terraria.Item,Terraria.Player,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::ModifyHitPvp(Terraria.Item,Terraria.Player,Terraria.Player,Terraria.Player/HurtModifiers&)
//     System.Void Terraria.ModLoader.GlobalItem::OnHitPvp(Terraria.Item,Terraria.Player,Terraria.Player,Terraria.Player/HurtInfo)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalItem::UseItem(Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::UseAnimation(Terraria.Item,Terraria.Player)
//     System.Boolean Terraria.ModLoader.GlobalItem::ConsumeItem(Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::OnConsumeItem(Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::UseItemFrame(Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::HoldItemFrame(Terraria.Item,Terraria.Player)
//     System.Boolean Terraria.ModLoader.GlobalItem::AltFunctionUse(Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::UpdateInventory(Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::UpdateInfoAccessory(Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::UpdateEquip(Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::UpdateAccessory(Terraria.Item,Terraria.Player,System.Boolean)
//     System.Void Terraria.ModLoader.GlobalItem::UpdateVanity(Terraria.Item,Terraria.Player)
//     System.String Terraria.ModLoader.GlobalItem::IsArmorSet(Terraria.Item,Terraria.Item,Terraria.Item)
//     System.Void Terraria.ModLoader.GlobalItem::UpdateArmorSet(Terraria.Player,System.String)
//     System.String Terraria.ModLoader.GlobalItem::IsVanitySet(System.Int32,System.Int32,System.Int32)
//     System.Void Terraria.ModLoader.GlobalItem::PreUpdateVanitySet(Terraria.Player,System.String)
//     System.Void Terraria.ModLoader.GlobalItem::UpdateVanitySet(Terraria.Player,System.String)
//     System.Void Terraria.ModLoader.GlobalItem::ArmorSetShadows(Terraria.Player,System.String)
//     System.Void Terraria.ModLoader.GlobalItem::SetMatch(System.Int32,System.Int32,System.Boolean,System.Int32&,System.Boolean&)
//     System.Boolean Terraria.ModLoader.GlobalItem::CanRightClick(Terraria.Item)
//     System.Void Terraria.ModLoader.GlobalItem::RightClick(Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::ModifyItemLoot(Terraria.Item,Terraria.ModLoader.ItemLoot)
//     System.Boolean Terraria.ModLoader.GlobalItem::CanStack(Terraria.Item,Terraria.Item)
//     System.Boolean Terraria.ModLoader.GlobalItem::CanStackInWorld(Terraria.Item,Terraria.Item)
//     System.Void Terraria.ModLoader.GlobalItem::OnStack(Terraria.Item,Terraria.Item,System.Int32)
//     System.Void Terraria.ModLoader.GlobalItem::SplitStack(Terraria.Item,Terraria.Item,System.Int32)
//     System.Boolean Terraria.ModLoader.GlobalItem::ReforgePrice(Terraria.Item,System.Int32&,System.Boolean&)
//     System.Boolean Terraria.ModLoader.GlobalItem::CanReforge(Terraria.Item)
//     System.Void Terraria.ModLoader.GlobalItem::PreReforge(Terraria.Item)
//     System.Void Terraria.ModLoader.GlobalItem::PostReforge(Terraria.Item)
//     System.Void Terraria.ModLoader.GlobalItem::DrawArmorColor(Terraria.ModLoader.EquipType,System.Int32,Terraria.Player,System.Single,Microsoft.Xna.Framework.Color&,System.Int32&,Microsoft.Xna.Framework.Color&)
//     System.Void Terraria.ModLoader.GlobalItem::ArmorArmGlowMask(System.Int32,Terraria.Player,System.Single,System.Int32&,Microsoft.Xna.Framework.Color&)
//     System.Void Terraria.ModLoader.GlobalItem::VerticalWingSpeeds(Terraria.Item,Terraria.Player,System.Single&,System.Single&,System.Single&,System.Single&,System.Single&)
//     System.Void Terraria.ModLoader.GlobalItem::HorizontalWingSpeeds(Terraria.Item,Terraria.Player,System.Single&,System.Single&)
//     System.Boolean Terraria.ModLoader.GlobalItem::WingUpdate(System.Int32,Terraria.Player,System.Boolean)
//     System.Void Terraria.ModLoader.GlobalItem::Update(Terraria.Item,System.Single&,System.Single&)
//     System.Void Terraria.ModLoader.GlobalItem::PostUpdate(Terraria.Item)
//     System.Void Terraria.ModLoader.GlobalItem::GrabRange(Terraria.Item,Terraria.Player,System.Int32&)
//     System.Boolean Terraria.ModLoader.GlobalItem::GrabStyle(Terraria.Item,Terraria.Player)
//     System.Boolean Terraria.ModLoader.GlobalItem::CanPickup(Terraria.Item,Terraria.Player)
//     System.Boolean Terraria.ModLoader.GlobalItem::OnPickup(Terraria.Item,Terraria.Player)
//     System.Boolean Terraria.ModLoader.GlobalItem::ItemSpace(Terraria.Item,Terraria.Player)
//     System.Nullable`1<Microsoft.Xna.Framework.Color> Terraria.ModLoader.GlobalItem::GetAlpha(Terraria.Item,Microsoft.Xna.Framework.Color)
//     System.Boolean Terraria.ModLoader.GlobalItem::PreDrawInWorld(Terraria.Item,Microsoft.Xna.Framework.Graphics.SpriteBatch,Microsoft.Xna.Framework.Color,Microsoft.Xna.Framework.Color,System.Single&,System.Single&,System.Int32)
//     System.Void Terraria.ModLoader.GlobalItem::PostDrawInWorld(Terraria.Item,Microsoft.Xna.Framework.Graphics.SpriteBatch,Microsoft.Xna.Framework.Color,Microsoft.Xna.Framework.Color,System.Single,System.Single,System.Int32)
//     System.Boolean Terraria.ModLoader.GlobalItem::PreDrawInInventory(Terraria.Item,Microsoft.Xna.Framework.Graphics.SpriteBatch,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Rectangle,Microsoft.Xna.Framework.Color,Microsoft.Xna.Framework.Color,Microsoft.Xna.Framework.Vector2,System.Single)
//     System.Void Terraria.ModLoader.GlobalItem::PostDrawInInventory(Terraria.Item,Microsoft.Xna.Framework.Graphics.SpriteBatch,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Rectangle,Microsoft.Xna.Framework.Color,Microsoft.Xna.Framework.Color,Microsoft.Xna.Framework.Vector2,System.Single)
//     System.Nullable`1<Microsoft.Xna.Framework.Vector2> Terraria.ModLoader.GlobalItem::HoldoutOffset(System.Int32)
//     System.Nullable`1<Microsoft.Xna.Framework.Vector2> Terraria.ModLoader.GlobalItem::HoldoutOrigin(System.Int32)
//     System.Boolean Terraria.ModLoader.GlobalItem::CanEquipAccessory(Terraria.Item,Terraria.Player,System.Int32,System.Boolean)
//     System.Boolean Terraria.ModLoader.GlobalItem::CanAccessoryBeEquippedWith(Terraria.Item,Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalItem::ExtractinatorUse(System.Int32,System.Int32,System.Int32&,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalItem::CaughtFishStack(System.Int32,System.Int32&)
//     System.Boolean Terraria.ModLoader.GlobalItem::IsAnglerQuestAvailable(System.Int32)
//     System.Void Terraria.ModLoader.GlobalItem::AnglerChat(System.Int32,System.String&,System.String&)
//     System.Void Terraria.ModLoader.GlobalItem::AddRecipes()
//     System.Boolean Terraria.ModLoader.GlobalItem::PreDrawTooltip(Terraria.Item,System.Collections.ObjectModel.ReadOnlyCollection`1<Terraria.ModLoader.TooltipLine>,System.Int32&,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalItem::PostDrawTooltip(Terraria.Item,System.Collections.ObjectModel.ReadOnlyCollection`1<Terraria.ModLoader.DrawableTooltipLine>)
//     System.Boolean Terraria.ModLoader.GlobalItem::PreDrawTooltipLine(Terraria.Item,Terraria.ModLoader.DrawableTooltipLine,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalItem::PostDrawTooltipLine(Terraria.Item,Terraria.ModLoader.DrawableTooltipLine)
//     System.Void Terraria.ModLoader.GlobalItem::ModifyTooltips(Terraria.Item,System.Collections.Generic.List`1<Terraria.ModLoader.TooltipLine>)
//     System.Void Terraria.ModLoader.GlobalItem::SaveData(Terraria.Item,Terraria.ModLoader.IO.TagCompound)
//     System.Void Terraria.ModLoader.GlobalItem::LoadData(Terraria.Item,Terraria.ModLoader.IO.TagCompound)
//     System.Void Terraria.ModLoader.GlobalItem::NetSend(Terraria.Item,System.IO.BinaryWriter)
//     System.Void Terraria.ModLoader.GlobalItem::NetReceive(Terraria.Item,System.IO.BinaryReader)
public static partial class GlobalItemHooks
{
    public static partial class OnCreated
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.DataStructures.ItemCreationContext context
        );

        public static event Definition Event;
    }

    public static partial class OnSpawn
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.DataStructures.IEntitySource source
        );

        public static event Definition Event;
    }

    public static partial class ChoosePrefix
    {
        public delegate int Definition(
            Terraria.Item item,
            Terraria.Utilities.UnifiedRandom rand
        );

        public static event Definition Event;
    }

    public static partial class PrefixChance
    {
        public delegate bool? Definition(
            Terraria.Item item,
            int pre,
            Terraria.Utilities.UnifiedRandom rand
        );

        public static event Definition Event;
    }

    public static partial class AllowPrefix
    {
        public delegate bool Definition(
            Terraria.Item item,
            int pre
        );

        public static event Definition Event;
    }

    public static partial class CanUseItem
    {
        public delegate bool Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class CanAutoReuseItem
    {
        public delegate bool? Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class UseStyle
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            Microsoft.Xna.Framework.Rectangle heldItemFrame
        );

        public static event Definition Event;
    }

    public static partial class HoldStyle
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            Microsoft.Xna.Framework.Rectangle heldItemFrame
        );

        public static event Definition Event;
    }

    public static partial class HoldItem
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class UseTimeMultiplier
    {
        public delegate float Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class UseAnimationMultiplier
    {
        public delegate float Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class UseSpeedMultiplier
    {
        public delegate float Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class GetHealLife
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            bool quickHeal,
            ref int healValue
        );

        public static event Definition Event;
    }

    public static partial class GetHealMana
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            bool quickHeal,
            ref int healValue
        );

        public static event Definition Event;
    }

    public static partial class ModifyManaCost
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            ref float reduce,
            ref float mult
        );

        public static event Definition Event;
    }

    public static partial class OnMissingMana
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            int neededMana
        );

        public static event Definition Event;
    }

    public static partial class OnConsumeMana
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            int manaConsumed
        );

        public static event Definition Event;
    }

    public static partial class ModifyWeaponDamage
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            ref Terraria.ModLoader.StatModifier damage
        );

        public static event Definition Event;
    }

    public static partial class ModifyResearchSorting
    {
        public delegate void Definition(
            Terraria.Item item,
            ref Terraria.ID.ContentSamples.CreativeHelper.ItemGroup itemGroup
        );

        public static event Definition Event;
    }

    public static partial class CanConsumeBait
    {
        public delegate bool? Definition(
            Terraria.Player player,
            Terraria.Item bait
        );

        public static event Definition Event;
    }

    public static partial class CanResearch
    {
        public delegate bool Definition(
            Terraria.Item item
        );

        public static event Definition Event;
    }

    public static partial class OnResearched
    {
        public delegate void Definition(
            Terraria.Item item,
            bool fullyResearched
        );

        public static event Definition Event;
    }

    public static partial class ModifyWeaponKnockback
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            ref Terraria.ModLoader.StatModifier knockback
        );

        public static event Definition Event;
    }

    public static partial class ModifyWeaponCrit
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            ref float crit
        );

        public static event Definition Event;
    }

    public static partial class NeedsAmmo
    {
        public delegate bool Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class PickAmmo
    {
        public delegate void Definition(
            Terraria.Item weapon,
            Terraria.Item ammo,
            Terraria.Player player,
            ref int type,
            ref float speed,
            ref Terraria.ModLoader.StatModifier damage,
            ref float knockback
        );

        public static event Definition Event;
    }

    public static partial class CanChooseAmmo
    {
        public delegate bool? Definition(
            Terraria.Item weapon,
            Terraria.Item ammo,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class CanBeChosenAsAmmo
    {
        public delegate bool? Definition(
            Terraria.Item ammo,
            Terraria.Item weapon,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class CanConsumeAmmo
    {
        public delegate bool Definition(
            Terraria.Item weapon,
            Terraria.Item ammo,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class CanBeConsumedAsAmmo
    {
        public delegate bool Definition(
            Terraria.Item ammo,
            Terraria.Item weapon,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class OnConsumeAmmo
    {
        public delegate void Definition(
            Terraria.Item weapon,
            Terraria.Item ammo,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class OnConsumedAsAmmo
    {
        public delegate void Definition(
            Terraria.Item ammo,
            Terraria.Item weapon,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class CanShoot
    {
        public delegate bool Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class ModifyShootStats
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            ref Microsoft.Xna.Framework.Vector2 position,
            ref Microsoft.Xna.Framework.Vector2 velocity,
            ref int type,
            ref int damage,
            ref float knockback
        );

        public static event Definition Event;
    }

    public static partial class Shoot
    {
        public delegate bool Definition(
            Terraria.Item item,
            Terraria.Player player,
            Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Vector2 velocity,
            int type,
            int damage,
            float knockback
        );

        public static event Definition Event;
    }

    public static partial class UseItemHitbox
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            ref Microsoft.Xna.Framework.Rectangle hitbox,
            ref bool noHitbox
        );

        public static event Definition Event;
    }

    public static partial class MeleeEffects
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            Microsoft.Xna.Framework.Rectangle hitbox
        );

        public static event Definition Event;
    }

    public static partial class CanCatchNPC
    {
        public delegate bool? Definition(
            Terraria.Item item,
            Terraria.NPC target,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class OnCatchNPC
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.NPC npc,
            Terraria.Player player,
            bool failed
        );

        public static event Definition Event;
    }

    public static partial class ModifyItemScale
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            ref float scale
        );

        public static event Definition Event;
    }

    public static partial class CanHitNPC
    {
        public delegate bool? Definition(
            Terraria.Item item,
            Terraria.Player player,
            Terraria.NPC target
        );

        public static event Definition Event;
    }

    public static partial class CanMeleeAttackCollideWithNPC
    {
        public delegate bool? Definition(
            Terraria.Item item,
            Microsoft.Xna.Framework.Rectangle meleeAttackHitbox,
            Terraria.Player player,
            Terraria.NPC target
        );

        public static event Definition Event;
    }

    public static partial class ModifyHitNPC
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            Terraria.NPC target,
            ref Terraria.NPC.HitModifiers modifiers
        );

        public static event Definition Event;
    }

    public static partial class OnHitNPC
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            Terraria.NPC target,
            Terraria.NPC.HitInfo hit,
            int damageDone
        );

        public static event Definition Event;
    }

    public static partial class CanHitPvp
    {
        public delegate bool Definition(
            Terraria.Item item,
            Terraria.Player player,
            Terraria.Player target
        );

        public static event Definition Event;
    }

    public static partial class ModifyHitPvp
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            Terraria.Player target,
            ref Terraria.Player.HurtModifiers modifiers
        );

        public static event Definition Event;
    }

    public static partial class OnHitPvp
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            Terraria.Player target,
            Terraria.Player.HurtInfo hurtInfo
        );

        public static event Definition Event;
    }

    public static partial class UseItem
    {
        public delegate bool? Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class UseAnimation
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class ConsumeItem
    {
        public delegate bool Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class OnConsumeItem
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class UseItemFrame
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class HoldItemFrame
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class AltFunctionUse
    {
        public delegate bool Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class UpdateInventory
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class UpdateInfoAccessory
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class UpdateEquip
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class UpdateAccessory
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            bool hideVisual
        );

        public static event Definition Event;
    }

    public static partial class UpdateVanity
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class IsArmorSet
    {
        public delegate string Definition(
            Terraria.Item head,
            Terraria.Item body,
            Terraria.Item legs
        );

        public static event Definition Event;
    }

    public static partial class UpdateArmorSet
    {
        public delegate void Definition(
            Terraria.Player player,
            string set
        );

        public static event Definition Event;
    }

    public static partial class IsVanitySet
    {
        public delegate string Definition(
            int head,
            int body,
            int legs
        );

        public static event Definition Event;
    }

    public static partial class PreUpdateVanitySet
    {
        public delegate void Definition(
            Terraria.Player player,
            string set
        );

        public static event Definition Event;
    }

    public static partial class UpdateVanitySet
    {
        public delegate void Definition(
            Terraria.Player player,
            string set
        );

        public static event Definition Event;
    }

    public static partial class ArmorSetShadows
    {
        public delegate void Definition(
            Terraria.Player player,
            string set
        );

        public static event Definition Event;
    }

    public static partial class SetMatch
    {
        public delegate void Definition(
            int armorSlot,
            int type,
            bool male,
            ref int equipSlot,
            ref bool robes
        );

        public static event Definition Event;
    }

    public static partial class CanRightClick
    {
        public delegate bool Definition(
            Terraria.Item item
        );

        public static event Definition Event;
    }

    public static partial class RightClick
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class ModifyItemLoot
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.ModLoader.ItemLoot itemLoot
        );

        public static event Definition Event;
    }

    public static partial class CanStack
    {
        public delegate bool Definition(
            Terraria.Item destination,
            Terraria.Item source
        );

        public static event Definition Event;
    }

    public static partial class CanStackInWorld
    {
        public delegate bool Definition(
            Terraria.Item destination,
            Terraria.Item source
        );

        public static event Definition Event;
    }

    public static partial class OnStack
    {
        public delegate void Definition(
            Terraria.Item destination,
            Terraria.Item source,
            int numToTransfer
        );

        public static event Definition Event;
    }

    public static partial class SplitStack
    {
        public delegate void Definition(
            Terraria.Item destination,
            Terraria.Item source,
            int numToTransfer
        );

        public static event Definition Event;
    }

    public static partial class ReforgePrice
    {
        public delegate bool Definition(
            Terraria.Item item,
            ref int reforgePrice,
            ref bool canApplyDiscount
        );

        public static event Definition Event;
    }

    public static partial class CanReforge
    {
        public delegate bool Definition(
            Terraria.Item item
        );

        public static event Definition Event;
    }

    public static partial class PreReforge
    {
        public delegate void Definition(
            Terraria.Item item
        );

        public static event Definition Event;
    }

    public static partial class PostReforge
    {
        public delegate void Definition(
            Terraria.Item item
        );

        public static event Definition Event;
    }

    public static partial class DrawArmorColor
    {
        public delegate void Definition(
            Terraria.ModLoader.EquipType type,
            int slot,
            Terraria.Player drawPlayer,
            float shadow,
            ref Microsoft.Xna.Framework.Color color,
            ref int glowMask,
            ref Microsoft.Xna.Framework.Color glowMaskColor
        );

        public static event Definition Event;
    }

    public static partial class ArmorArmGlowMask
    {
        public delegate void Definition(
            int slot,
            Terraria.Player drawPlayer,
            float shadow,
            ref int glowMask,
            ref Microsoft.Xna.Framework.Color color
        );

        public static event Definition Event;
    }

    public static partial class VerticalWingSpeeds
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            ref float ascentWhenFalling,
            ref float ascentWhenRising,
            ref float maxCanAscendMultiplier,
            ref float maxAscentMultiplier,
            ref float constantAscend
        );

        public static event Definition Event;
    }

    public static partial class HorizontalWingSpeeds
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            ref float speed,
            ref float acceleration
        );

        public static event Definition Event;
    }

    public static partial class WingUpdate
    {
        public delegate bool Definition(
            int wings,
            Terraria.Player player,
            bool inUse
        );

        public static event Definition Event;
    }

    public static partial class Update
    {
        public delegate void Definition(
            Terraria.Item item,
            ref float gravity,
            ref float maxFallSpeed
        );

        public static event Definition Event;
    }

    public static partial class PostUpdate
    {
        public delegate void Definition(
            Terraria.Item item
        );

        public static event Definition Event;
    }

    public static partial class GrabRange
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.Player player,
            ref int grabRange
        );

        public static event Definition Event;
    }

    public static partial class GrabStyle
    {
        public delegate bool Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class CanPickup
    {
        public delegate bool Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class OnPickup
    {
        public delegate bool Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class ItemSpace
    {
        public delegate bool Definition(
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class GetAlpha
    {
        public delegate Microsoft.Xna.Framework.Color? Definition(
            Terraria.Item item,
            Microsoft.Xna.Framework.Color lightColor
        );

        public static event Definition Event;
    }

    public static partial class PreDrawInWorld
    {
        public delegate bool Definition(
            Terraria.Item item,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Microsoft.Xna.Framework.Color lightColor,
            Microsoft.Xna.Framework.Color alphaColor,
            ref float rotation,
            ref float scale,
            int whoAmI
        );

        public static event Definition Event;
    }

    public static partial class PostDrawInWorld
    {
        public delegate void Definition(
            Terraria.Item item,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Microsoft.Xna.Framework.Color lightColor,
            Microsoft.Xna.Framework.Color alphaColor,
            float rotation,
            float scale,
            int whoAmI
        );

        public static event Definition Event;
    }

    public static partial class PreDrawInInventory
    {
        public delegate bool Definition(
            Terraria.Item item,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle frame,
            Microsoft.Xna.Framework.Color drawColor,
            Microsoft.Xna.Framework.Color itemColor,
            Microsoft.Xna.Framework.Vector2 origin,
            float scale
        );

        public static event Definition Event;
    }

    public static partial class PostDrawInInventory
    {
        public delegate void Definition(
            Terraria.Item item,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Rectangle frame,
            Microsoft.Xna.Framework.Color drawColor,
            Microsoft.Xna.Framework.Color itemColor,
            Microsoft.Xna.Framework.Vector2 origin,
            float scale
        );

        public static event Definition Event;
    }

    public static partial class HoldoutOffset
    {
        public delegate Microsoft.Xna.Framework.Vector2? Definition(
            int type
        );

        public static event Definition Event;
    }

    public static partial class HoldoutOrigin
    {
        public delegate Microsoft.Xna.Framework.Vector2? Definition(
            int type
        );

        public static event Definition Event;
    }

    public static partial class CanEquipAccessory
    {
        public delegate bool Definition(
            Terraria.Item item,
            Terraria.Player player,
            int slot,
            bool modded
        );

        public static event Definition Event;
    }

    public static partial class CanAccessoryBeEquippedWith
    {
        public delegate bool Definition(
            Terraria.Item equippedItem,
            Terraria.Item incomingItem,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class ExtractinatorUse
    {
        public delegate void Definition(
            int extractType,
            int extractinatorBlockType,
            ref int resultType,
            ref int resultStack
        );

        public static event Definition Event;
    }

    public static partial class CaughtFishStack
    {
        public delegate void Definition(
            int type,
            ref int stack
        );

        public static event Definition Event;
    }

    public static partial class IsAnglerQuestAvailable
    {
        public delegate bool Definition(
            int type
        );

        public static event Definition Event;
    }

    public static partial class AnglerChat
    {
        public delegate void Definition(
            int type,
            ref string chat,
            ref string catchLocation
        );

        public static event Definition Event;
    }

    public static partial class AddRecipes
    {
        public delegate void Definition();


        public static event Definition Event;
    }

    public static partial class PreDrawTooltip
    {
        public delegate bool Definition(
            Terraria.Item item,
            System.Collections.ObjectModel.ReadOnlyCollection<Terraria.ModLoader.TooltipLine> lines,
            ref int x,
            ref int y
        );

        public static event Definition Event;
    }

    public static partial class PostDrawTooltip
    {
        public delegate void Definition(
            Terraria.Item item,
            System.Collections.ObjectModel.ReadOnlyCollection<Terraria.ModLoader.DrawableTooltipLine> lines
        );

        public static event Definition Event;
    }

    public static partial class PreDrawTooltipLine
    {
        public delegate bool Definition(
            Terraria.Item item,
            Terraria.ModLoader.DrawableTooltipLine line,
            ref int yOffset
        );

        public static event Definition Event;
    }

    public static partial class PostDrawTooltipLine
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.ModLoader.DrawableTooltipLine line
        );

        public static event Definition Event;
    }

    public static partial class ModifyTooltips
    {
        public delegate void Definition(
            Terraria.Item item,
            System.Collections.Generic.List<Terraria.ModLoader.TooltipLine> tooltips
        );

        public static event Definition Event;
    }

    public static partial class SaveData
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.ModLoader.IO.TagCompound tag
        );

        public static event Definition Event;
    }

    public static partial class LoadData
    {
        public delegate void Definition(
            Terraria.Item item,
            Terraria.ModLoader.IO.TagCompound tag
        );

        public static event Definition Event;
    }

    public static partial class NetSend
    {
        public delegate void Definition(
            Terraria.Item item,
            System.IO.BinaryWriter writer
        );

        public static event Definition Event;
    }

    public static partial class NetReceive
    {
        public delegate void Definition(
            Terraria.Item item,
            System.IO.BinaryReader reader
        );

        public static event Definition Event;
    }

}
