using System;
using System.IO;
using System.Linq;
using System.Text;

using Build.Shared;

using ShaderDecompiler;

namespace Build.Pre.Features.Assets;

internal interface IAssetReference
{
    bool Eligible(ProjectFile file);

    string GenerateCode(ProjectContext ctx, AssetFile asset, string indent);
}

internal sealed class TextureReference : IAssetReference
{
    public bool Eligible(ProjectFile file)
    {
        return file.RelativePath.EndsWith(".png") ||
               file.RelativePath.EndsWith(".rawimg");
    }

    public string GenerateCode(ProjectContext ctx, AssetFile asset, string indent)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"{indent}public const string KEY = \"{ctx.ModName}/{Path.ChangeExtension(asset.Path.Replace('\\', '/'), null)}\";");
        sb.AppendLine();
        sb.AppendLine($"{indent}public static ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D> Asset => lazy.Value;");
        sb.AppendLine();
        sb.AppendLine($"{indent}private static readonly System.Lazy<ReLogic.Content.Asset<Microsoft.Xna.Framework.Graphics.Texture2D>> lazy = new(() => Terraria.ModLoader.ModContent.Request<Microsoft.Xna.Framework.Graphics.Texture2D>(KEY));");

        return sb.ToString().TrimEnd();
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

    public string GenerateCode(ProjectContext ctx, AssetFile asset, string indent)
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

    public string GenerateCode(ProjectContext ctx, AssetFile asset, string indent)
    {
        const string type = "Microsoft.Xna.Framework.Graphics.Effect";

        var sb = new StringBuilder();

        var effect = Effect.ReadXnbOrFxc(asset.File.FullPath, out _);

        sb.AppendLine($"{indent}public sealed class Parameters : IShaderParameters");
        sb.AppendLine($"{indent}{{");

        foreach (var param in effect.Parameters)
        {
            var uniformType = GetUniformType(param.Value.Type.ToString());
            sb.AppendLine($"{indent}    public {uniformType} {param.Value.Name} {{ get; set; }}");
            sb.AppendLine();
        }

        sb.AppendLine($"{indent}    public void Apply(Microsoft.Xna.Framework.Graphics.EffectParameterCollection parameters)");
        sb.AppendLine($"{indent}    {{");
        foreach (var param in effect.Parameters)
        {
            if (param.Value.Name == "uTime")
            {
                sb.AppendLine($"{indent}        parameters[\"{param.Value.Name}\"]?.SetValue(Terraria.Main.GlobalTimeWrappedHourly);");
                continue;
            }

            sb.AppendLine($"{indent}        parameters[\"{param.Value.Name}\"]?.SetValue({param.Value.Name});");
        }
        sb.AppendLine($"{indent}    }}");

        sb.AppendLine($"{indent}}}");
        sb.AppendLine();
        sb.AppendLine($"{indent}public const string KEY = \"{ctx.ModName}/{Path.ChangeExtension(asset.Path.Replace('\\', '/'), null)}\";");
        sb.AppendLine();
        sb.AppendLine($"{indent}public static ReLogic.Content.Asset<{type}> Asset => lazy.Value;");
        sb.AppendLine();
        sb.AppendLine($"{indent}private static readonly System.Lazy<ReLogic.Content.Asset<{type}>> lazy = new(() => Terraria.ModLoader.ModContent.Request<{type}>(KEY));");
        sb.AppendLine();

        foreach (var passes in effect.Techniques.SelectMany(x => x.Passes))
        {
            sb.AppendLine($"{indent}public static WrapperShaderData<Parameters> Create{passes.Name}()");
            sb.AppendLine($"{indent}{{");
            sb.AppendLine($"{indent}    return new WrapperShaderData<Parameters>(Asset, \"{passes.Name}\");");
            sb.AppendLine($"{indent}}}");
        }

        return sb.ToString().TrimEnd();
    }

    private static string GetUniformType(string uniformType)
    {
        return uniformType switch
        {
            "float" => "float",
            "float2" => "Microsoft.Xna.Framework.Vector2",
            "float3" => "Microsoft.Xna.Framework.Vector3",
            "float4" => "Microsoft.Xna.Framework.Vector4",
            "matrix" => "Microsoft.Xna.Framework.Matrix",
            "sampler" => "Microsoft.Xna.Framework.Graphics.Texture2D?",
            _ => throw new InvalidOperationException("Unsupported uniform type: " + uniformType),
        };
    }
}