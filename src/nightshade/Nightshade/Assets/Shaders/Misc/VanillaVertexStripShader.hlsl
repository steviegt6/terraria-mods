#undef TECHNIQUES
#include "VanillaVertexStripShader.effect.uniforms.hlsl"

#include "../pixelation.hlsl"

sampler uImage0 : register(s0);

float4 main(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    coords = normalize_with_pixelation(coords * uSize, uPixel, uSize);

    float4 color = tex2D(uImage0, coords);
    return quantize_color_with_alpha(color, uColorResolution);
}

#define TECHNIQUES
#include "VanillaVertexStripShader.effect.uniforms.hlsl"
