sampler uImage0 : register(s0);

float2 uImageSize0;
float3 uColor;
float3 uLightSource;

float4 uSourceRect;
float uTime;

float4 ArmorMirage(float4 v0 /* vertex color input */, float2 t0 /* texture coordinates (uv) */, sampler s0)
{
    // preshader
    float4 r0;
    float4 c2;
    float c0 = uTime;
    float4 c1 = uSourceRect;
    c2.xy = uImageSize0;

    // rcp r0.x, c2.x
    r0.x = 1.0f / c2.x;

    // add c2.x, r0.x, r0.x
    c2.x = r0.x + r0.x;

    // rcp c0.x, c1.w
    c0 = 1.0f / c1.w;

    // mul c1.x, c0.x, (5)
    c1.x = uTime * 5.0f;

    r0 = float4(0.0f, 0.0f, 0.0f, 0.0f);

    // ps_2_0
    float4 c3 = uSourceRect;
    float4 c4;
    c4.xy = uImageSize0;

    float4 r1;
    float4 r2;

    // def c5, -0.5, 30, 0.159154937, 0.5
    float4 c5 = float4(-0.5f, 30.0f, 0.159154937f, 0.5f);

    // def c6, 6.28318548, -3.14159274, 0, 1
    float4 c6 = float4(6.28318548f, -3.14159274f, 0.0f, 1.0f);

    // def c7, -1.55009923e-006, -2.17013894e-005, 0.00260416674, 0.00026041668
    float4 c7 = float4(-1.55009923e-006f, -2.17013894e-005f, 0.00260416674f, 0.00026041668f);

    // def c8, -0.020833334, -0.125, 1, 0.5
    float4 c8 = float4(-0.020833334f, -0.125f, 1.0f, 0.5f);

    // dcl v0
    // #define v0 color

    // dcl t0.xy
    // #define t0 uv

    // dcl_2d s0
    // #define s0 uImage0

    // mov r0.w, c4.y
    r0.w = c4.y;

    // mad r0.x, t0.y, r0.w, -c3.y
    r0.x = t0.y * r0.w - c3.y;

    // mov r1.xy, c5
    r1.xy = c5.xy;

    // mad r0.x, r0.x, c0.x, r1.x
    r0.x = r0.x * c0.x + r1.x;

    // mad r0.x, r0.x, r1.y, c1.x
    r0.x = r0.x * r1.y + c1.x;

    // mad r0.x, r0.x, c5.z, c5.w
    r0.x = r0.x * c5.z + c5.w;

#define frc(src) (src - floor(src))
    // frc r0.x, r0.x
    r0.x = frc(r0.x);

    // mad r0.x, r0.x, c6.x, c6.y
    r0.x = r0.x * c6.x + c6.y;

    // sincos r1.y, r0.x, c7, c8
    r1.y = sin(r0.x);

    // mul r0.x, r1.y, c2.x
    r0.x = r1.y * c2.x;

    // mov r1.x, -r0.x
    r1.x = -r0.x;

    // mov r1.y, c6.z
    r1.y = c6.z;

    // add r1.xy, r1, t0
    r1.xy = r1.xy + t0.xy;

    // mov r0.y, c6.z
    r0.y = c6.z;

    // add r0.xy, r0, t0
    r0.xy = r0.xy + t0.xy;

    // texld r1, r1, s0
    r1 = tex2D(s0, r1.xy);

    // texld r0, r0, s0
    r0 = tex2D(s0, r0.xy);

    // texld r2, t0, s0
    r2 = tex2D(s0, t0.xy);

    // mov r0.x, r1.x
    r0.x = r1.x;

    // mov r0.w, c6.w
    r0.w = c6.w;

    // mov r0.y, r2.y
    r0.y = r2.y;

    // mul r0, r2.w, r0
    r0 = r2.w * r0;

    // mul r0, r0, v0
    r0 = r0 * v0;

    // mov oC0, r0
    return r0;
}

float4 ArmorReflectiveColor(float4 v0 /* vertex color input */, float2 t0 /* texture coordinates (uv) */)
{
    // preshader

    // uImageSize0  c0  1
    // #define c0 uImageSize0
    // float2 c0 = uImageSize0;
    float4 c0;
    float4 c1;
    float4 r0;
    float4 r1;

    c0.xy = uImageSize0;

    // rcp r0.x, c0.x
    r0.x = 1.0f / c0.x;

    // rcp r0.y, c0.y
    r0.y = 1.0f / c0.y;

    // add r1.xy, r0.xy, r0.xy
    r1.xy = r0.xy + r0.xy;

    // mov c0.x, r1.y
    c0.x = r1.y;

    // mov c1.x, r1.x
    c1.x = r1.x;

    // clean up since they are different register namespaces
    r0 = float4(0.0f, 0.0f, 0.0f, 0.0f);
    r1 = float4(0.0f, 0.0f, 0.0f, 0.0f);

    // ps_2_0

    // uColor        c2  1
    // uLightSource  c3  1
    // uImage0       s0  1
#define c2 uColor
#define c3 uLightSource
    float4 r2;
    float4 r3;
    float4 r4;

    // def c4, 0, 0.333333343, 1, -0.300000012
    float4 c4 = float4(0.0f, 0.333333343f, 1.0f, -0.300000012f);

    // def c5, 1.53846157, -0.461538464, -2, 3
    float4 c5 = float4(1.53846157f, -0.461538464f, -2.0f, 3.0f);

    // def c6, 0.5, 0, 0, 0
    float4 c6 = float4(0.5f, 0.0f, 0.0f, 0.0f);

    // dcl v0
    // #define v0 color

    // dcl t0.xy
    // #define t0 uv

    // dcl_2d s0
#define s0 uImage0

    // mov r0.x, -c1.x
    r0.x = -c1.x;

    // mov r0.y, c4.x
    r0.y = c4.x;

    // add r0.xy, r0, t0
    r0.xy = r0.xy + t0.xy;

    // add r1.x, t0.x, c1.x
    r1.x = t0.x + c1.x;

    // mov r1.y, t0.y
    r1.y = t0.y;

    // mov r2.x, t0.x
    r2.x = t0.x;

    // add r2.y, t0.y, c0.x
    r2.y = t0.y + c0.x;

    // mov r3.x, t0.x
    r3.x = t0.x;

    // add r3.y, t0.y, -c0.x
    r3.y = t0.y - c0.x;

    // texld r0, r0, s0
    r0 = ArmorMirage(v0, r0.xy, s0);

    // texld r1, r1, s0
    r1 = ArmorMirage(v0, r1.xy, s0);

    // texld r2, r2, s0
    r2 = ArmorMirage(v0, r2.xy, s0);

    // texld r3, r3, s0
    r3 = ArmorMirage(v0, r3.xy, s0);

    // texld r4, t0, s0
    r4 = ArmorMirage(v0, t0.xy, s0);

    // add r0.xyz, r0, -r1
    r0.xyz = r0.xyz - r1.xyz;

    // add r2.w, r0.y, r0.x
    r2.w = r0.y + r0.x;

    // add r2.w, r0.z, r2.w
    r2.w = r0.z + r2.w;

    // mul r0.x, r2.w, c4.y
    r0.x = r2.w * c4.y;

    // add r1.xyz, -r2, r3
    r1.xyz = -r2.xyz + r3.xyz;

    // add r0.w, r1.y, r1.x
    r0.w = r1.y + r1.x;

    // add r0.w, r1.z, r0.w
    r0.w = r1.z + r0.w;

    // mul r0.y, r0.w, c4.y
    r0.y = r0.w * c4.y;

    // mul r0.w, r0.y, r0.y
    r0.w = r0.y * r0.y;

    // mad r0.w, r0.x, r0.x, r0.w
    r0.w = r0.x * r0.x + r0.w;

    // add r0.w, -r0.w, c4.z
    r0.w = -r0.w + c4.z;

    // rsq r1.x, r0.w
    r1.x = 1.0f / sqrt(r0.w);

    // rcp r1.x, r1.x
    r1.x = 1.0f / r1.x;

    // cmp r0.z, r0.w, r1.x, c4.x
    r0.z = (r0.w > 0) ? r1.x : c4.x;

#define dp3(src0, src1) (src0.x * src1.x) + (src0.y * src1.y) + (src0.z * src1.z)
    // dp3 r0.x, r0, c3
    r0.x = dp3(r0, c3);

    // add r0.y, r0.x, c4.w
    r0.y = r0.x + c4.w;

    // mul r0.y, r0.y, c5.x
    r0.y = r0.y * c5.x;

    // cmp_sat r0.x, r0.x, r0.y, c5.y
    r0.x = saturate((r0.x > 0) ? r0.y : c5.y);

    // mad r0.y, r0.x, c5.z, c5.w
    r0.y = r0.x * c5.z + c5.w;

    // mul r0.x, r0.x, r0.x
    r0.x = r0.x * r0.x;

    // mul r0.x, r0.y, r0.x
    r0.x = r0.y * r0.x;

    // add r0.x, r0.x, r0.x
    r0.x = r0.x + r0.x;

    // mul r0.x, r0.x, r0.x
    r0.x = r0.x * r0.x;

    // mul r0.x, r4.w, r0.x
    r0.x = r4.w * r0.x;

    // add r0.y, r4.y, r4.x
    r0.y = r4.y + r4.x;

    // add r0.y, r4.z, r0.y
    r0.y = r4.z + r0.y;

    // mul r0.y, r0.y, c4.y
    r0.y = r0.y * c4.y;

    // mul r0.yzw, r0.y, c2.wzyx
    r0.yzw = r0.y * float4(c2, 1.0f).wzyx;

    // mul r1.xyz, r0.wzyx, c6.x
    r1.xyz = r0.wzyx * c6.x;

    // mad r4.xyz, r0.x, r0.wzyx, r1
    r4.xyz = r0.x * r0.wzyx + r1.xyz;

    // mul r0, r4, v0
    r0 = r4 * v0;

    // mov oC0, r0
    return r0;
}

float4 main(float4 color : COLOR0, float2 uv : TEXCOORD0) : COLOR0
{
    return ArmorReflectiveColor(color, uv);
}

#ifdef FX
technique Technique1
{
    pass ArmorReflectiveColor
    {
        PixelShader = compile ps_3_0 main();
    }
}
#endif // FX
