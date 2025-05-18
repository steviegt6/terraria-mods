namespace Daybreak.Common.Features.Hooks;

using System.Linq;

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// Hooks to generate for 'Terraria.ModLoader.ModPlayer':
//     Terraria.ModLoader.ModPlayer Terraria.ModLoader.ModPlayer::NewInstance(Terraria.Player)
//     System.Void Terraria.ModLoader.ModPlayer::Initialize()
//     System.Void Terraria.ModLoader.ModPlayer::ResetEffects()
//     System.Void Terraria.ModLoader.ModPlayer::ResetInfoAccessories()
//     System.Void Terraria.ModLoader.ModPlayer::RefreshInfoAccessoriesFromTeamPlayers(Terraria.Player)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyMaxStats(Terraria.ModLoader.StatModifier&,Terraria.ModLoader.StatModifier&)
//     System.Void Terraria.ModLoader.ModPlayer::UpdateDead()
//     System.Void Terraria.ModLoader.ModPlayer::PreSaveCustomData()
//     System.Void Terraria.ModLoader.ModPlayer::SaveData(Terraria.ModLoader.IO.TagCompound)
//     System.Void Terraria.ModLoader.ModPlayer::LoadData(Terraria.ModLoader.IO.TagCompound)
//     System.Void Terraria.ModLoader.ModPlayer::PreSavePlayer()
//     System.Void Terraria.ModLoader.ModPlayer::PostSavePlayer()
//     System.Void Terraria.ModLoader.ModPlayer::CopyClientState(Terraria.ModLoader.ModPlayer)
//     System.Void Terraria.ModLoader.ModPlayer::SyncPlayer(System.Int32,System.Int32,System.Boolean)
//     System.Void Terraria.ModLoader.ModPlayer::SendClientChanges(Terraria.ModLoader.ModPlayer)
//     System.Void Terraria.ModLoader.ModPlayer::UpdateBadLifeRegen()
//     System.Void Terraria.ModLoader.ModPlayer::UpdateLifeRegen()
//     System.Void Terraria.ModLoader.ModPlayer::NaturalLifeRegen(System.Single&)
//     System.Void Terraria.ModLoader.ModPlayer::UpdateAutopause()
//     System.Void Terraria.ModLoader.ModPlayer::PreUpdate()
//     System.Void Terraria.ModLoader.ModPlayer::ProcessTriggers(Terraria.GameInput.TriggersSet)
//     System.Void Terraria.ModLoader.ModPlayer::ArmorSetBonusActivated()
//     System.Void Terraria.ModLoader.ModPlayer::ArmorSetBonusHeld(System.Int32)
//     System.Void Terraria.ModLoader.ModPlayer::SetControls()
//     System.Void Terraria.ModLoader.ModPlayer::PreUpdateBuffs()
//     System.Void Terraria.ModLoader.ModPlayer::PostUpdateBuffs()
//     System.Void Terraria.ModLoader.ModPlayer::UpdateEquips()
//     System.Void Terraria.ModLoader.ModPlayer::PostUpdateEquips()
//     System.Void Terraria.ModLoader.ModPlayer::UpdateVisibleAccessories()
//     System.Void Terraria.ModLoader.ModPlayer::UpdateVisibleVanityAccessories()
//     System.Void Terraria.ModLoader.ModPlayer::UpdateDyes()
//     System.Void Terraria.ModLoader.ModPlayer::PostUpdateMiscEffects()
//     System.Void Terraria.ModLoader.ModPlayer::PostUpdateRunSpeeds()
//     System.Void Terraria.ModLoader.ModPlayer::PreUpdateMovement()
//     System.Void Terraria.ModLoader.ModPlayer::PostUpdate()
//     System.Void Terraria.ModLoader.ModPlayer::ModifyExtraJumpDurationMultiplier(Terraria.ModLoader.ExtraJump,System.Single&)
//     System.Boolean Terraria.ModLoader.ModPlayer::CanStartExtraJump(Terraria.ModLoader.ExtraJump)
//     System.Void Terraria.ModLoader.ModPlayer::OnExtraJumpStarted(Terraria.ModLoader.ExtraJump,System.Boolean&)
//     System.Void Terraria.ModLoader.ModPlayer::OnExtraJumpEnded(Terraria.ModLoader.ExtraJump)
//     System.Void Terraria.ModLoader.ModPlayer::OnExtraJumpRefreshed(Terraria.ModLoader.ExtraJump)
//     System.Void Terraria.ModLoader.ModPlayer::ExtraJumpVisuals(Terraria.ModLoader.ExtraJump)
//     System.Boolean Terraria.ModLoader.ModPlayer::CanShowExtraJumpVisuals(Terraria.ModLoader.ExtraJump)
//     System.Void Terraria.ModLoader.ModPlayer::OnExtraJumpCleared(Terraria.ModLoader.ExtraJump)
//     System.Void Terraria.ModLoader.ModPlayer::FrameEffects()
//     System.Boolean Terraria.ModLoader.ModPlayer::ImmuneTo(Terraria.DataStructures.PlayerDeathReason,System.Int32,System.Boolean)
//     System.Boolean Terraria.ModLoader.ModPlayer::FreeDodge(Terraria.Player/HurtInfo)
//     System.Boolean Terraria.ModLoader.ModPlayer::ConsumableDodge(Terraria.Player/HurtInfo)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyHurt(Terraria.Player/HurtModifiers&)
//     System.Void Terraria.ModLoader.ModPlayer::OnHurt(Terraria.Player/HurtInfo)
//     System.Void Terraria.ModLoader.ModPlayer::PostHurt(Terraria.Player/HurtInfo)
//     System.Boolean Terraria.ModLoader.ModPlayer::PreKill(System.Double,System.Int32,System.Boolean,System.Boolean&,System.Boolean&,Terraria.DataStructures.PlayerDeathReason&)
//     System.Void Terraria.ModLoader.ModPlayer::Kill(System.Double,System.Int32,System.Boolean,Terraria.DataStructures.PlayerDeathReason)
//     System.Boolean Terraria.ModLoader.ModPlayer::PreModifyLuck(System.Single&)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyLuck(System.Single&)
//     System.Boolean Terraria.ModLoader.ModPlayer::PreItemCheck()
//     System.Void Terraria.ModLoader.ModPlayer::PostItemCheck()
//     System.Single Terraria.ModLoader.ModPlayer::UseTimeMultiplier(Terraria.Item)
//     System.Single Terraria.ModLoader.ModPlayer::UseAnimationMultiplier(Terraria.Item)
//     System.Single Terraria.ModLoader.ModPlayer::UseSpeedMultiplier(Terraria.Item)
//     System.Void Terraria.ModLoader.ModPlayer::GetHealLife(Terraria.Item,System.Boolean,System.Int32&)
//     System.Void Terraria.ModLoader.ModPlayer::GetHealMana(Terraria.Item,System.Boolean,System.Int32&)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyManaCost(Terraria.Item,System.Single&,System.Single&)
//     System.Void Terraria.ModLoader.ModPlayer::OnMissingMana(Terraria.Item,System.Int32)
//     System.Void Terraria.ModLoader.ModPlayer::OnConsumeMana(Terraria.Item,System.Int32)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyWeaponDamage(Terraria.Item,Terraria.ModLoader.StatModifier&)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyWeaponKnockback(Terraria.Item,Terraria.ModLoader.StatModifier&)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyWeaponCrit(Terraria.Item,System.Single&)
//     System.Boolean Terraria.ModLoader.ModPlayer::CanConsumeAmmo(Terraria.Item,Terraria.Item)
//     System.Void Terraria.ModLoader.ModPlayer::OnConsumeAmmo(Terraria.Item,Terraria.Item)
//     System.Boolean Terraria.ModLoader.ModPlayer::CanShoot(Terraria.Item)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyShootStats(Terraria.Item,Microsoft.Xna.Framework.Vector2&,Microsoft.Xna.Framework.Vector2&,System.Int32&,System.Int32&,System.Single&)
//     System.Boolean Terraria.ModLoader.ModPlayer::Shoot(Terraria.Item,Terraria.DataStructures.EntitySource_ItemUse_WithAmmo,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Vector2,System.Int32,System.Int32,System.Single)
//     System.Void Terraria.ModLoader.ModPlayer::MeleeEffects(Terraria.Item,Microsoft.Xna.Framework.Rectangle)
//     System.Void Terraria.ModLoader.ModPlayer::EmitEnchantmentVisualsAt(Terraria.Projectile,Microsoft.Xna.Framework.Vector2,System.Int32,System.Int32)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.ModPlayer::CanCatchNPC(Terraria.NPC,Terraria.Item)
//     System.Void Terraria.ModLoader.ModPlayer::OnCatchNPC(Terraria.NPC,Terraria.Item,System.Boolean)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyItemScale(Terraria.Item,System.Single&)
//     System.Void Terraria.ModLoader.ModPlayer::OnHitAnything(System.Single,System.Single,Terraria.Entity)
//     System.Boolean Terraria.ModLoader.ModPlayer::CanHitNPC(Terraria.NPC)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.ModPlayer::CanMeleeAttackCollideWithNPC(Terraria.Item,Microsoft.Xna.Framework.Rectangle,Terraria.NPC)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyHitNPC(Terraria.NPC,Terraria.NPC/HitModifiers&)
//     System.Void Terraria.ModLoader.ModPlayer::OnHitNPC(Terraria.NPC,Terraria.NPC/HitInfo,System.Int32)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.ModPlayer::CanHitNPCWithItem(Terraria.Item,Terraria.NPC)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyHitNPCWithItem(Terraria.Item,Terraria.NPC,Terraria.NPC/HitModifiers&)
//     System.Void Terraria.ModLoader.ModPlayer::OnHitNPCWithItem(Terraria.Item,Terraria.NPC,Terraria.NPC/HitInfo,System.Int32)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.ModPlayer::CanHitNPCWithProj(Terraria.Projectile,Terraria.NPC)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyHitNPCWithProj(Terraria.Projectile,Terraria.NPC,Terraria.NPC/HitModifiers&)
//     System.Void Terraria.ModLoader.ModPlayer::OnHitNPCWithProj(Terraria.Projectile,Terraria.NPC,Terraria.NPC/HitInfo,System.Int32)
//     System.Boolean Terraria.ModLoader.ModPlayer::CanHitPvp(Terraria.Item,Terraria.Player)
//     System.Boolean Terraria.ModLoader.ModPlayer::CanHitPvpWithProj(Terraria.Projectile,Terraria.Player)
//     System.Boolean Terraria.ModLoader.ModPlayer::CanBeHitByNPC(Terraria.NPC,System.Int32&)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyHitByNPC(Terraria.NPC,Terraria.Player/HurtModifiers&)
//     System.Void Terraria.ModLoader.ModPlayer::OnHitByNPC(Terraria.NPC,Terraria.Player/HurtInfo)
//     System.Boolean Terraria.ModLoader.ModPlayer::CanBeHitByProjectile(Terraria.Projectile)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyHitByProjectile(Terraria.Projectile,Terraria.Player/HurtModifiers&)
//     System.Void Terraria.ModLoader.ModPlayer::OnHitByProjectile(Terraria.Projectile,Terraria.Player/HurtInfo)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyFishingAttempt(Terraria.DataStructures.FishingAttempt&)
//     System.Void Terraria.ModLoader.ModPlayer::CatchFish(Terraria.DataStructures.FishingAttempt,System.Int32&,System.Int32&,Terraria.AdvancedPopupRequest&,Microsoft.Xna.Framework.Vector2&)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyCaughtFish(Terraria.Item)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.ModPlayer::CanConsumeBait(Terraria.Item)
//     System.Void Terraria.ModLoader.ModPlayer::GetFishingLevel(Terraria.Item,Terraria.Item,System.Single&)
//     System.Void Terraria.ModLoader.ModPlayer::AnglerQuestReward(System.Single,System.Collections.Generic.List`1<Terraria.Item>)
//     System.Void Terraria.ModLoader.ModPlayer::GetDyeTraderReward(System.Collections.Generic.List`1<System.Int32>)
//     System.Void Terraria.ModLoader.ModPlayer::DrawEffects(Terraria.DataStructures.PlayerDrawSet,System.Single&,System.Single&,System.Single&,System.Single&,System.Boolean&)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyDrawInfo(Terraria.DataStructures.PlayerDrawSet&)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyDrawLayerOrdering(System.Collections.Generic.IDictionary`2<Terraria.ModLoader.PlayerDrawLayer,Terraria.ModLoader.PlayerDrawLayer/Position>)
//     System.Void Terraria.ModLoader.ModPlayer::HideDrawLayers(Terraria.DataStructures.PlayerDrawSet)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyScreenPosition()
//     System.Void Terraria.ModLoader.ModPlayer::ModifyZoom(System.Single&)
//     System.Void Terraria.ModLoader.ModPlayer::PlayerConnect()
//     System.Void Terraria.ModLoader.ModPlayer::PlayerDisconnect()
//     System.Void Terraria.ModLoader.ModPlayer::OnEnterWorld()
//     System.Void Terraria.ModLoader.ModPlayer::OnRespawn()
//     System.Boolean Terraria.ModLoader.ModPlayer::ShiftClickSlot(Terraria.Item[],System.Int32,System.Int32)
//     System.Boolean Terraria.ModLoader.ModPlayer::HoverSlot(Terraria.Item[],System.Int32,System.Int32)
//     System.Void Terraria.ModLoader.ModPlayer::PostSellItem(Terraria.NPC,Terraria.Item[],Terraria.Item)
//     System.Boolean Terraria.ModLoader.ModPlayer::CanSellItem(Terraria.NPC,Terraria.Item[],Terraria.Item)
//     System.Void Terraria.ModLoader.ModPlayer::PostBuyItem(Terraria.NPC,Terraria.Item[],Terraria.Item)
//     System.Boolean Terraria.ModLoader.ModPlayer::CanBuyItem(Terraria.NPC,Terraria.Item[],Terraria.Item)
//     System.Boolean Terraria.ModLoader.ModPlayer::CanUseItem(Terraria.Item)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.ModPlayer::CanAutoReuseItem(Terraria.Item)
//     System.Boolean Terraria.ModLoader.ModPlayer::ModifyNurseHeal(Terraria.NPC,System.Int32&,System.Boolean&,System.String&)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyNursePrice(Terraria.NPC,System.Int32,System.Boolean,System.Int32&)
//     System.Void Terraria.ModLoader.ModPlayer::PostNurseHeal(Terraria.NPC,System.Int32,System.Boolean,System.Int32)
//     System.Collections.Generic.IEnumerable`1<Terraria.Item> Terraria.ModLoader.ModPlayer::AddStartingItems(System.Boolean)
//     System.Void Terraria.ModLoader.ModPlayer::ModifyStartingInventory(System.Collections.Generic.IReadOnlyDictionary`2<System.String,System.Collections.Generic.List`1<Terraria.Item>>,System.Boolean)
//     System.Collections.Generic.IEnumerable`1<Terraria.Item> Terraria.ModLoader.ModPlayer::AddMaterialsForCrafting(Terraria.ModLoader.ModPlayer/ItemConsumedCallback&)
//     System.Boolean Terraria.ModLoader.ModPlayer::OnPickup(Terraria.Item)
public static partial class ModPlayerHooks
{
    public static partial class NewInstance
    {
        public delegate Terraria.ModLoader.ModPlayer Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Player entity
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static Terraria.ModLoader.ModPlayer Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Player entity
        )
        {
            Event?.Invoke(self, entity);
        }
    }

    public static partial class Initialize
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class ResetEffects
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class ResetInfoAccessories
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class RefreshInfoAccessoriesFromTeamPlayers
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Player otherPlayer
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Player otherPlayer
        )
        {
            Event?.Invoke(self, otherPlayer);
        }
    }

    public static partial class ModifyMaxStats
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            out Terraria.ModLoader.StatModifier health,
            out Terraria.ModLoader.StatModifier mana
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            out Terraria.ModLoader.StatModifier health,
            out Terraria.ModLoader.StatModifier mana
        )
        {
            Event?.Invoke(self, out health, out mana);
        }
    }

    public static partial class UpdateDead
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class PreSaveCustomData
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class SaveData
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.IO.TagCompound tag
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.IO.TagCompound tag
        )
        {
            Event?.Invoke(self, tag);
        }
    }

    public static partial class LoadData
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.IO.TagCompound tag
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.IO.TagCompound tag
        )
        {
            Event?.Invoke(self, tag);
        }
    }

    public static partial class PreSavePlayer
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class PostSavePlayer
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class CopyClientState
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ModPlayer targetCopy
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ModPlayer targetCopy
        )
        {
            Event?.Invoke(self, targetCopy);
        }
    }

    public static partial class SyncPlayer
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            int toWho,
            int fromWho,
            bool newPlayer
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            int toWho,
            int fromWho,
            bool newPlayer
        )
        {
            Event?.Invoke(self, toWho, fromWho, newPlayer);
        }
    }

    public static partial class SendClientChanges
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ModPlayer clientPlayer
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ModPlayer clientPlayer
        )
        {
            Event?.Invoke(self, clientPlayer);
        }
    }

    public static partial class UpdateBadLifeRegen
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class UpdateLifeRegen
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class NaturalLifeRegen
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            ref float regen
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            ref float regen
        )
        {
            Event?.Invoke(self, ref regen);
        }
    }

    public static partial class UpdateAutopause
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class PreUpdate
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class ProcessTriggers
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.GameInput.TriggersSet triggersSet
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.GameInput.TriggersSet triggersSet
        )
        {
            Event?.Invoke(self, triggersSet);
        }
    }

    public static partial class ArmorSetBonusActivated
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class ArmorSetBonusHeld
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            int holdTime
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            int holdTime
        )
        {
            Event?.Invoke(self, holdTime);
        }
    }

    public static partial class SetControls
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class PreUpdateBuffs
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class PostUpdateBuffs
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class UpdateEquips
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class PostUpdateEquips
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class UpdateVisibleAccessories
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class UpdateVisibleVanityAccessories
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class UpdateDyes
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class PostUpdateMiscEffects
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class PostUpdateRunSpeeds
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class PreUpdateMovement
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class PostUpdate
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class ModifyExtraJumpDurationMultiplier
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump,
            ref float duration
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump,
            ref float duration
        )
        {
            Event?.Invoke(self, jump, ref duration);
        }
    }

    public static partial class CanStartExtraJump
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump
        )
        {
            Event?.Invoke(self, jump);
        }
    }

    public static partial class OnExtraJumpStarted
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump,
            ref bool playSound
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump,
            ref bool playSound
        )
        {
            Event?.Invoke(self, jump, ref playSound);
        }
    }

    public static partial class OnExtraJumpEnded
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump
        )
        {
            Event?.Invoke(self, jump);
        }
    }

    public static partial class OnExtraJumpRefreshed
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump
        )
        {
            Event?.Invoke(self, jump);
        }
    }

    public static partial class ExtraJumpVisuals
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump
        )
        {
            Event?.Invoke(self, jump);
        }
    }

    public static partial class CanShowExtraJumpVisuals
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump
        )
        {
            Event?.Invoke(self, jump);
        }
    }

    public static partial class OnExtraJumpCleared
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.ModLoader.ExtraJump jump
        )
        {
            Event?.Invoke(self, jump);
        }
    }

    public static partial class FrameEffects
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class ImmuneTo
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.DataStructures.PlayerDeathReason damageSource,
            int cooldownCounter,
            bool dodgeable
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.DataStructures.PlayerDeathReason damageSource,
            int cooldownCounter,
            bool dodgeable
        )
        {
            Event?.Invoke(self, damageSource, cooldownCounter, dodgeable);
        }
    }

    public static partial class FreeDodge
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Player.HurtInfo info
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Player.HurtInfo info
        )
        {
            Event?.Invoke(self, info);
        }
    }

    public static partial class ConsumableDodge
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Player.HurtInfo info
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Player.HurtInfo info
        )
        {
            Event?.Invoke(self, info);
        }
    }

    public static partial class ModifyHurt
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            ref Terraria.Player.HurtModifiers modifiers
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            ref Terraria.Player.HurtModifiers modifiers
        )
        {
            Event?.Invoke(self, ref modifiers);
        }
    }

    public static partial class OnHurt
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Player.HurtInfo info
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Player.HurtInfo info
        )
        {
            Event?.Invoke(self, info);
        }
    }

    public static partial class PostHurt
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Player.HurtInfo info
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Player.HurtInfo info
        )
        {
            Event?.Invoke(self, info);
        }
    }

    public static partial class PreKill
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            double damage,
            int hitDirection,
            bool pvp,
            ref bool playSound,
            ref bool genDust,
            ref Terraria.DataStructures.PlayerDeathReason damageSource
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            double damage,
            int hitDirection,
            bool pvp,
            ref bool playSound,
            ref bool genDust,
            ref Terraria.DataStructures.PlayerDeathReason damageSource
        )
        {
            Event?.Invoke(self, damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
        }
    }

    public static partial class Kill
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            double damage,
            int hitDirection,
            bool pvp,
            Terraria.DataStructures.PlayerDeathReason damageSource
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            double damage,
            int hitDirection,
            bool pvp,
            Terraria.DataStructures.PlayerDeathReason damageSource
        )
        {
            Event?.Invoke(self, damage, hitDirection, pvp, damageSource);
        }
    }

    public static partial class PreModifyLuck
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            ref float luck
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            ref float luck
        )
        {
            Event?.Invoke(self, ref luck);
        }
    }

    public static partial class ModifyLuck
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            ref float luck
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            ref float luck
        )
        {
            Event?.Invoke(self, ref luck);
        }
    }

    public static partial class PreItemCheck
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class PostItemCheck
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class UseTimeMultiplier
    {
        public delegate float Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static float Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item
        )
        {
            Event?.Invoke(self, item);
        }
    }

    public static partial class UseAnimationMultiplier
    {
        public delegate float Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static float Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item
        )
        {
            Event?.Invoke(self, item);
        }
    }

    public static partial class UseSpeedMultiplier
    {
        public delegate float Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static float Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item
        )
        {
            Event?.Invoke(self, item);
        }
    }

    public static partial class GetHealLife
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            bool quickHeal,
            ref int healValue
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            bool quickHeal,
            ref int healValue
        )
        {
            Event?.Invoke(self, item, quickHeal, ref healValue);
        }
    }

    public static partial class GetHealMana
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            bool quickHeal,
            ref int healValue
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            bool quickHeal,
            ref int healValue
        )
        {
            Event?.Invoke(self, item, quickHeal, ref healValue);
        }
    }

    public static partial class ModifyManaCost
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            ref float reduce,
            ref float mult
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            ref float reduce,
            ref float mult
        )
        {
            Event?.Invoke(self, item, ref reduce, ref mult);
        }
    }

    public static partial class OnMissingMana
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            int neededMana
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            int neededMana
        )
        {
            Event?.Invoke(self, item, neededMana);
        }
    }

    public static partial class OnConsumeMana
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            int manaConsumed
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            int manaConsumed
        )
        {
            Event?.Invoke(self, item, manaConsumed);
        }
    }

    public static partial class ModifyWeaponDamage
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            ref Terraria.ModLoader.StatModifier damage
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            ref Terraria.ModLoader.StatModifier damage
        )
        {
            Event?.Invoke(self, item, ref damage);
        }
    }

    public static partial class ModifyWeaponKnockback
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            ref Terraria.ModLoader.StatModifier knockback
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            ref Terraria.ModLoader.StatModifier knockback
        )
        {
            Event?.Invoke(self, item, ref knockback);
        }
    }

    public static partial class ModifyWeaponCrit
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            ref float crit
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            ref float crit
        )
        {
            Event?.Invoke(self, item, ref crit);
        }
    }

    public static partial class CanConsumeAmmo
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item weapon,
            Terraria.Item ammo
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item weapon,
            Terraria.Item ammo
        )
        {
            Event?.Invoke(self, weapon, ammo);
        }
    }

    public static partial class OnConsumeAmmo
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item weapon,
            Terraria.Item ammo
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item weapon,
            Terraria.Item ammo
        )
        {
            Event?.Invoke(self, weapon, ammo);
        }
    }

    public static partial class CanShoot
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item
        )
        {
            Event?.Invoke(self, item);
        }
    }

    public static partial class ModifyShootStats
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            ref Microsoft.Xna.Framework.Vector2 position,
            ref Microsoft.Xna.Framework.Vector2 velocity,
            ref int type,
            ref int damage,
            ref float knockback
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            ref Microsoft.Xna.Framework.Vector2 position,
            ref Microsoft.Xna.Framework.Vector2 velocity,
            ref int type,
            ref int damage,
            ref float knockback
        )
        {
            Event?.Invoke(self, item, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
    }

    public static partial class Shoot
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Vector2 velocity,
            int type,
            int damage,
            float knockback
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Microsoft.Xna.Framework.Vector2 position,
            Microsoft.Xna.Framework.Vector2 velocity,
            int type,
            int damage,
            float knockback
        )
        {
            Event?.Invoke(self, item, source, position, velocity, type, damage, knockback);
        }
    }

    public static partial class MeleeEffects
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            Microsoft.Xna.Framework.Rectangle hitbox
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            Microsoft.Xna.Framework.Rectangle hitbox
        )
        {
            Event?.Invoke(self, item, hitbox);
        }
    }

    public static partial class EmitEnchantmentVisualsAt
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile projectile,
            Microsoft.Xna.Framework.Vector2 boxPosition,
            int boxWidth,
            int boxHeight
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile projectile,
            Microsoft.Xna.Framework.Vector2 boxPosition,
            int boxWidth,
            int boxHeight
        )
        {
            Event?.Invoke(self, projectile, boxPosition, boxWidth, boxHeight);
        }
    }

    public static partial class CanCatchNPC
    {
        public delegate bool? Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC target,
            Terraria.Item item
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool? Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC target,
            Terraria.Item item
        )
        {
            Event?.Invoke(self, target, item);
        }
    }

    public static partial class OnCatchNPC
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC npc,
            Terraria.Item item,
            bool failed
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC npc,
            Terraria.Item item,
            bool failed
        )
        {
            Event?.Invoke(self, npc, item, failed);
        }
    }

    public static partial class ModifyItemScale
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            ref float scale
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            ref float scale
        )
        {
            Event?.Invoke(self, item, ref scale);
        }
    }

    public static partial class OnHitAnything
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            float x,
            float y,
            Terraria.Entity victim
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            float x,
            float y,
            Terraria.Entity victim
        )
        {
            Event?.Invoke(self, x, y, victim);
        }
    }

    public static partial class CanHitNPC
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC target
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC target
        )
        {
            Event?.Invoke(self, target);
        }
    }

    public static partial class CanMeleeAttackCollideWithNPC
    {
        public delegate bool? Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            Microsoft.Xna.Framework.Rectangle meleeAttackHitbox,
            Terraria.NPC target
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool? Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            Microsoft.Xna.Framework.Rectangle meleeAttackHitbox,
            Terraria.NPC target
        )
        {
            Event?.Invoke(self, item, meleeAttackHitbox, target);
        }
    }

    public static partial class ModifyHitNPC
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC target,
            ref Terraria.NPC.HitModifiers modifiers
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC target,
            ref Terraria.NPC.HitModifiers modifiers
        )
        {
            Event?.Invoke(self, target, ref modifiers);
        }
    }

    public static partial class OnHitNPC
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC target,
            Terraria.NPC.HitInfo hit,
            int damageDone
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC target,
            Terraria.NPC.HitInfo hit,
            int damageDone
        )
        {
            Event?.Invoke(self, target, hit, damageDone);
        }
    }

    public static partial class CanHitNPCWithItem
    {
        public delegate bool? Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            Terraria.NPC target
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool? Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            Terraria.NPC target
        )
        {
            Event?.Invoke(self, item, target);
        }
    }

    public static partial class ModifyHitNPCWithItem
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            Terraria.NPC target,
            ref Terraria.NPC.HitModifiers modifiers
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            Terraria.NPC target,
            ref Terraria.NPC.HitModifiers modifiers
        )
        {
            Event?.Invoke(self, item, target, ref modifiers);
        }
    }

    public static partial class OnHitNPCWithItem
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            Terraria.NPC target,
            Terraria.NPC.HitInfo hit,
            int damageDone
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            Terraria.NPC target,
            Terraria.NPC.HitInfo hit,
            int damageDone
        )
        {
            Event?.Invoke(self, item, target, hit, damageDone);
        }
    }

    public static partial class CanHitNPCWithProj
    {
        public delegate bool? Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile proj,
            Terraria.NPC target
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool? Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile proj,
            Terraria.NPC target
        )
        {
            Event?.Invoke(self, proj, target);
        }
    }

    public static partial class ModifyHitNPCWithProj
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile proj,
            Terraria.NPC target,
            ref Terraria.NPC.HitModifiers modifiers
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile proj,
            Terraria.NPC target,
            ref Terraria.NPC.HitModifiers modifiers
        )
        {
            Event?.Invoke(self, proj, target, ref modifiers);
        }
    }

    public static partial class OnHitNPCWithProj
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile proj,
            Terraria.NPC target,
            Terraria.NPC.HitInfo hit,
            int damageDone
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile proj,
            Terraria.NPC target,
            Terraria.NPC.HitInfo hit,
            int damageDone
        )
        {
            Event?.Invoke(self, proj, target, hit, damageDone);
        }
    }

    public static partial class CanHitPvp
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            Terraria.Player target
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item,
            Terraria.Player target
        )
        {
            Event?.Invoke(self, item, target);
        }
    }

    public static partial class CanHitPvpWithProj
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile proj,
            Terraria.Player target
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile proj,
            Terraria.Player target
        )
        {
            Event?.Invoke(self, proj, target);
        }
    }

    public static partial class CanBeHitByNPC
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC npc,
            ref int cooldownSlot
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC npc,
            ref int cooldownSlot
        )
        {
            Event?.Invoke(self, npc, ref cooldownSlot);
        }
    }

    public static partial class ModifyHitByNPC
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC npc,
            ref Terraria.Player.HurtModifiers modifiers
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC npc,
            ref Terraria.Player.HurtModifiers modifiers
        )
        {
            Event?.Invoke(self, npc, ref modifiers);
        }
    }

    public static partial class OnHitByNPC
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC npc,
            Terraria.Player.HurtInfo hurtInfo
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC npc,
            Terraria.Player.HurtInfo hurtInfo
        )
        {
            Event?.Invoke(self, npc, hurtInfo);
        }
    }

    public static partial class CanBeHitByProjectile
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile proj
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile proj
        )
        {
            Event?.Invoke(self, proj);
        }
    }

    public static partial class ModifyHitByProjectile
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile proj,
            ref Terraria.Player.HurtModifiers modifiers
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile proj,
            ref Terraria.Player.HurtModifiers modifiers
        )
        {
            Event?.Invoke(self, proj, ref modifiers);
        }
    }

    public static partial class OnHitByProjectile
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile proj,
            Terraria.Player.HurtInfo hurtInfo
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Projectile proj,
            Terraria.Player.HurtInfo hurtInfo
        )
        {
            Event?.Invoke(self, proj, hurtInfo);
        }
    }

    public static partial class ModifyFishingAttempt
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            ref Terraria.DataStructures.FishingAttempt attempt
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            ref Terraria.DataStructures.FishingAttempt attempt
        )
        {
            Event?.Invoke(self, ref attempt);
        }
    }

    public static partial class CatchFish
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.DataStructures.FishingAttempt attempt,
            ref int itemDrop,
            ref int npcSpawn,
            ref Terraria.AdvancedPopupRequest sonar,
            ref Microsoft.Xna.Framework.Vector2 sonarPosition
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.DataStructures.FishingAttempt attempt,
            ref int itemDrop,
            ref int npcSpawn,
            ref Terraria.AdvancedPopupRequest sonar,
            ref Microsoft.Xna.Framework.Vector2 sonarPosition
        )
        {
            Event?.Invoke(self, attempt, ref itemDrop, ref npcSpawn, ref sonar, ref sonarPosition);
        }
    }

    public static partial class ModifyCaughtFish
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item fish
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item fish
        )
        {
            Event?.Invoke(self, fish);
        }
    }

    public static partial class CanConsumeBait
    {
        public delegate bool? Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item bait
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool? Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item bait
        )
        {
            Event?.Invoke(self, bait);
        }
    }

    public static partial class GetFishingLevel
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item fishingRod,
            Terraria.Item bait,
            ref float fishingLevel
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item fishingRod,
            Terraria.Item bait,
            ref float fishingLevel
        )
        {
            Event?.Invoke(self, fishingRod, bait, ref fishingLevel);
        }
    }

    public static partial class AnglerQuestReward
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            float rareMultiplier,
            System.Collections.Generic.List<Terraria.Item> rewardItems
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            float rareMultiplier,
            System.Collections.Generic.List<Terraria.Item> rewardItems
        )
        {
            Event?.Invoke(self, rareMultiplier, rewardItems);
        }
    }

    public static partial class GetDyeTraderReward
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            System.Collections.Generic.List<int> rewardPool
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            System.Collections.Generic.List<int> rewardPool
        )
        {
            Event?.Invoke(self, rewardPool);
        }
    }

    public static partial class DrawEffects
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.DataStructures.PlayerDrawSet drawInfo,
            ref float r,
            ref float g,
            ref float b,
            ref float a,
            ref bool fullBright
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.DataStructures.PlayerDrawSet drawInfo,
            ref float r,
            ref float g,
            ref float b,
            ref float a,
            ref bool fullBright
        )
        {
            Event?.Invoke(self, drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
        }
    }

    public static partial class ModifyDrawInfo
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            ref Terraria.DataStructures.PlayerDrawSet drawInfo
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            ref Terraria.DataStructures.PlayerDrawSet drawInfo
        )
        {
            Event?.Invoke(self, ref drawInfo);
        }
    }

    public static partial class ModifyDrawLayerOrdering
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            System.Collections.Generic.IDictionary<Terraria.ModLoader.PlayerDrawLayer, Terraria.ModLoader.PlayerDrawLayer.Position> positions
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            System.Collections.Generic.IDictionary<Terraria.ModLoader.PlayerDrawLayer, Terraria.ModLoader.PlayerDrawLayer.Position> positions
        )
        {
            Event?.Invoke(self, positions);
        }
    }

    public static partial class HideDrawLayers
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.DataStructures.PlayerDrawSet drawInfo
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.DataStructures.PlayerDrawSet drawInfo
        )
        {
            Event?.Invoke(self, drawInfo);
        }
    }

    public static partial class ModifyScreenPosition
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class ModifyZoom
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            ref float zoom
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            ref float zoom
        )
        {
            Event?.Invoke(self, ref zoom);
        }
    }

    public static partial class PlayerConnect
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class PlayerDisconnect
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class OnEnterWorld
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class OnRespawn
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self
        )
        {
            Event?.Invoke(self);
        }
    }

    public static partial class ShiftClickSlot
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item[] inventory,
            int context,
            int slot
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item[] inventory,
            int context,
            int slot
        )
        {
            Event?.Invoke(self, inventory, context, slot);
        }
    }

    public static partial class HoverSlot
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item[] inventory,
            int context,
            int slot
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item[] inventory,
            int context,
            int slot
        )
        {
            Event?.Invoke(self, inventory, context, slot);
        }
    }

    public static partial class PostSellItem
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC vendor,
            Terraria.Item[] shopInventory,
            Terraria.Item item
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC vendor,
            Terraria.Item[] shopInventory,
            Terraria.Item item
        )
        {
            Event?.Invoke(self, vendor, shopInventory, item);
        }
    }

    public static partial class CanSellItem
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC vendor,
            Terraria.Item[] shopInventory,
            Terraria.Item item
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC vendor,
            Terraria.Item[] shopInventory,
            Terraria.Item item
        )
        {
            Event?.Invoke(self, vendor, shopInventory, item);
        }
    }

    public static partial class PostBuyItem
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC vendor,
            Terraria.Item[] shopInventory,
            Terraria.Item item
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC vendor,
            Terraria.Item[] shopInventory,
            Terraria.Item item
        )
        {
            Event?.Invoke(self, vendor, shopInventory, item);
        }
    }

    public static partial class CanBuyItem
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC vendor,
            Terraria.Item[] shopInventory,
            Terraria.Item item
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC vendor,
            Terraria.Item[] shopInventory,
            Terraria.Item item
        )
        {
            Event?.Invoke(self, vendor, shopInventory, item);
        }
    }

    public static partial class CanUseItem
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item
        )
        {
            Event?.Invoke(self, item);
        }
    }

    public static partial class CanAutoReuseItem
    {
        public delegate bool? Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool? Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item
        )
        {
            Event?.Invoke(self, item);
        }
    }

    public static partial class ModifyNurseHeal
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC nurse,
            ref int health,
            ref bool removeDebuffs,
            ref string chatText
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC nurse,
            ref int health,
            ref bool removeDebuffs,
            ref string chatText
        )
        {
            Event?.Invoke(self, nurse, ref health, ref removeDebuffs, ref chatText);
        }
    }

    public static partial class ModifyNursePrice
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC nurse,
            int health,
            bool removeDebuffs,
            ref int price
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC nurse,
            int health,
            bool removeDebuffs,
            ref int price
        )
        {
            Event?.Invoke(self, nurse, health, removeDebuffs, ref price);
        }
    }

    public static partial class PostNurseHeal
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC nurse,
            int health,
            bool removeDebuffs,
            int price
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.NPC nurse,
            int health,
            bool removeDebuffs,
            int price
        )
        {
            Event?.Invoke(self, nurse, health, removeDebuffs, price);
        }
    }

    public static partial class AddStartingItems
    {
        public delegate System.Collections.Generic.IEnumerable<Terraria.Item> Definition(
            Terraria.ModLoader.ModPlayer self,
            bool mediumCoreDeath
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static System.Collections.Generic.IEnumerable<Terraria.Item> Invoke(
            Terraria.ModLoader.ModPlayer self,
            bool mediumCoreDeath
        )
        {
            Event?.Invoke(self, mediumCoreDeath);
        }
    }

    public static partial class ModifyStartingInventory
    {
        public delegate void Definition(
            Terraria.ModLoader.ModPlayer self,
            System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.List<Terraria.Item>> itemsByMod,
            bool mediumCoreDeath
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.ModPlayer self,
            System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.List<Terraria.Item>> itemsByMod,
            bool mediumCoreDeath
        )
        {
            Event?.Invoke(self, itemsByMod, mediumCoreDeath);
        }
    }

    public static partial class AddMaterialsForCrafting
    {
        public delegate System.Collections.Generic.IEnumerable<Terraria.Item> Definition(
            Terraria.ModLoader.ModPlayer self,
            out Terraria.ModLoader.ModPlayer.ItemConsumedCallback itemConsumedCallback
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static System.Collections.Generic.IEnumerable<Terraria.Item> Invoke(
            Terraria.ModLoader.ModPlayer self,
            out Terraria.ModLoader.ModPlayer.ItemConsumedCallback itemConsumedCallback
        )
        {
            Event?.Invoke(self, out itemConsumedCallback);
        }
    }

    public static partial class OnPickup
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.ModPlayer self,
            Terraria.Item item
        )
        {
            Event?.Invoke(self, item);
        }
    }
}
