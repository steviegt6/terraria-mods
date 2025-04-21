#undef TECHNIQUES
#include "TungstenArmorMoltenShader.effect.uniforms.hlsl"

#include "../pixelation.hlsl"
#include "../glsl.hlsl"

sampler uImage0 : register(s0);

float mod(float x, float y)
{
    return x - y * floor(x / y);
}

float2 mod(float2 x, float2 y)
{
    return x - y * floor(x / y);
}

float3 mod(float3 x, float3 y)
{
    return x - y * floor(x / y);
}

float4 mod(float4 x, float4 y)
{
    return x - y * floor(x / y);
}

float4 permute(float4 x)
{
    return mod(((x * 34.0f) + 1.0f.xxxx) * x, 289.0f.xxxx);
}

float4 taylorInvSqrt(float4 r)
{
    return 1.792842864990234375f.xxxx - (r * 0.8537347316741943359375f);
}

float3 hash33(float3 p3)
{
	float3 p = frac(p3 * float3(.1031,.11369,.13787));
    p += dot(p, p.yxz+19.19);
    return -1.0 + 2.0 * frac(float3((p.x + p.y)*p.z, (p.x+p.z)*p.y, (p.y+p.z)*p.x));
}

float worley(float3 p, float scale){

    float3 id = floor(p);
    float3 fd = frac(p);

    float n = 0.;

    float minimalDist = 1.;


    for(float x = -2.; x <=2.; x++){
        for(float y = -2.; y <=2.; y++){
            for(float z = -2.; z <=2.; z++){

                float3 coord = float3(x,y,z);
                float3 rId = hash33(mod(abs(id+coord),scale))*0.33;

                float3 r = coord + rId - fd; 

                float d = dot(r,r);

                if(d < minimalDist){
                    minimalDist = d;
                }

            }//z
        }//y
    }//x

  return minimalDist;
}

float fbm(float3 p,float scale){
  float G = exp(-0.3);
  float amp = 1.;
  float freq = 1.;
  float n = 0.;
    
  for(int i = 0; i <5; i++){
    n+= worley(p*freq,scale*freq)*amp;
    freq*=2.;
    amp*=G;
  }
    
  return n*n;  
}

float3 overlay(in float3 src, in float3 dst, float intensity)
{
    return lerp(dst, lerp(2.0 * src * dst, 1.0 - 2.0 * (1.0 - src) * (1.0-dst), step(0.5, dst)), intensity);
}

float4 main(float4 sampleColor : COLOR0, float2 uv : TEXCOORD0, float4 fragCoord : SV_POSITION) : COLOR0
{   
    fragCoord += float4(0.5f, 0.5f, 0.0f, 0.0f);
    float intensity = 1;
    
    float2 uv2 = fragCoord.xy / (uSize / 10);
    
    float3 param = float3(uv2.x, uv2.y, uSize.x*5. + uTime*0.1);

    float4 color = tex2D(uImage0, uv);
    float fbm_1 = fbm(param, 2) * 3;
    float3 mult = float3(5 * fbm_1,.8 * fbm_1,.05 * fbm_1);

    mult = pow(mult.xyz, float3(smoothstep(1, .80, intensity - 0.5).xxx));
    mult = pow(mult.xyz, 0.80);
    float3 color_resolution = float3(8., 8., 8.);
    float3 janding =
    floor((mult.xyz) * color_resolution) / (color_resolution - (1.).xxx);

    janding = overlay(janding, color.xyz, 1.0f);

    return float4(janding.xyz, color.a);
}

#define TECHNIQUES
#include "TungstenArmorMoltenShader.effect.uniforms.hlsl"
