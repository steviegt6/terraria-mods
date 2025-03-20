using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using ReLogic.Threading;

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

        if (cts is null)
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

        try
        {
            modHandle = mod.modFile.Open();

            DisplayText = "Extracting files...";
            {
                var totalExtracted = 0;

                ForWithoutDeadlockCheck(
                    0,
                    mod.modFile.Count,
                    (fromFile, toFile, _) =>
                    {
                        for (var i = fromFile; i < toFile; i++)
                        {
                            cts.Token.ThrowIfCancellationRequested();

                            var entry = mod.modFile.fileTable[i];

                            var entryName = entry.Name;
                            ContentConverters.Reverse(ref entryName, out var converter);

                            Progress = totalExtracted / (float)mod.modFile.Count;

                            var entryPath = Path.Combine(dir, entryName);
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(entryPath)!);
                            }

                            using var dest = File.OpenWrite(entryPath);
                            using var src  = mod.modFile.GetStream(entry, true);

                            if (converter is not null)
                            {
                                converter(src, dest);
                            }
                            else
                            {
                                src.CopyTo(dest);
                            }

                            Interlocked.Increment(ref totalExtracted);
                        }
                    }
                );

                var modAssembliesDir = Path.Combine(ModCompile.ModSourcePath, "ModAssemblies");
                {
                    Directory.CreateDirectory(dir);
                }

                var dllName = $"{mod.Name}.dll";
                var pdbName = $"{mod.Name}.pdb";
                var xmlName = $"{mod.Name}.xml";

                var dllPath = Path.Combine(dir, dllName);
                var pdbPath = Path.Combine(dir, pdbName);
                var xmlPath = Path.Combine(dir, xmlName);

                var dllReferencesPath = Path.Combine(modAssembliesDir, $"{mod.Name}_v{mod.modFile.Version}.dll");
                var pdbReferencesPath = Path.Combine(modAssembliesDir, $"{mod.Name}_v{mod.modFile.Version}.pdb");
                var xmlReferencesPath = Path.Combine(modAssembliesDir, $"{mod.Name}_v{mod.modFile.Version}.xml");

                if (File.Exists(dllPath))
                {
                    File.Move(dllPath, dllReferencesPath, true);
                }
                else
                {
                    throw new FileNotFoundException("Could not find the mod's DLL file.");
                }

                if (File.Exists(pdbPath))
                {
                    File.Move(pdbPath, pdbReferencesPath, true);
                }
                else
                {
                    pdbReferencesPath = null;
                }

                if (File.Exists(xmlPath))
                {
                    File.Move(xmlPath, xmlReferencesPath, true);
                }
                else
                {
                    xmlReferencesPath = null;
                }

                var decompiledMod = new DecompiledMod(
                    dllReferencesPath,
                    pdbReferencesPath,
                    xmlReferencesPath
                );

                // We are going to be relatively good-faith here and assume
                // that, if includeSource is false, the mod does not need
                // decompiling because its source files will remain intact.
                if (!mod.properties.includeSource)
                {
                    ProjectDecompiler.Decompile(mod, dir, decompiledMod, cts.Token);
                }

                ProjectDecompiler.WriteCsproj(mod, dir);
            }
        }
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

    private static void ForWithoutDeadlockCheck(
        int               fromInclusive,
        int               toExclusive,
        ParallelForAction callback,
        object?           context = null
    )
    {
        var count = toExclusive - fromInclusive;
        if (count == 0)
        {
            return;
        }

        var threadCount = Math.Min(Math.Max(1, Environment.ProcessorCount + 1 - 1 - 1), count);

        if (FastParallel.ForceTasksOnCallingThread)
        {
            threadCount = 1;
        }

        var num3 = count / threadCount;
        var num4 = count % threadCount;
        var num5 = toExclusive;

        var countdownEvent = new CountdownEvent(threadCount);
        for (var num6 = threadCount - 1; num6 >= 0; num6--)
        {
            var num7 = num3;
            if (num6 < num4)
            {
                num7++;
            }

            num5 -= num7;
            var num8         = num5;
            var toExclusive2 = num8 + num7;

            var rangeTask = new FastParallel.RangeTask(callback, num8, toExclusive2, context, countdownEvent);
            if (num6 < 1)
            {
                FastParallel.InvokeTask(rangeTask);
            }
            else
            {
                ThreadPool.QueueUserWorkItem(FastParallel.InvokeTask, rangeTask);
            }
        }

        countdownEvent.Wait();
    }
}