namespace Daybreak.Common.Features.Hooks;

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedType.Global
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// Hooks to generate for 'Terraria.ModLoader.GlobalNPC':
//     System.Void Terraria.ModLoader.GlobalNPC::SetDefaultsFromNetId(Terraria.NPC)
//     System.Void Terraria.ModLoader.GlobalNPC::OnSpawn(Terraria.NPC,Terraria.DataStructures.IEntitySource)
//     System.Void Terraria.ModLoader.GlobalNPC::ApplyDifficultyAndPlayerScaling(Terraria.NPC,System.Int32,System.Single,System.Single)
//     System.Void Terraria.ModLoader.GlobalNPC::SetBestiary(Terraria.NPC,Terraria.GameContent.Bestiary.BestiaryDatabase,Terraria.GameContent.Bestiary.BestiaryEntry)
//     System.Void Terraria.ModLoader.GlobalNPC::ModifyTypeName(Terraria.NPC,System.String&)
//     System.Void Terraria.ModLoader.GlobalNPC::ModifyHoverBoundingBox(Terraria.NPC,Microsoft.Xna.Framework.Rectangle&)
//     Terraria.GameContent.ITownNPCProfile Terraria.ModLoader.GlobalNPC::ModifyTownNPCProfile(Terraria.NPC)
//     System.Void Terraria.ModLoader.GlobalNPC::ModifyNPCNameList(Terraria.NPC,System.Collections.Generic.List`1<System.String>)
//     System.Void Terraria.ModLoader.GlobalNPC::ResetEffects(Terraria.NPC)
//     System.Boolean Terraria.ModLoader.GlobalNPC::PreAI(Terraria.NPC)
//     System.Void Terraria.ModLoader.GlobalNPC::AI(Terraria.NPC)
//     System.Void Terraria.ModLoader.GlobalNPC::PostAI(Terraria.NPC)
//     System.Void Terraria.ModLoader.GlobalNPC::SendExtraAI(Terraria.NPC,Terraria.ModLoader.IO.BitWriter,System.IO.BinaryWriter)
//     System.Void Terraria.ModLoader.GlobalNPC::ReceiveExtraAI(Terraria.NPC,Terraria.ModLoader.IO.BitReader,System.IO.BinaryReader)
//     System.Void Terraria.ModLoader.GlobalNPC::FindFrame(Terraria.NPC,System.Int32)
//     System.Void Terraria.ModLoader.GlobalNPC::HitEffect(Terraria.NPC,Terraria.NPC/HitInfo)
//     System.Void Terraria.ModLoader.GlobalNPC::UpdateLifeRegen(Terraria.NPC,System.Int32&)
//     System.Boolean Terraria.ModLoader.GlobalNPC::CheckActive(Terraria.NPC)
//     System.Boolean Terraria.ModLoader.GlobalNPC::CheckDead(Terraria.NPC)
//     System.Boolean Terraria.ModLoader.GlobalNPC::SpecialOnKill(Terraria.NPC)
//     System.Boolean Terraria.ModLoader.GlobalNPC::PreKill(Terraria.NPC)
//     System.Void Terraria.ModLoader.GlobalNPC::OnKill(Terraria.NPC)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalNPC::CanFallThroughPlatforms(Terraria.NPC)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalNPC::CanBeCaughtBy(Terraria.NPC,Terraria.Item,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalNPC::OnCaughtBy(Terraria.NPC,Terraria.Player,Terraria.Item,System.Boolean)
//     System.Void Terraria.ModLoader.GlobalNPC::ModifyNPCLoot(Terraria.NPC,Terraria.ModLoader.NPCLoot)
//     System.Void Terraria.ModLoader.GlobalNPC::ModifyGlobalLoot(Terraria.ModLoader.GlobalLoot)
//     System.Boolean Terraria.ModLoader.GlobalNPC::CanHitPlayer(Terraria.NPC,Terraria.Player,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalNPC::ModifyHitPlayer(Terraria.NPC,Terraria.Player,Terraria.Player/HurtModifiers&)
//     System.Void Terraria.ModLoader.GlobalNPC::OnHitPlayer(Terraria.NPC,Terraria.Player,Terraria.Player/HurtInfo)
//     System.Boolean Terraria.ModLoader.GlobalNPC::CanHitNPC(Terraria.NPC,Terraria.NPC)
//     System.Boolean Terraria.ModLoader.GlobalNPC::CanBeHitByNPC(Terraria.NPC,Terraria.NPC)
//     System.Void Terraria.ModLoader.GlobalNPC::ModifyHitNPC(Terraria.NPC,Terraria.NPC,Terraria.NPC/HitModifiers&)
//     System.Void Terraria.ModLoader.GlobalNPC::OnHitNPC(Terraria.NPC,Terraria.NPC,Terraria.NPC/HitInfo)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalNPC::CanBeHitByItem(Terraria.NPC,Terraria.Player,Terraria.Item)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalNPC::CanCollideWithPlayerMeleeAttack(Terraria.NPC,Terraria.Player,Terraria.Item,Microsoft.Xna.Framework.Rectangle)
//     System.Void Terraria.ModLoader.GlobalNPC::ModifyHitByItem(Terraria.NPC,Terraria.Player,Terraria.Item,Terraria.NPC/HitModifiers&)
//     System.Void Terraria.ModLoader.GlobalNPC::OnHitByItem(Terraria.NPC,Terraria.Player,Terraria.Item,Terraria.NPC/HitInfo,System.Int32)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalNPC::CanBeHitByProjectile(Terraria.NPC,Terraria.Projectile)
//     System.Void Terraria.ModLoader.GlobalNPC::ModifyHitByProjectile(Terraria.NPC,Terraria.Projectile,Terraria.NPC/HitModifiers&)
//     System.Void Terraria.ModLoader.GlobalNPC::OnHitByProjectile(Terraria.NPC,Terraria.Projectile,Terraria.NPC/HitInfo,System.Int32)
//     System.Void Terraria.ModLoader.GlobalNPC::ModifyIncomingHit(Terraria.NPC,Terraria.NPC/HitModifiers&)
//     System.Void Terraria.ModLoader.GlobalNPC::BossHeadSlot(Terraria.NPC,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalNPC::BossHeadRotation(Terraria.NPC,System.Single&)
//     System.Void Terraria.ModLoader.GlobalNPC::BossHeadSpriteEffects(Terraria.NPC,Microsoft.Xna.Framework.Graphics.SpriteEffects&)
//     System.Nullable`1<Microsoft.Xna.Framework.Color> Terraria.ModLoader.GlobalNPC::GetAlpha(Terraria.NPC,Microsoft.Xna.Framework.Color)
//     System.Void Terraria.ModLoader.GlobalNPC::DrawEffects(Terraria.NPC,Microsoft.Xna.Framework.Color&)
//     System.Boolean Terraria.ModLoader.GlobalNPC::PreDraw(Terraria.NPC,Microsoft.Xna.Framework.Graphics.SpriteBatch,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Color)
//     System.Void Terraria.ModLoader.GlobalNPC::PostDraw(Terraria.NPC,Microsoft.Xna.Framework.Graphics.SpriteBatch,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Color)
//     System.Void Terraria.ModLoader.GlobalNPC::DrawBehind(Terraria.NPC,System.Int32)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalNPC::DrawHealthBar(Terraria.NPC,System.Byte,System.Single&,Microsoft.Xna.Framework.Vector2&)
//     System.Void Terraria.ModLoader.GlobalNPC::EditSpawnRate(Terraria.Player,System.Int32&,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalNPC::EditSpawnRange(Terraria.Player,System.Int32&,System.Int32&,System.Int32&,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalNPC::EditSpawnPool(System.Collections.Generic.IDictionary`2<System.Int32,System.Single>,Terraria.ModLoader.NPCSpawnInfo)
//     System.Void Terraria.ModLoader.GlobalNPC::SpawnNPC(System.Int32,System.Int32,System.Int32)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalNPC::CanChat(Terraria.NPC)
//     System.Void Terraria.ModLoader.GlobalNPC::GetChat(Terraria.NPC,System.String&)
//     System.Boolean Terraria.ModLoader.GlobalNPC::PreChatButtonClicked(Terraria.NPC,System.Boolean)
//     System.Void Terraria.ModLoader.GlobalNPC::OnChatButtonClicked(Terraria.NPC,System.Boolean)
//     System.Void Terraria.ModLoader.GlobalNPC::ModifyShop(Terraria.ModLoader.NPCShop)
//     System.Void Terraria.ModLoader.GlobalNPC::ModifyActiveShop(Terraria.NPC,System.String,Terraria.Item[])
//     System.Void Terraria.ModLoader.GlobalNPC::SetupTravelShop(System.Int32[],System.Int32&)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalNPC::CanGoToStatue(Terraria.NPC,System.Boolean)
//     System.Void Terraria.ModLoader.GlobalNPC::OnGoToStatue(Terraria.NPC,System.Boolean)
//     System.Void Terraria.ModLoader.GlobalNPC::BuffTownNPC(System.Single&,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalNPC::TownNPCAttackStrength(Terraria.NPC,System.Int32&,System.Single&)
//     System.Void Terraria.ModLoader.GlobalNPC::TownNPCAttackCooldown(Terraria.NPC,System.Int32&,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalNPC::TownNPCAttackProj(Terraria.NPC,System.Int32&,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalNPC::TownNPCAttackProjSpeed(Terraria.NPC,System.Single&,System.Single&,System.Single&)
//     System.Void Terraria.ModLoader.GlobalNPC::TownNPCAttackShoot(Terraria.NPC,System.Boolean&)
//     System.Void Terraria.ModLoader.GlobalNPC::TownNPCAttackMagic(Terraria.NPC,System.Single&)
//     System.Void Terraria.ModLoader.GlobalNPC::TownNPCAttackSwing(Terraria.NPC,System.Int32&,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalNPC::DrawTownAttackGun(Terraria.NPC,Microsoft.Xna.Framework.Graphics.Texture2D&,Microsoft.Xna.Framework.Rectangle&,System.Single&,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalNPC::DrawTownAttackSwing(Terraria.NPC,Microsoft.Xna.Framework.Graphics.Texture2D&,Microsoft.Xna.Framework.Rectangle&,System.Int32&,System.Single&,Microsoft.Xna.Framework.Vector2&)
//     System.Boolean Terraria.ModLoader.GlobalNPC::ModifyCollisionData(Terraria.NPC,Microsoft.Xna.Framework.Rectangle,System.Int32&,Terraria.ModLoader.MultipliableFloat&,Microsoft.Xna.Framework.Rectangle&)
//     System.Boolean Terraria.ModLoader.GlobalNPC::NeedSaving(Terraria.NPC)
//     System.Void Terraria.ModLoader.GlobalNPC::SaveData(Terraria.NPC,Terraria.ModLoader.IO.TagCompound)
//     System.Void Terraria.ModLoader.GlobalNPC::LoadData(Terraria.NPC,Terraria.ModLoader.IO.TagCompound)
//     System.Nullable`1<System.Int32> Terraria.ModLoader.GlobalNPC::PickEmote(Terraria.NPC,Terraria.Player,System.Collections.Generic.List`1<System.Int32>,Terraria.GameContent.UI.WorldUIAnchor)
//     System.Void Terraria.ModLoader.GlobalNPC::ChatBubblePosition(Terraria.NPC,Microsoft.Xna.Framework.Vector2&,Microsoft.Xna.Framework.Graphics.SpriteEffects&)
//     System.Void Terraria.ModLoader.GlobalNPC::PartyHatPosition(Terraria.NPC,Microsoft.Xna.Framework.Vector2&,Microsoft.Xna.Framework.Graphics.SpriteEffects&)
//     System.Void Terraria.ModLoader.GlobalNPC::EmoteBubblePosition(Terraria.NPC,Microsoft.Xna.Framework.Vector2&,Microsoft.Xna.Framework.Graphics.SpriteEffects&)
public static partial class GlobalNPCHooks
{
    public static partial class SetDefaultsFromNetId
    {
        public delegate void Definition(
            Terraria.NPC npc
        );

        public static event Definition Event;
    }

    public static partial class OnSpawn
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.DataStructures.IEntitySource source
        );

        public static event Definition Event;
    }

    public static partial class ApplyDifficultyAndPlayerScaling
    {
        public delegate void Definition(
            Terraria.NPC npc,
            int numPlayers,
            float balance,
            float bossAdjustment
        );

        public static event Definition Event;
    }

    public static partial class SetBestiary
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.GameContent.Bestiary.BestiaryDatabase database,
            Terraria.GameContent.Bestiary.BestiaryEntry bestiaryEntry
        );

        public static event Definition Event;
    }

    public static partial class ModifyTypeName
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref string typeName
        );

        public static event Definition Event;
    }

    public static partial class ModifyHoverBoundingBox
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref Microsoft.Xna.Framework.Rectangle boundingBox
        );

        public static event Definition Event;
    }

    public static partial class ModifyTownNPCProfile
    {
        public delegate Terraria.GameContent.ITownNPCProfile Definition(
            Terraria.NPC npc
        );

        public static event Definition Event;
    }

    public static partial class ModifyNPCNameList
    {
        public delegate void Definition(
            Terraria.NPC npc,
            System.Collections.Generic.List<string> nameList
        );

        public static event Definition Event;
    }

    public static partial class ResetEffects
    {
        public delegate void Definition(
            Terraria.NPC npc
        );

        public static event Definition Event;
    }

    public static partial class PreAI
    {
        public delegate bool Definition(
            Terraria.NPC npc
        );

        public static event Definition Event;
    }

    public static partial class AI
    {
        public delegate void Definition(
            Terraria.NPC npc
        );

        public static event Definition Event;
    }

    public static partial class PostAI
    {
        public delegate void Definition(
            Terraria.NPC npc
        );

        public static event Definition Event;
    }

    public static partial class SendExtraAI
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.ModLoader.IO.BitWriter bitWriter,
            System.IO.BinaryWriter binaryWriter
        );

        public static event Definition Event;
    }

    public static partial class ReceiveExtraAI
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.ModLoader.IO.BitReader bitReader,
            System.IO.BinaryReader binaryReader
        );

        public static event Definition Event;
    }

    public static partial class FindFrame
    {
        public delegate void Definition(
            Terraria.NPC npc,
            int frameHeight
        );

        public static event Definition Event;
    }

    public static partial class HitEffect
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.NPC.HitInfo hit
        );

        public static event Definition Event;
    }

    public static partial class UpdateLifeRegen
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref int damage
        );

        public static event Definition Event;
    }

    public static partial class CheckActive
    {
        public delegate bool Definition(
            Terraria.NPC npc
        );

        public static event Definition Event;
    }

    public static partial class CheckDead
    {
        public delegate bool Definition(
            Terraria.NPC npc
        );

        public static event Definition Event;
    }

    public static partial class SpecialOnKill
    {
        public delegate bool Definition(
            Terraria.NPC npc
        );

        public static event Definition Event;
    }

    public static partial class PreKill
    {
        public delegate bool Definition(
            Terraria.NPC npc
        );

        public static event Definition Event;
    }

    public static partial class OnKill
    {
        public delegate void Definition(
            Terraria.NPC npc
        );

        public static event Definition Event;
    }

    public static partial class CanFallThroughPlatforms
    {
        public delegate bool? Definition(
            Terraria.NPC npc
        );

        public static event Definition Event;
    }

    public static partial class CanBeCaughtBy
    {
        public delegate bool? Definition(
            Terraria.NPC npc,
            Terraria.Item item,
            Terraria.Player player
        );

        public static event Definition Event;
    }

    public static partial class OnCaughtBy
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.Player player,
            Terraria.Item item,
            bool failed
        );

        public static event Definition Event;
    }

    public static partial class ModifyNPCLoot
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.ModLoader.NPCLoot npcLoot
        );

        public static event Definition Event;
    }

    public static partial class ModifyGlobalLoot
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalLoot globalLoot
        );

        public static event Definition Event;
    }

    public static partial class CanHitPlayer
    {
        public delegate bool Definition(
            Terraria.NPC npc,
            Terraria.Player target,
            ref int cooldownSlot
        );

        public static event Definition Event;
    }

    public static partial class ModifyHitPlayer
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.Player target,
            ref Terraria.Player.HurtModifiers modifiers
        );

        public static event Definition Event;
    }

    public static partial class OnHitPlayer
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.Player target,
            Terraria.Player.HurtInfo hurtInfo
        );

        public static event Definition Event;
    }

    public static partial class CanHitNPC
    {
        public delegate bool Definition(
            Terraria.NPC npc,
            Terraria.NPC target
        );

        public static event Definition Event;
    }

    public static partial class CanBeHitByNPC
    {
        public delegate bool Definition(
            Terraria.NPC npc,
            Terraria.NPC attacker
        );

        public static event Definition Event;
    }

    public static partial class ModifyHitNPC
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.NPC target,
            ref Terraria.NPC.HitModifiers modifiers
        );

        public static event Definition Event;
    }

    public static partial class OnHitNPC
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.NPC target,
            Terraria.NPC.HitInfo hit
        );

        public static event Definition Event;
    }

    public static partial class CanBeHitByItem
    {
        public delegate bool? Definition(
            Terraria.NPC npc,
            Terraria.Player player,
            Terraria.Item item
        );

        public static event Definition Event;
    }

    public static partial class CanCollideWithPlayerMeleeAttack
    {
        public delegate bool? Definition(
            Terraria.NPC npc,
            Terraria.Player player,
            Terraria.Item item,
            Microsoft.Xna.Framework.Rectangle meleeAttackHitbox
        );

        public static event Definition Event;
    }

    public static partial class ModifyHitByItem
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.Player player,
            Terraria.Item item,
            ref Terraria.NPC.HitModifiers modifiers
        );

        public static event Definition Event;
    }

    public static partial class OnHitByItem
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.Player player,
            Terraria.Item item,
            Terraria.NPC.HitInfo hit,
            int damageDone
        );

        public static event Definition Event;
    }

    public static partial class CanBeHitByProjectile
    {
        public delegate bool? Definition(
            Terraria.NPC npc,
            Terraria.Projectile projectile
        );

        public static event Definition Event;
    }

    public static partial class ModifyHitByProjectile
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.Projectile projectile,
            ref Terraria.NPC.HitModifiers modifiers
        );

        public static event Definition Event;
    }

    public static partial class OnHitByProjectile
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.Projectile projectile,
            Terraria.NPC.HitInfo hit,
            int damageDone
        );

        public static event Definition Event;
    }

    public static partial class ModifyIncomingHit
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref Terraria.NPC.HitModifiers modifiers
        );

        public static event Definition Event;
    }

    public static partial class BossHeadSlot
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref int index
        );

        public static event Definition Event;
    }

    public static partial class BossHeadRotation
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref float rotation
        );

        public static event Definition Event;
    }

    public static partial class BossHeadSpriteEffects
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref Microsoft.Xna.Framework.Graphics.SpriteEffects spriteEffects
        );

        public static event Definition Event;
    }

    public static partial class GetAlpha
    {
        public delegate Microsoft.Xna.Framework.Color? Definition(
            Terraria.NPC npc,
            Microsoft.Xna.Framework.Color drawColor
        );

        public static event Definition Event;
    }

    public static partial class DrawEffects
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref Microsoft.Xna.Framework.Color drawColor
        );

        public static event Definition Event;
    }

    public static partial class PreDraw
    {
        public delegate bool Definition(
            Terraria.NPC npc,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Microsoft.Xna.Framework.Vector2 screenPos,
            Microsoft.Xna.Framework.Color drawColor
        );

        public static event Definition Event;
    }

    public static partial class PostDraw
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch,
            Microsoft.Xna.Framework.Vector2 screenPos,
            Microsoft.Xna.Framework.Color drawColor
        );

        public static event Definition Event;
    }

    public static partial class DrawBehind
    {
        public delegate void Definition(
            Terraria.NPC npc,
            int index
        );

        public static event Definition Event;
    }

    public static partial class DrawHealthBar
    {
        public delegate bool? Definition(
            Terraria.NPC npc,
            byte hbPosition,
            ref float scale,
            ref Microsoft.Xna.Framework.Vector2 position
        );

        public static event Definition Event;
    }

    public static partial class EditSpawnRate
    {
        public delegate void Definition(
            Terraria.Player player,
            ref int spawnRate,
            ref int maxSpawns
        );

        public static event Definition Event;
    }

    public static partial class EditSpawnRange
    {
        public delegate void Definition(
            Terraria.Player player,
            ref int spawnRangeX,
            ref int spawnRangeY,
            ref int safeRangeX,
            ref int safeRangeY
        );

        public static event Definition Event;
    }

    public static partial class EditSpawnPool
    {
        public delegate void Definition(
            System.Collections.Generic.IDictionary<int, float> pool,
            Terraria.ModLoader.NPCSpawnInfo spawnInfo
        );

        public static event Definition Event;
    }

    public static partial class SpawnNPC
    {
        public delegate void Definition(
            int npc,
            int tileX,
            int tileY
        );

        public static event Definition Event;
    }

    public static partial class CanChat
    {
        public delegate bool? Definition(
            Terraria.NPC npc
        );

        public static event Definition Event;
    }

    public static partial class GetChat
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref string chat
        );

        public static event Definition Event;
    }

    public static partial class PreChatButtonClicked
    {
        public delegate bool Definition(
            Terraria.NPC npc,
            bool firstButton
        );

        public static event Definition Event;
    }

    public static partial class OnChatButtonClicked
    {
        public delegate void Definition(
            Terraria.NPC npc,
            bool firstButton
        );

        public static event Definition Event;
    }

    public static partial class ModifyShop
    {
        public delegate void Definition(
            Terraria.ModLoader.NPCShop shop
        );

        public static event Definition Event;
    }

    public static partial class ModifyActiveShop
    {
        public delegate void Definition(
            Terraria.NPC npc,
            string shopName,
            Terraria.Item[] items
        );

        public static event Definition Event;
    }

    public static partial class SetupTravelShop
    {
        public delegate void Definition(
            System.Int32[] shop,
            ref int nextSlot
        );

        public static event Definition Event;
    }

    public static partial class CanGoToStatue
    {
        public delegate bool? Definition(
            Terraria.NPC npc,
            bool toKingStatue
        );

        public static event Definition Event;
    }

    public static partial class OnGoToStatue
    {
        public delegate void Definition(
            Terraria.NPC npc,
            bool toKingStatue
        );

        public static event Definition Event;
    }

    public static partial class BuffTownNPC
    {
        public delegate void Definition(
            ref float damageMult,
            ref int defense
        );

        public static event Definition Event;
    }

    public static partial class TownNPCAttackStrength
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref int damage,
            ref float knockback
        );

        public static event Definition Event;
    }

    public static partial class TownNPCAttackCooldown
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref int cooldown,
            ref int randExtraCooldown
        );

        public static event Definition Event;
    }

    public static partial class TownNPCAttackProj
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref int projType,
            ref int attackDelay
        );

        public static event Definition Event;
    }

    public static partial class TownNPCAttackProjSpeed
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref float multiplier,
            ref float gravityCorrection,
            ref float randomOffset
        );

        public static event Definition Event;
    }

    public static partial class TownNPCAttackShoot
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref bool inBetweenShots
        );

        public static event Definition Event;
    }

    public static partial class TownNPCAttackMagic
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref float auraLightMultiplier
        );

        public static event Definition Event;
    }

    public static partial class TownNPCAttackSwing
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref int itemWidth,
            ref int itemHeight
        );

        public static event Definition Event;
    }

    public static partial class DrawTownAttackGun
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref Microsoft.Xna.Framework.Graphics.Texture2D item,
            ref Microsoft.Xna.Framework.Rectangle itemFrame,
            ref float scale,
            ref int horizontalHoldoutOffset
        );

        public static event Definition Event;
    }

    public static partial class DrawTownAttackSwing
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref Microsoft.Xna.Framework.Graphics.Texture2D item,
            ref Microsoft.Xna.Framework.Rectangle itemFrame,
            ref int itemSize,
            ref float scale,
            ref Microsoft.Xna.Framework.Vector2 offset
        );

        public static event Definition Event;
    }

    public static partial class ModifyCollisionData
    {
        public delegate bool Definition(
            Terraria.NPC npc,
            Microsoft.Xna.Framework.Rectangle victimHitbox,
            ref int immunityCooldownSlot,
            ref Terraria.ModLoader.MultipliableFloat damageMultiplier,
            ref Microsoft.Xna.Framework.Rectangle npcHitbox
        );

        public static event Definition Event;
    }

    public static partial class NeedSaving
    {
        public delegate bool Definition(
            Terraria.NPC npc
        );

        public static event Definition Event;
    }

    public static partial class SaveData
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.ModLoader.IO.TagCompound tag
        );

        public static event Definition Event;
    }

    public static partial class LoadData
    {
        public delegate void Definition(
            Terraria.NPC npc,
            Terraria.ModLoader.IO.TagCompound tag
        );

        public static event Definition Event;
    }

    public static partial class PickEmote
    {
        public delegate int? Definition(
            Terraria.NPC npc,
            Terraria.Player closestPlayer,
            System.Collections.Generic.List<int> emoteList,
            Terraria.GameContent.UI.WorldUIAnchor otherAnchor
        );

        public static event Definition Event;
    }

    public static partial class ChatBubblePosition
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref Microsoft.Xna.Framework.Vector2 position,
            ref Microsoft.Xna.Framework.Graphics.SpriteEffects spriteEffects
        );

        public static event Definition Event;
    }

    public static partial class PartyHatPosition
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref Microsoft.Xna.Framework.Vector2 position,
            ref Microsoft.Xna.Framework.Graphics.SpriteEffects spriteEffects
        );

        public static event Definition Event;
    }

    public static partial class EmoteBubblePosition
    {
        public delegate void Definition(
            Terraria.NPC npc,
            ref Microsoft.Xna.Framework.Vector2 position,
            ref Microsoft.Xna.Framework.Graphics.SpriteEffects spriteEffects
        );

        public static event Definition Event;
    }

}
