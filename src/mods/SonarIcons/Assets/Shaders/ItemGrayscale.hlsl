sampler uImage0 : register(s0);


float3 naive_grayscale(float3 c)
{
    return ((c.r + c.g + c.b) / 3).xxx;
}

// https://gist.github.com/Volcanoscar/4a9500d240497d3c0228f663593d167a
float3 luminosity_grayscale(float3 c)
{
#define COLOR_FACTOR 0.0

    float gray = 0.21 * c.r + 0.71 * c.g + 0.07 * c.b;
    return float3(c.r * COLOR_FACTOR + gray * (1.0 - COLOR_FACTOR), c.g * COLOR_FACTOR + gray * (1.0 - COLOR_FACTOR), c.b * COLOR_FACTOR + gray * (1.0 - COLOR_FACTOR)) * 1.2;
}

#define GRAYSCALE luminosity_grayscale

float4 main(float2 coords : TEXCOORD0) : COLOR0
{
    float4 c = tex2D(uImage0, coords);
    return float4(GRAYSCALE(c.rgb), c.a);
}

#ifdef FX
technique Technique1
{
    pass HueShader
    {
        PixelShader = compile ps_3_0 main();
    
    
    }
}
#endif // FX
