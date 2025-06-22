#include "../pixelation.h"

sampler uImage0 : register(s0);

texture2D uSecondaryTexture;
float uProgress; // not named uTime to not be automatically set
float uWaviness;
float uScale;
float uIntensity;
float uColorRes;
float4 uColor;

sampler uImage1 = sampler_state
{
    Texture = <uSecondaryTexture>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

float4 Main(float2 fragCoord : TEXCOORD) : COLOR
{
    float _gradient = smoothstep(1, 0, fragCoord.y);
    float waviness = sin(fragCoord + uProgress) * (1 - fragCoord.y) * 0.05 * uWaviness;
    
    float4 c = tex2D(uImage1, uScale * (fragCoord + float2(0, uProgress * 0.9) + float2(waviness, 0)));
    float4 c2 = tex2D(uImage1, uScale * (-fragCoord + float2(1, 1)) + float2(0.5, 0.17) - float2(uProgress * 0.31, uProgress * 1.17) - float2(waviness * 0.91, 0));
    float4 mask = tex2D(uImage0, fragCoord + float2(waviness, 0));
    
    float maskA = max(mask.r, max(mask.g, mask.b)) * mask.a * 4;
    
    float a = maskA * smoothstep(0, 1, max(c.r, c2.r)) * uIntensity * _gradient;
    return quantize_color_with_alpha(c * c2, uColorRes) * a * uColor;
}

#ifdef FX
Technique techique1
{
    pass StripShader
    {
        PixelShader = compile ps_3_0 Main();
    }
}
#endif // FX