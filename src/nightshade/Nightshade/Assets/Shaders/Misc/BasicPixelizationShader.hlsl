#include "../pixelation.h"

float uPixel;
float2 uSize;

sampler uImage0 : register(s0);

float4 main(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    coords = normalize_with_pixelation(coords * uSize, uPixel, uSize);
    return tex2D(uImage0, coords);
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
