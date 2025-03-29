using System;

namespace Tomat.Terraria.TML.SourceGenerator.Generators.DataDriven;

public interface IAssetReference
{
    string QualifiedType { get; }

    string Extension { get; }
}

public sealed class Texture2DAssetReference(string extension) : IAssetReference
{
    public string QualifiedType => "global::Microsoft.Xna.Framework.Graphics.Texture2D";

    public string Extension { get; } = extension;
}

// Special case with special handling.
public sealed class EffectAssetReference : IAssetReference
{
    public string QualifiedType => throw new InvalidOperationException();
    
    public string Extension => throw new InvalidOperationException();
}