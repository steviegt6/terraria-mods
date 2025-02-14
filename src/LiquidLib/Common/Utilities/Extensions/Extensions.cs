using System;
using System.Reflection;

using Terraria;

namespace Tomat.Terraria.TML.LiquidLib.Common.Utilities.Extensions;

/// <summary>
///     Extensions exposing data.
/// </summary>
public static class SceneMetricsExtensions
{
    private static readonly FieldInfo liquid_counts_field = typeof(SceneMetrics).GetField("_liquidCounts", BindingFlags.NonPublic | BindingFlags.Instance)!;

    public static int[] GetLiquidCounts(this SceneMetrics sceneMetrics)
    {
        return sceneMetrics._liquidCounts;
    }

    public static void ResizeLiquidCounts(this SceneMetrics sceneMetrics, int newSize)
    {
        var newArray = new int[newSize];
        Array.Copy(sceneMetrics._liquidCounts, newArray, Math.Min(sceneMetrics._liquidCounts.Length, newArray.Length));

        liquid_counts_field.SetValue(sceneMetrics, newArray);
    }
}