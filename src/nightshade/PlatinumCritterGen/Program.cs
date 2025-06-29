﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PlatinumCritterGen;

internal static class Program
{
    private readonly record struct NameAndKind(string Name, string Kind, string Path);

    private const string input_dir = "gold";
    private const string output_dir = "platinum";

    private const string critters_dir = "critters";
    private const string cages_dir = "cages";

    private static readonly Dictionary<Rgba32, Rgba32> critter_color_map = new()
    {
        // basic mapping of gold bar/critter colors
        [new Rgba32(83, 62, 11)] = new Rgba32(55, 62, 89),
        [new Rgba32(102, 77, 14)] = new Rgba32(85, 86, 128),
        [new Rgba32(121, 92, 18)] = new Rgba32(101, 110, 136),
        [new Rgba32(147, 125, 30)] = new Rgba32(133, 152, 179),
        [new Rgba32(203, 179, 73)] = new Rgba32(182, 188, 213),
        [new Rgba32(255, 249, 181)] = new Rgba32(242, 223, 236),

        // weird case for frogs only
        [new Rgba32(129, 92, 10)] = new Rgba32(101, 113, 145),
    };

    private static readonly Dictionary<Rgba32, Rgba32> cage_color_map = new()
    {
        // basic mapping of gold bar/critter colors
        [new Rgba32(83, 62, 11)] = new Rgba32(55, 62, 89),
        [new Rgba32(102, 77, 14)] = new Rgba32(85, 86, 128),
        [new Rgba32(121, 92, 18)] = new Rgba32(101, 110, 136),
        [new Rgba32(147, 125, 30)] = new Rgba32(133, 152, 179),
        [new Rgba32(203, 179, 73)] = new Rgba32(182, 188, 213),
        [new Rgba32(255, 249, 181)] = new Rgba32(242, 223, 236),

        // additional shade present as cage outline
        [new Rgba32(29, 20, 0)] = new Rgba32(14, 16, 38),

        // tinted shades that mirror the main shades
        [new Rgba32(84, 84, 47)] = new Rgba32(55, 62, 89),
        [new Rgba32(99, 96, 49)] = new Rgba32(85, 86, 128),
        [new Rgba32(114, 108, 52)] = new Rgba32(101, 110, 136),
        [new Rgba32(135, 135, 62)] = new Rgba32(133, 152, 179),
        [new Rgba32(180, 178, 96)] = new Rgba32(182, 188, 213),
        [new Rgba32(221, 234, 183)] = new Rgba32(242, 223, 236),

        // exceptions for grasshopper cage tile
        [new Rgba32(218, 155, 16)] = new Rgba32(182, 188, 213),
        [new Rgba32(231, 196, 121)] = new Rgba32(242, 223, 236),
    };

    public static void Main()
    {
        var dir = Directory.GetCurrentDirectory();

        ProcessCritters(dir);
        ProcessCages(dir);
    }

    private static void ProcessCritters(string dir)
    {
        var inputPath = Path.Combine(dir, input_dir, critters_dir);
        var outputPath = Path.Combine(dir, output_dir);

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        var images = GetNamesFromDirectory(inputPath).ToArray();

        foreach (var image in images)
        {
            ProcessImage(
                image.Path,
                Path.Combine(outputPath, GetDirectoryFromKind(image.Kind), $"{image.Name}.png"),
                BasicPaletteSwap(critter_color_map)
            );
        }
    }

    private static void ProcessCages(string dir)
    {
        var inputPath = Path.Combine(dir, input_dir, cages_dir);
        var outputPath = Path.Combine(dir, output_dir, "Cages");

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        var images = GetNamesFromDirectory(inputPath).ToArray();

        foreach (var image in images)
        {
            ProcessImage(
                image.Path,
                Path.Combine(outputPath, GetDirectoryFromKind(image.Kind), $"{image.Name}.png"),
                BasicPaletteSwap(cage_color_map)
            );
        }
    }

    private static IEnumerable<NameAndKind> GetNamesFromDirectory(string dir)
    {
        foreach (var file in Directory.GetFiles(dir, "*.png", SearchOption.TopDirectoryOnly))
        {
            var parts = Path.GetFileNameWithoutExtension(file).Split('_');

            var kind = parts.Last();
            var name = PascalCase(parts[..^1].Select(p => p.ToLowerInvariant()));

            yield return new NameAndKind(name, kind, file);
        }
    }

    private static string PascalCase(IEnumerable<string> inputs)
    {
        var sb = new StringBuilder();

        foreach (var input in inputs)
        {
            var firstChar = true;
            foreach (var c in input)
            {
                sb.Append(firstChar ? char.ToUpperInvariant(c) : c);
                firstChar = false;
            }
        }

        return sb.ToString();
    }

    private static string GetDirectoryFromKind(string kind)
    {
        return kind switch
        {
            "item" => "Items",
            "npc" => "NPCs",
            "tile" => "Tiles",
            "armor" => "Armor",
            _ => throw new InvalidOperationException(),
        };
    }

    private static void ProcessImage(string inputPath, string outputPath, Action<Image<Rgba32>> imageFunc)
    {
        if (Path.GetDirectoryName(outputPath) is { } dirName)
        {
            Directory.CreateDirectory(dirName);
        }

        using var image = Image.Load<Rgba32>(inputPath);
        imageFunc(image);
        image.Save(outputPath);
    }

    private static Action<Image<Rgba32>> BasicPaletteSwap(Dictionary<Rgba32, Rgba32> paletteSwap)
    {
        return image =>
        {
            for (var y = 0; y < image.Height; y++)
            for (var x = 0; x < image.Width; x++)
            {
                var pixel = image[x, y];
                if (paletteSwap.TryGetValue(pixel, out var newColor))
                {
                    image[x, y] = newColor;
                }
            }
        };
    }
}