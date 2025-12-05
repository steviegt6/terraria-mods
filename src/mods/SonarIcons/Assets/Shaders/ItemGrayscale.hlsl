sampler uImage0 : register(s0);

// TODO: We can use a less naive grayscale approach in the future.

float4 main(float2 coords : TEXCOORD0) : COLOR0
{
    float4 c = tex2D(uImage0, coords);
    float avg = (c.r + c.g + c.b) / 3;
    return float4(avg.xxx, c.a);
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