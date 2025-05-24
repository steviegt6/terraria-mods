#undef TECHNIQUES
#include "BasicPixelizationShader.effect.uniforms.hlsl"

#include "../pixelation.hlsl"

sampler uImage0 : register(s0);

float4 main(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    coords = normalize_with_pixelation(coords * uSize, uPixel, uSize);
    return tex2D(uImage0, coords);
}

#define TECHNIQUES
#include "BasicPixelizationShader.effect.uniforms.hlsl"
