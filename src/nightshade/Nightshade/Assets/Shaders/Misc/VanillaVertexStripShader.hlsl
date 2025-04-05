#undef TECHNIQUES
#include "VanillaVertexStripShader.effect.uniforms.hlsl"

#include "../pixelation.hlsl"

float4 main(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    return float4(0,0,0,0);
    return float4(1,0,0,1);
    
    coords = pixelate(coords, uPixel);

    // TODO: Quantize.
    float4 color = tex2D(uImage0, coords);
    float3 quantized_color = quantize_color(color, uColorResolution);
    return float4(quantized_color, color.a) * sampleColor;
}

#define TECHNIQUES
#include "VanillaVertexStripShader.effect.uniforms.hlsl"
