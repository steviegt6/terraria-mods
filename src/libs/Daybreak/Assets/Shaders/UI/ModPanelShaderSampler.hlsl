#undef TECHNIQUES
#include "ModPanelShaderSampler.effect.uniforms.hlsl"
#include "../pixelation.hlsl"

float4 main(float2 coords : SV_POSITION, float2 tex_coords : TEXCOORD0) : COLOR0
{
    float2 resolution = uSource.xy;
    float2 position = uSource.zw;

    coords -= position;

    // Normalize the coordinates but with optional pixelation.
    float2 uv = normalize_with_pixelation(coords, 2.0f, resolution);

    // uImage1 is at half the resolution of uImage0 (resolution), sample it such
    // that we get a free pixelation effect (2x)
    float3 finalCol = tex2D(uImage1, uv).xyz;

    // Take the original texture into account.  This is because we draw to a UI
    // panel which will have corners and stuff of that nature, so we don't want
    // to draw over that.  We don't reference the original texture for the
    // shader's effects, but we'll take alpha into account.
    float alpha = tex2D(uImage0, tex_coords).a;
    return float4(finalCol, alpha);
}

#define TECHNIQUES
#include "ModPanelShader.effect.uniforms.hlsl"
