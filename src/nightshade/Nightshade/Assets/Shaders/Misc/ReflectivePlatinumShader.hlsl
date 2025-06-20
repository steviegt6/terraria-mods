sampler uImage0 : register(s0);

float2 uImageSize0;
float3 uColor;
float3 uLightSource;

float3 color1 = float3(101./255.,132./255.,174./255.) * 2.05;
float3 color2 = float3(246./255.,216./255./1.5,235./255./1.5) * 2.5; // float3(0.75, 1., 0.75);

float get_sat(float3 col)
{
    float max_c = max(col.r, max(col.g, col.b));
    float min_c = min(col.r, min(col.g, col.b));
    float d = max_c - min_c;
    return max_c > 0.0f ? (d / max_c) : 0.0f;
}

float4 ArmorReflectiveColor(float4 v0 /* vertex color input */, float2 t0 /* texture coordinates (uv) */, sampler s0)
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

    float sat = get_sat(tex2D(s0, t0));
    float3 theColor = sat > 0.125f ? color1 : color2;

    // uColor        c2  1
    // uLightSource  c3  1
    // uImage0       s0  1
#define c2 theColor
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

    // #define texld(src0, src1) ArmorMirage(src0, src1.xy, s0)
#define texld(src0, src1) tex2D(s0, src1)

    // texld r0, r0, s0
    r0 = texld(v0, r0.xy);

    // texld r1, r1, s0
    r1 = texld(v0, r1.xy);

    // texld r2, r2, s0
    r2 = texld(v0, r2.xy);

    // texld r3, r3, s0
    r3 = texld(v0, r3.xy);

    // texld r4, t0, s0
    r4 = texld(v0, t0.xy);

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
    r0.yzw = r0.y * c2.bgr;

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
    return ArmorReflectiveColor(color, uv, uImage0);
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
