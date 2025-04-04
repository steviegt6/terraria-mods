using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using log4net;

using Nightshade.Common.Features.ItemVariants;

using Terraria.ID;
using Terraria.ModLoader;

// Configure where to look for our variants.
[assembly: ItemVariantsPath("Assets/Images/Items/Variants/")]

namespace Nightshade.Common.Features.ItemVariants;

[Autoload(Side = ModSide.Client)]
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class ItemVariantLoader : ModSystem
{
    private readonly record struct ItemVariant(int? ItemType, int? NpcType);

    private readonly record struct ItemVariants(
        List<string>                  Variants,
        Dictionary<int, List<string>> NpcVariants
    );

    /// <summary>
    ///     Known item variants loaded at runtime.
    /// </summary>
    private static FrozenDictionary<int, ItemVariantRecord>? item_variants;

    public override void Load()
    {
        base.Load();

        var collectedVariants = new Dictionary<int, ItemVariants>();

        foreach (var mod in ModLoader.Mods)
        {
            var modVariants = LoadVariantsFromMod(mod, Mod.Logger);
            if (modVariants.Keys.Count == 0)
            {
                continue;
            }

            // It's annoying we have to do this but let's merge everything into
            // one dictionary.  We also have to do this to make full paths out
            // of relative mod ones.  Yawn!
            foreach (var (npcId, varEntry) in modVariants)
            {
                if (!collectedVariants.TryGetValue(npcId, out var entry))
                {
                    collectedVariants[npcId] = entry = new ItemVariants([], []);
                }

                foreach (var variant in varEntry.Variants)
                {
                    entry.Variants.Add(GetQualifiedVariant(variant, mod));
                }

                foreach (var npcVariant in varEntry.NpcVariants)
                {
                    if (!entry.NpcVariants.TryGetValue(npcVariant.Key, out var npcEntry))
                    {
                        entry.NpcVariants[npcVariant.Key] = npcEntry = [];
                    }

                    foreach (var variant in npcVariant.Value)
                    {
                        npcEntry.Add(GetQualifiedVariant(variant, mod));
                    }
                }
            }
        }

        var compiledVariants = collectedVariants.ToDictionary(
            x => x.Key,
            x => new ItemVariantRecord(
                x.Value.Variants.ToArray(),
                x.Value.NpcVariants.ToDictionary(
                    y => y.Key,
                    y => y.Value.ToArray()
                )
            )
        );

        item_variants = compiledVariants.ToFrozenDictionary();

        return;

        static string GetQualifiedVariant(string path, Mod mod)
        {
            return mod.Name + '/' + Path.ChangeExtension(path, null);
        }
    }

    private static Dictionary<int, ItemVariants> LoadVariantsFromMod(Mod mod, ILog logger)
    {
        logger.Debug("Loading item variants from mod: " + mod.Name);

        if (mod.File is null)
        {
            logger.Debug("    Mod does not have an associated file, is this ModLoaderMod?  Skipping.");
            return [];
        }

        var code = mod.Code;
        if (code?.GetCustomAttribute<ItemVariantsPathAttribute>() is not { } pathAttribute)
        {
            logger.Debug("    No ItemVariantsPathAttribute found, skipping.");
            return [];
        }

        var path = pathAttribute.Path;
        logger.Debug("    Using path: " + path);

        var candidateFiles = mod.GetFileNames().Where(x => x.StartsWith(path, StringComparison.InvariantCultureIgnoreCase)).ToArray();
        if (candidateFiles.Length == 0)
        {
            logger.Debug("    No files found in path, skipping.");
            return [];
        }

        logger.Debug($"    Found {candidateFiles.Length} possible item variant(s).");

        var variants = new Dictionary<int, ItemVariants>();
        foreach (var candidate in candidateFiles)
        {
            var name    = Path.GetFileNameWithoutExtension(candidate);
            var variant = GetVariantFromFileName(name);

            if (!variant.ItemType.HasValue)
            {
                logger.Warn($"    Skipping variant candidate: {candidate}");
                continue;
            }

            if (!variants.TryGetValue(variant.ItemType.Value, out var entry))
            {
                variants[variant.ItemType.Value] = entry = new ItemVariants([], []);
            }

            if (variant.NpcType.HasValue)
            {
                if (!entry.NpcVariants.TryGetValue(variant.NpcType.Value, out var npcEntry))
                {
                    entry.NpcVariants[variant.NpcType.Value] = npcEntry = [];
                }

                npcEntry.Add(candidate);
            }
            else
            {
                entry.Variants.Add(candidate);
            }
        }

        return variants;
    }

    private static ItemVariant GetVariantFromFileName(string fileName)
    {
        var nameParts = fileName.Split('_');
        if (nameParts.Length == 0)
        {
            // how?
            return new ItemVariant(null, null);
        }

        if (!ItemID.Search.TryGetId(nameParts[0], out var itemId))
        {
            return new ItemVariant(null, null);
        }

        if (nameParts.Length > 1 && NPCID.Search.TryGetId(nameParts[1], out var npcId))
        {
            return new ItemVariant(itemId, npcId);
        }

        // Ignore any remaining parts (necessary for duplicate entries) or
        // invalid NPC IDs.
        // TODO: Only permit numbers following names to allow duplicates but
        //       still enforce names.
        return new ItemVariant(itemId, null);
    }
}