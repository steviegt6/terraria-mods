using System;
using System.IO;
using System.Text;

using Build.Shared;

namespace Build.Pre.Features.Assets;

internal interface IAssetReference
{
    bool Eligible(ProjectFile file);

    string GenerateCode(AssetFile asset, string indent);
}

internal sealed class TextureReference : IAssetReference
{
    public bool Eligible(ProjectFile file)
    {
        return file.RelativePath.EndsWith(".png") ||
               file.RelativePath.EndsWith(".rawimg");
    }

    public string GenerateCode(AssetFile asset, string indent)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"{indent}public const string KEY = \"{Path.ChangeExtension(asset.Path.Replace('\\', '/'), null)}\";");
        sb.AppendLine();
        sb.AppendLine($"{indent}public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;");
        sb.AppendLine();
        sb.AppendLine($"{indent}private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<global::Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));");

        return sb.ToString();
    }
}

internal sealed class SoundReference : IAssetReference
{
    public bool Eligible(ProjectFile file)
    {
        return file.RelativePath.EndsWith(".wav") ||
               file.RelativePath.EndsWith(".ogg") ||
               file.RelativePath.EndsWith(".mp3");
    }

    public string GenerateCode(AssetFile asset, string indent)
    {
        return $"{indent}// TODO: {asset.Name}";
    }
}

internal sealed class EffectReference : IAssetReference
{
    public bool Eligible(ProjectFile file)
    {
        return file.RelativePath.EndsWith(".fxc")
            || file.RelativePath.EndsWith(".xnb");
    }

    public string GenerateCode(AssetFile asset, string indent)
    {
        return $"{indent}// TODO: {asset.Name}";
    }

    private static string GetUniformType(string uniformType)
    {
        return uniformType switch
        {
            "float4" => "global::Microsoft.Xna.Framework.Vector4",
            "float3" => "global::Microsoft.Xna.Framework.Vector3",
            "float2" => "global::Microsoft.Xna.Framework.Vector2",
            "float" => "float",
            "matrix" => "global::Microsoft.Xna.Framework.Matrix",
            _ => throw new InvalidOperationException("Unsupported uniform type: " + uniformType),
        };
    }
}