#include "../pixelation.h"

float uPixel;
float uColorResolution;
float2 uSize;

sampler uImage0 : register(s0);

float4 main(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    coords = normalize_with_pixelation(coords * uSize, uPixel, uSize);

    float4 color = tex2D(uImage0, coords);
    return quantize_color_with_alpha(color, uColorResolution);
}

#ifdef FX
technique Technique1
{
    pass StripShader
    {
        PixelShader = compile ps_3_0 main();
    }
}
#endif // FX
