using Terraria.ModLoader;

namespace Daybreak.Common.Features.Hooks;

// OnModLoad and OnModUnload are left out because they'd only be run during
// DAYBREAK's load cycle instead of per-mod.  Would be weird and not expected
// behavior.

/// <summary>
///     Hook definitions for <see cref="ModSystem"/> APIs.
/// </summary>
public static partial class ModSystemHooks
{
    [HookDefinition]
    public partial interface ISetupContent
    {
        public delegate void Description();
    
        // Description.Type Action(Description.Parameters);

        // public static event Description Event;
    }

    public partial interface IPostSetupContent
    {
    }

    public partial interface IOnLocalizationsLoaded
    {
    
    }

    private sealed class ModSystemImpl : ModSystem
    {
        public override void SetupContent()
        {
            base.SetupContent();
        }

        public override void PostSetupContent()
        {
            base.PostSetupContent();
        }

        public override void OnLocalizationsLoaded()
        {
            base.OnLocalizationsLoaded();
        }
    }
}