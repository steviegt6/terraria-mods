#undef TECHNIQUES
#include "GaussianBloom.effect.uniforms.hlsl"

#include "../blurs.hlsl"

sampler uImage0 : register(s0);

float4 main(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    return gauss_bloom(uImage0, coords, AspectCorrectedGBlurScale(uSize, 1.0f), 32, 1., 0.3f);
}

#define TECHNIQUES
#include "GaussianBloom.effect.uniforms.hlsl"
