sampler uImage0 : register(s0);

float4 colorTL;
float4 colorTR;
float4 colorBL;
float4 colorBR;

float4 main(float4 sampleColor : COLOR0, float2 uv : TEXCOORD0) : COLOR0
{
    float4 bottom = lerp(colorBL, colorBR, uv.x);
    float4 top = lerp(colorTL, colorTR, uv.x);
    float4 light = lerp(top, bottom, uv.y);
    return tex2D(uImage0, uv) * light;
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