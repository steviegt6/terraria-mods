namespace Daybreak.Common.Features.Hooks;

using System.Linq;

// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// Hooks to generate for 'Terraria.ModLoader.ModSystem':
//     System.Void Terraria.ModLoader.ModSystem::PostSetupContent()
//     System.Void Terraria.ModLoader.ModSystem::OnLocalizationsLoaded()
//     System.Void Terraria.ModLoader.ModSystem::AddRecipes()
//     System.Void Terraria.ModLoader.ModSystem::PostAddRecipes()
//     System.Void Terraria.ModLoader.ModSystem::PostSetupRecipes()
//     System.Void Terraria.ModLoader.ModSystem::AddRecipeGroups()
//     System.Void Terraria.ModLoader.ModSystem::OnWorldLoad()
//     System.Void Terraria.ModLoader.ModSystem::OnWorldUnload()
//     System.Void Terraria.ModLoader.ModSystem::ClearWorld()
//     System.Void Terraria.ModLoader.ModSystem::ModifyScreenPosition()
//     System.Void Terraria.ModLoader.ModSystem::ModifyTransformMatrix(Terraria.Graphics.SpriteViewMatrix&)
//     System.Void Terraria.ModLoader.ModSystem::UpdateUI(Microsoft.Xna.Framework.GameTime)
//     System.Void Terraria.ModLoader.ModSystem::PreUpdateEntities()
//     System.Void Terraria.ModLoader.ModSystem::PreUpdatePlayers()
//     System.Void Terraria.ModLoader.ModSystem::PostUpdatePlayers()
//     System.Void Terraria.ModLoader.ModSystem::PreUpdateNPCs()
//     System.Void Terraria.ModLoader.ModSystem::PostUpdateNPCs()
//     System.Void Terraria.ModLoader.ModSystem::PreUpdateGores()
//     System.Void Terraria.ModLoader.ModSystem::PostUpdateGores()
//     System.Void Terraria.ModLoader.ModSystem::PreUpdateProjectiles()
//     System.Void Terraria.ModLoader.ModSystem::PostUpdateProjectiles()
//     System.Void Terraria.ModLoader.ModSystem::PreUpdateItems()
//     System.Void Terraria.ModLoader.ModSystem::PostUpdateItems()
//     System.Void Terraria.ModLoader.ModSystem::PreUpdateDusts()
//     System.Void Terraria.ModLoader.ModSystem::PostUpdateDusts()
//     System.Void Terraria.ModLoader.ModSystem::PreUpdateTime()
//     System.Void Terraria.ModLoader.ModSystem::PostUpdateTime()
//     System.Void Terraria.ModLoader.ModSystem::PreUpdateWorld()
//     System.Void Terraria.ModLoader.ModSystem::PostUpdateWorld()
//     System.Void Terraria.ModLoader.ModSystem::PreUpdateInvasions()
//     System.Void Terraria.ModLoader.ModSystem::PostUpdateInvasions()
//     System.Void Terraria.ModLoader.ModSystem::PostUpdateEverything()
//     System.Void Terraria.ModLoader.ModSystem::ModifyInterfaceLayers(System.Collections.Generic.List`1<Terraria.UI.GameInterfaceLayer>)
//     System.Void Terraria.ModLoader.ModSystem::ModifyGameTipVisibility(System.Collections.Generic.IReadOnlyList`1<Terraria.ModLoader.GameTipData>)
//     System.Void Terraria.ModLoader.ModSystem::PostDrawInterface(Microsoft.Xna.Framework.Graphics.SpriteBatch)
//     System.Void Terraria.ModLoader.ModSystem::PreDrawMapIconOverlay(System.Collections.Generic.IReadOnlyList`1<Terraria.Map.IMapLayer>,Terraria.Map.MapOverlayDrawContext)
//     System.Void Terraria.ModLoader.ModSystem::PostDrawFullscreenMap(System.String&)
//     System.Void Terraria.ModLoader.ModSystem::PostUpdateInput()
//     System.Void Terraria.ModLoader.ModSystem::PreSaveAndQuit()
//     System.Void Terraria.ModLoader.ModSystem::PostDrawTiles()
//     System.Void Terraria.ModLoader.ModSystem::ModifyTimeRate(System.Double&,System.Double&,System.Double&)
//     System.Void Terraria.ModLoader.ModSystem::SaveWorldData(Terraria.ModLoader.IO.TagCompound)
//     System.Void Terraria.ModLoader.ModSystem::LoadWorldData(Terraria.ModLoader.IO.TagCompound)
//     System.Void Terraria.ModLoader.ModSystem::SaveWorldHeader(Terraria.ModLoader.IO.TagCompound)
//     System.Boolean Terraria.ModLoader.ModSystem::CanWorldBePlayed(Terraria.IO.PlayerFileData,Terraria.IO.WorldFileData)
//     System.String Terraria.ModLoader.ModSystem::WorldCanBePlayedRejectionMessage(Terraria.IO.PlayerFileData,Terraria.IO.WorldFileData)
//     System.Void Terraria.ModLoader.ModSystem::NetSend(System.IO.BinaryWriter)
//     System.Void Terraria.ModLoader.ModSystem::NetReceive(System.IO.BinaryReader)
//     System.Boolean Terraria.ModLoader.ModSystem::HijackGetData(System.Byte&,System.IO.BinaryReader&,System.Int32)
//     System.Boolean Terraria.ModLoader.ModSystem::HijackSendData(System.Int32,System.Int32,System.Int32,System.Int32,Terraria.Localization.NetworkText,System.Int32,System.Single,System.Single,System.Single,System.Int32,System.Int32,System.Int32)
//     System.Void Terraria.ModLoader.ModSystem::PreWorldGen()
//     System.Void Terraria.ModLoader.ModSystem::ModifyWorldGenTasks(System.Collections.Generic.List`1<Terraria.WorldBuilding.GenPass>,System.Double&)
//     System.Void Terraria.ModLoader.ModSystem::PostWorldGen()
//     System.Void Terraria.ModLoader.ModSystem::ResetNearbyTileEffects()
//     System.Void Terraria.ModLoader.ModSystem::ModifyHardmodeTasks(System.Collections.Generic.List`1<Terraria.WorldBuilding.GenPass>)
//     System.Void Terraria.ModLoader.ModSystem::ModifySunLightColor(Microsoft.Xna.Framework.Color&,Microsoft.Xna.Framework.Color&)
//     System.Void Terraria.ModLoader.ModSystem::ModifyLightingBrightness(System.Single&)
//     System.Void Terraria.ModLoader.ModSystem::TileCountsAvailable(System.ReadOnlySpan`1<System.Int32>)
//     System.Void Terraria.ModLoader.ModSystem::ResizeArrays()
public static partial class ModSystemHooks
{
    public static partial class PostSetupContent
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class OnLocalizationsLoaded
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class AddRecipes
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostAddRecipes
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostSetupRecipes
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class AddRecipeGroups
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class OnWorldLoad
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class OnWorldUnload
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class ClearWorld
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class ModifyScreenPosition
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class ModifyTransformMatrix
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            ref Terraria.Graphics.SpriteViewMatrix Transform
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class UpdateUI
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            Microsoft.Xna.Framework.GameTime gameTime
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PreUpdateEntities
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PreUpdatePlayers
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostUpdatePlayers
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PreUpdateNPCs
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostUpdateNPCs
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PreUpdateGores
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostUpdateGores
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PreUpdateProjectiles
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostUpdateProjectiles
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PreUpdateItems
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostUpdateItems
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PreUpdateDusts
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostUpdateDusts
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PreUpdateTime
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostUpdateTime
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PreUpdateWorld
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostUpdateWorld
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PreUpdateInvasions
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostUpdateInvasions
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostUpdateEverything
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class ModifyInterfaceLayers
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            System.Collections.Generic.List<Terraria.UI.GameInterfaceLayer> layers
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class ModifyGameTipVisibility
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            System.Collections.Generic.IReadOnlyList<Terraria.ModLoader.GameTipData> gameTips
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostDrawInterface
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PreDrawMapIconOverlay
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            System.Collections.Generic.IReadOnlyList<Terraria.Map.IMapLayer> layers,
            Terraria.Map.MapOverlayDrawContext mapOverlayDrawContext
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostDrawFullscreenMap
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            ref string mouseText
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostUpdateInput
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PreSaveAndQuit
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostDrawTiles
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class ModifyTimeRate
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            ref double timeRate,
            ref double tileUpdateRate,
            ref double eventUpdateRate
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class SaveWorldData
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            Terraria.ModLoader.IO.TagCompound tag
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class LoadWorldData
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            Terraria.ModLoader.IO.TagCompound tag
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class SaveWorldHeader
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            Terraria.ModLoader.IO.TagCompound tag
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class CanWorldBePlayed
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModSystem self,
            Terraria.IO.PlayerFileData playerData,
            Terraria.IO.WorldFileData worldFileData
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class WorldCanBePlayedRejectionMessage
    {
        public delegate string Definition(
            Terraria.ModLoader.ModSystem self,
            Terraria.IO.PlayerFileData playerData,
            Terraria.IO.WorldFileData worldData
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class NetSend
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            System.IO.BinaryWriter writer
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class NetReceive
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            System.IO.BinaryReader reader
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class HijackGetData
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModSystem self,
            ref byte messageType,
            ref System.IO.BinaryReader reader,
            int playerNumber
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class HijackSendData
    {
        public delegate bool Definition(
            Terraria.ModLoader.ModSystem self,
            int whoAmI,
            int msgType,
            int remoteClient,
            int ignoreClient,
            Terraria.Localization.NetworkText text,
            int number,
            float number2,
            float number3,
            float number4,
            int number5,
            int number6,
            int number7
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PreWorldGen
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class ModifyWorldGenTasks
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            System.Collections.Generic.List<Terraria.WorldBuilding.GenPass> tasks,
            ref double totalWeight
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class PostWorldGen
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class ResetNearbyTileEffects
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class ModifyHardmodeTasks
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            System.Collections.Generic.List<Terraria.WorldBuilding.GenPass> list
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class ModifySunLightColor
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            ref Microsoft.Xna.Framework.Color tileColor,
            ref Microsoft.Xna.Framework.Color backgroundColor
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class ModifyLightingBrightness
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            ref float scale
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class TileCountsAvailable
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self,
            System.ReadOnlySpan<int> tileCounts
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }

    public static partial class ResizeArrays
    {
        public delegate void Definition(
            Terraria.ModLoader.ModSystem self
        );

        public static event Definition? Event;

        internal static System.Collections.Generic.IEnumerable<Definition> GetInvocationList()
        {
            return Event?.GetInvocationList().Select(x => (Definition)x) ?? [];
        }
    }
}
