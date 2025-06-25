sampler uImage0 : register(s0);

texture uTexture0;
sampler tex0 = sampler_state
{
    texture = <uTexture0>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};

float uTime;
float2 uScreenPosition;
float2 uScreenSize;
float2 uGasCenter;
float2 uGasVelocity;
float uLoss;
float uPropDistance;

static const float grain = 12;

float4 main(float2 uv : TEXCOORD0, float4 baseColor : COLOR0) : COLOR0
{
    float4 distortDistorter = tex2D(tex0, (uv * float2(uScreenSize.x / uScreenSize.y, 1) + uScreenPosition / uScreenSize.y) * 3 + float2(0, frac(uTime)));
    float4 distortImage = tex2D(tex0, (uv * float2(uScreenSize.x / uScreenSize.y, 1) + uScreenPosition / uScreenSize.y) * 5 + float2(0, -frac(uTime)) + (length(distortDistorter.rgb)) * 0.1);
      
    float4 original = tex2D(uImage0, uv);

    float2 dd = float2(ddx(length(original.rgb) / 2.0), ddy(length(original.rgb / 2.0)));
    float propogation = (pow(length(original) / 4.0, 0.25) * 5 + 1) * uPropDistance;
    float2 distortDirection = uGasVelocity + normalize(uGasCenter - uv) + dd * grain;
    
    float4 image = tex2D(uImage0, uv + (length(distortImage.rgb) / 2 + 0.01) / uScreenSize.y * propogation * distortDirection);
    if (length(image) < 0.03333)
        return 0;
    
    return round(image * uLoss * 1024) / 1024;
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
