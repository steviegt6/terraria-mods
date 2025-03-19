using System.Reflection;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.UI;
using Terraria.UI;

using Tomat.TML.Mod.FullProjectDecompiler.Common;

namespace Tomat.TML.Mod.FullProjectDecompiler.Content.UI;

internal sealed class DecompileModUi : UIProgress
{
    private sealed class InjectIntoExtractModAction : IInitializer
    {
        void ILoadable.Load(global::Terraria.ModLoader.Mod mod)
        {
            MonoModHooks.Add(
                typeof(UIModInfo).GetMethod("ExtractMod", BindingFlags.NonPublic | BindingFlags.Instance)!,
                DecompileMod
            );
        }

        private static void DecompileMod(UIModInfo self, UIMouseEvent _, UIElement __)
        {
            SoundEngine.PlaySound(SoundID.MenuOpen);
            instance.Show(self._localMod, self._gotoMenu);
        }
    }

    private static readonly DecompileModUi instance = new();

    private LocalMod? mod;

    private void Show(LocalMod modToShow, int menuToGoto)
    {
        mod      = modToShow;
        gotoMenu = menuToGoto;

        Main.MenuUI.SetState(this);
        Main.menuMode = 888;
    }
}