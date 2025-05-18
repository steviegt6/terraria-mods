namespace Daybreak.Common.Features.Hooks;

using System.Linq;

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// Hooks to generate for 'Terraria.ModLoader.GlobalProjectile':
//     System.Void Terraria.ModLoader.GlobalProjectile::OnSpawn(Terraria.Projectile,Terraria.DataStructures.IEntitySource)
//     System.Boolean Terraria.ModLoader.GlobalProjectile::PreAI(Terraria.Projectile)
//     System.Void Terraria.ModLoader.GlobalProjectile::AI(Terraria.Projectile)
//     System.Void Terraria.ModLoader.GlobalProjectile::PostAI(Terraria.Projectile)
//     System.Void Terraria.ModLoader.GlobalProjectile::SendExtraAI(Terraria.Projectile,Terraria.ModLoader.IO.BitWriter,System.IO.BinaryWriter)
//     System.Void Terraria.ModLoader.GlobalProjectile::ReceiveExtraAI(Terraria.Projectile,Terraria.ModLoader.IO.BitReader,System.IO.BinaryReader)
//     System.Boolean Terraria.ModLoader.GlobalProjectile::ShouldUpdatePosition(Terraria.Projectile)
//     System.Boolean Terraria.ModLoader.GlobalProjectile::TileCollideStyle(Terraria.Projectile,System.Int32&,System.Int32&,System.Boolean&,Microsoft.Xna.Framework.Vector2&)
//     System.Boolean Terraria.ModLoader.GlobalProjectile::OnTileCollide(Terraria.Projectile,Microsoft.Xna.Framework.Vector2)
//     System.Boolean Terraria.ModLoader.GlobalProjectile::PreKill(Terraria.Projectile,System.Int32)
//     System.Void Terraria.ModLoader.GlobalProjectile::OnKill(Terraria.Projectile,System.Int32)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalProjectile::CanCutTiles(Terraria.Projectile)
//     System.Void Terraria.ModLoader.GlobalProjectile::CutTiles(Terraria.Projectile)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalProjectile::CanDamage(Terraria.Projectile)
//     System.Boolean Terraria.ModLoader.GlobalProjectile::MinionContactDamage(Terraria.Projectile)
//     System.Void Terraria.ModLoader.GlobalProjectile::ModifyDamageHitbox(Terraria.Projectile,Microsoft.Xna.Framework.Rectangle&)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalProjectile::CanHitNPC(Terraria.Projectile,Terraria.NPC)
//     System.Void Terraria.ModLoader.GlobalProjectile::ModifyHitNPC(Terraria.Projectile,Terraria.NPC,Terraria.NPC/HitModifiers&)
//     System.Void Terraria.ModLoader.GlobalProjectile::OnHitNPC(Terraria.Projectile,Terraria.NPC,Terraria.NPC/HitInfo,System.Int32)
//     System.Boolean Terraria.ModLoader.GlobalProjectile::CanHitPvp(Terraria.Projectile,Terraria.Player)
//     System.Boolean Terraria.ModLoader.GlobalProjectile::CanHitPlayer(Terraria.Projectile,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalProjectile::ModifyHitPlayer(Terraria.Projectile,Terraria.Player,Terraria.Player/HurtModifiers&)
//     System.Void Terraria.ModLoader.GlobalProjectile::OnHitPlayer(Terraria.Projectile,Terraria.Player,Terraria.Player/HurtInfo)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalProjectile::Colliding(Terraria.Projectile,Microsoft.Xna.Framework.Rectangle,Microsoft.Xna.Framework.Rectangle)
//     System.Nullable`1<Microsoft.Xna.Framework.Color> Terraria.ModLoader.GlobalProjectile::GetAlpha(Terraria.Projectile,Microsoft.Xna.Framework.Color)
//     System.Boolean Terraria.ModLoader.GlobalProjectile::PreDrawExtras(Terraria.Projectile)
//     System.Boolean Terraria.ModLoader.GlobalProjectile::PreDraw(Terraria.Projectile,Microsoft.Xna.Framework.Color&)
//     System.Void Terraria.ModLoader.GlobalProjectile::PostDraw(Terraria.Projectile,Microsoft.Xna.Framework.Color)
//     System.Void Terraria.ModLoader.GlobalProjectile::DrawBehind(Terraria.Projectile,System.Int32,System.Collections.Generic.List`1<System.Int32>,System.Collections.Generic.List`1<System.Int32>,System.Collections.Generic.List`1<System.Int32>,System.Collections.Generic.List`1<System.Int32>,System.Collections.Generic.List`1<System.Int32>)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalProjectile::CanUseGrapple(System.Int32,Terraria.Player)
//     System.Void Terraria.ModLoader.GlobalProjectile::UseGrapple(Terraria.Player,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalProjectile::NumGrappleHooks(Terraria.Projectile,Terraria.Player,System.Int32&)
//     System.Void Terraria.ModLoader.GlobalProjectile::GrappleRetreatSpeed(Terraria.Projectile,Terraria.Player,System.Single&)
//     System.Void Terraria.ModLoader.GlobalProjectile::GrapplePullSpeed(Terraria.Projectile,Terraria.Player,System.Single&)
//     System.Void Terraria.ModLoader.GlobalProjectile::GrappleTargetPoint(Terraria.Projectile,Terraria.Player,System.Single&,System.Single&)
//     System.Nullable`1<System.Boolean> Terraria.ModLoader.GlobalProjectile::GrappleCanLatchOnTo(Terraria.Projectile,Terraria.Player,System.Int32,System.Int32)
//     System.Void Terraria.ModLoader.GlobalProjectile::PrepareBombToBlow(Terraria.Projectile)
//     System.Void Terraria.ModLoader.GlobalProjectile::EmitEnchantmentVisualsAt(Terraria.Projectile,Microsoft.Xna.Framework.Vector2,System.Int32,System.Int32)
public static partial class GlobalProjectileHooks
{
    public static partial class OnSpawn
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.DataStructures.IEntitySource source
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.DataStructures.IEntitySource source
        )
        {
            Event?.Invoke(self, projectile, source);
        }
    }

    public static partial class PreAI
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        )
        {
            Event?.Invoke(self, projectile);
        }
    }

    public static partial class AI
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        )
        {
            Event?.Invoke(self, projectile);
        }
    }

    public static partial class PostAI
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        )
        {
            Event?.Invoke(self, projectile);
        }
    }

    public static partial class SendExtraAI
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.ModLoader.IO.BitWriter bitWriter,
            System.IO.BinaryWriter binaryWriter
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.ModLoader.IO.BitWriter bitWriter,
            System.IO.BinaryWriter binaryWriter
        )
        {
            Event?.Invoke(self, projectile, bitWriter, binaryWriter);
        }
    }

    public static partial class ReceiveExtraAI
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.ModLoader.IO.BitReader bitReader,
            System.IO.BinaryReader binaryReader
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.ModLoader.IO.BitReader bitReader,
            System.IO.BinaryReader binaryReader
        )
        {
            Event?.Invoke(self, projectile, bitReader, binaryReader);
        }
    }

    public static partial class ShouldUpdatePosition
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        )
        {
            Event?.Invoke(self, projectile);
        }
    }

    public static partial class TileCollideStyle
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            ref int width,
            ref int height,
            ref bool fallThrough,
            ref Microsoft.Xna.Framework.Vector2 hitboxCenterFrac
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            ref int width,
            ref int height,
            ref bool fallThrough,
            ref Microsoft.Xna.Framework.Vector2 hitboxCenterFrac
        )
        {
            Event?.Invoke(self, projectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
    }

    public static partial class OnTileCollide
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Microsoft.Xna.Framework.Vector2 oldVelocity
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Microsoft.Xna.Framework.Vector2 oldVelocity
        )
        {
            Event?.Invoke(self, projectile, oldVelocity);
        }
    }

    public static partial class PreKill
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            int timeLeft
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            int timeLeft
        )
        {
            Event?.Invoke(self, projectile, timeLeft);
        }
    }

    public static partial class OnKill
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            int timeLeft
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            int timeLeft
        )
        {
            Event?.Invoke(self, projectile, timeLeft);
        }
    }

    public static partial class CanCutTiles
    {
        public delegate bool? Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool? Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        )
        {
            Event?.Invoke(self, projectile);
        }
    }

    public static partial class CutTiles
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        )
        {
            Event?.Invoke(self, projectile);
        }
    }

    public static partial class CanDamage
    {
        public delegate bool? Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool? Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        )
        {
            Event?.Invoke(self, projectile);
        }
    }

    public static partial class MinionContactDamage
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        )
        {
            Event?.Invoke(self, projectile);
        }
    }

    public static partial class ModifyDamageHitbox
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            ref Microsoft.Xna.Framework.Rectangle hitbox
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            ref Microsoft.Xna.Framework.Rectangle hitbox
        )
        {
            Event?.Invoke(self, projectile, ref hitbox);
        }
    }

    public static partial class CanHitNPC
    {
        public delegate bool? Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.NPC target
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool? Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.NPC target
        )
        {
            Event?.Invoke(self, projectile, target);
        }
    }

    public static partial class ModifyHitNPC
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.NPC target,
            ref Terraria.NPC.HitModifiers modifiers
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.NPC target,
            ref Terraria.NPC.HitModifiers modifiers
        )
        {
            Event?.Invoke(self, projectile, target, ref modifiers);
        }
    }

    public static partial class OnHitNPC
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
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
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.NPC target,
            Terraria.NPC.HitInfo hit,
            int damageDone
        )
        {
            Event?.Invoke(self, projectile, target, hit, damageDone);
        }
    }

    public static partial class CanHitPvp
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player target
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player target
        )
        {
            Event?.Invoke(self, projectile, target);
        }
    }

    public static partial class CanHitPlayer
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player target
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player target
        )
        {
            Event?.Invoke(self, projectile, target);
        }
    }

    public static partial class ModifyHitPlayer
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player target,
            ref Terraria.Player.HurtModifiers modifiers
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player target,
            ref Terraria.Player.HurtModifiers modifiers
        )
        {
            Event?.Invoke(self, projectile, target, ref modifiers);
        }
    }

    public static partial class OnHitPlayer
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player target,
            Terraria.Player.HurtInfo info
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player target,
            Terraria.Player.HurtInfo info
        )
        {
            Event?.Invoke(self, projectile, target, info);
        }
    }

    public static partial class Colliding
    {
        public delegate bool? Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Microsoft.Xna.Framework.Rectangle projHitbox,
            Microsoft.Xna.Framework.Rectangle targetHitbox
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool? Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Microsoft.Xna.Framework.Rectangle projHitbox,
            Microsoft.Xna.Framework.Rectangle targetHitbox
        )
        {
            Event?.Invoke(self, projectile, projHitbox, targetHitbox);
        }
    }

    public static partial class GetAlpha
    {
        public delegate Microsoft.Xna.Framework.Color? Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Microsoft.Xna.Framework.Color lightColor
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static Microsoft.Xna.Framework.Color? Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Microsoft.Xna.Framework.Color lightColor
        )
        {
            Event?.Invoke(self, projectile, lightColor);
        }
    }

    public static partial class PreDrawExtras
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        )
        {
            Event?.Invoke(self, projectile);
        }
    }

    public static partial class PreDraw
    {
        public delegate bool Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            ref Microsoft.Xna.Framework.Color lightColor
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            ref Microsoft.Xna.Framework.Color lightColor
        )
        {
            Event?.Invoke(self, projectile, ref lightColor);
        }
    }

    public static partial class PostDraw
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Microsoft.Xna.Framework.Color lightColor
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Microsoft.Xna.Framework.Color lightColor
        )
        {
            Event?.Invoke(self, projectile, lightColor);
        }
    }

    public static partial class DrawBehind
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            int index,
            System.Collections.Generic.List<int> behindNPCsAndTiles,
            System.Collections.Generic.List<int> behindNPCs,
            System.Collections.Generic.List<int> behindProjectiles,
            System.Collections.Generic.List<int> overPlayers,
            System.Collections.Generic.List<int> overWiresUI
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            int index,
            System.Collections.Generic.List<int> behindNPCsAndTiles,
            System.Collections.Generic.List<int> behindNPCs,
            System.Collections.Generic.List<int> behindProjectiles,
            System.Collections.Generic.List<int> overPlayers,
            System.Collections.Generic.List<int> overWiresUI
        )
        {
            Event?.Invoke(self, projectile, index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
    }

    public static partial class CanUseGrapple
    {
        public delegate bool? Definition(
            Terraria.ModLoader.GlobalProjectile self,
            int type,
            Terraria.Player player
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool? Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            int type,
            Terraria.Player player
        )
        {
            Event?.Invoke(self, type, player);
        }
    }

    public static partial class UseGrapple
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Player player,
            ref int type
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Player player,
            ref int type
        )
        {
            Event?.Invoke(self, player, ref type);
        }
    }

    public static partial class NumGrappleHooks
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player player,
            ref int numHooks
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player player,
            ref int numHooks
        )
        {
            Event?.Invoke(self, projectile, player, ref numHooks);
        }
    }

    public static partial class GrappleRetreatSpeed
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player player,
            ref float speed
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player player,
            ref float speed
        )
        {
            Event?.Invoke(self, projectile, player, ref speed);
        }
    }

    public static partial class GrapplePullSpeed
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player player,
            ref float speed
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player player,
            ref float speed
        )
        {
            Event?.Invoke(self, projectile, player, ref speed);
        }
    }

    public static partial class GrappleTargetPoint
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player player,
            ref float grappleX,
            ref float grappleY
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player player,
            ref float grappleX,
            ref float grappleY
        )
        {
            Event?.Invoke(self, projectile, player, ref grappleX, ref grappleY);
        }
    }

    public static partial class GrappleCanLatchOnTo
    {
        public delegate bool? Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player player,
            int x,
            int y
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static bool? Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Terraria.Player player,
            int x,
            int y
        )
        {
            Event?.Invoke(self, projectile, player, x, y);
        }
    }

    public static partial class PrepareBombToBlow
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }

        public static void Invoke(
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile
        )
        {
            Event?.Invoke(self, projectile);
        }
    }

    public static partial class EmitEnchantmentVisualsAt
    {
        public delegate void Definition(
            Terraria.ModLoader.GlobalProjectile self,
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
            Terraria.ModLoader.GlobalProjectile self,
            Terraria.Projectile projectile,
            Microsoft.Xna.Framework.Vector2 boxPosition,
            int boxWidth,
            int boxHeight
        )
        {
            Event?.Invoke(self, projectile, boxPosition, boxWidth, boxHeight);
        }
    }
}
