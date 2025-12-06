sampler uImage0 : register(s0);

float4 main(float2 coords : TEXCOORD0) : COLOR0
{
    return float4(tex2D(uImage0, coords).aaaa);
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
