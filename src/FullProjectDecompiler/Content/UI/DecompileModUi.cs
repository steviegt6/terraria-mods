using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

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

    private CancellationTokenSource? cts;

    public override void OnActivate()
    {
        base.OnActivate();

        cts = new CancellationTokenSource();
        {
            OnCancel += () =>
            {
                // TODO: Should canceling clean up unfinished working directories?
                cts.Cancel();
            };
        }

        Task.Run(Decompile, cts.Token);
    }

    private void Show(LocalMod modToShow, int menuToGoto)
    {
        mod      = modToShow;
        gotoMenu = menuToGoto;

        Main.MenuUI.SetState(this);
        Main.menuMode = 888;
    }

    private Task Decompile()
    {
        if (mod is null)
        {
            return Task.FromResult(false);
        }

        var dir = Path.Combine(Main.SavePath, "ModSources", "decompiled", mod.Name);
        {
            // Be sure to clean up previous decompilation attempts.
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }

            Directory.CreateDirectory(dir);
        }

        var modHandle = default(IDisposable?);

        try { }
        catch (OperationCanceledException)
        {
            return Task.FromResult(false);
        }
        catch (Exception e)
        {
            // TODO: Logging!

            Main.menuMode = gotoMenu;
            return Task.FromResult(false);
        }
        finally
        {
            modHandle?.Dispose();
        }

        // Assuming everything went well since we got here.  Open the directory.
        Utils.OpenFolder(dir);

        Main.menuMode = gotoMenu;
        return Task.FromResult(true);
    }
}