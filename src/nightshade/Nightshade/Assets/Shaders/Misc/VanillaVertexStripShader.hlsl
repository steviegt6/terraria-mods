#undef TECHNIQUES
#include "VanillaVertexStripShader.effect.uniforms.hlsl"

#include "../pixelation.hlsl"

#define PIXEL_SIZE 12.0
#define QUANTIZE_RESOLUTION 2.0

sampler2D uImage0 : register(s0);
sampler2D uImage1 : register(s1);
sampler2D uImage2 : register(s2);

float uSaturation;
float uTime;
float uOpacity;
float4 uShaderSpecificData;

float2 uImageSize0;
float2 uImageSize1;
float2 uImageSize2;

// float4 uSource;

// float4 magic_missile(float2 coords : SV_POSITION, float4 color : COLOR0) : COLOR0
float4 magic_missile(float2 texCoord : TEXCOORD0, float4 color : COLOR0) : COLOR0
{
    // texCoord = pixelate(texCoord, 2.0);
    // texCoord = floor(texCoord / 2.0) * 2.0;

    // float2 resolution = uSource.xy;
    // float2 position = uSource.zw;

    // Make coords relative.
    // coords -= position;

    // float2 texCoord = normalize_with_pixelation(coords, PIXEL_SIZE, resolution);

    texCoord = normalize_with_pixelation(texCoord * uImageSize0, PIXEL_SIZE, uImageSize0);

    // This matches the constant: float4(1.5, -0.5, 0, 0)
    const float2 scaleBias = float2(1.5, -0.5);

    // Compute offset texture coordinate
    float2 offsetCoord;
    offsetCoord.x = texCoord.x * scaleBias.x + scaleBias.y + uSaturation * uTime;
    offsetCoord.y = texCoord.y;

    // Sample the textures
    // float4 tex2 = tex2D(uImage2, normalize_with_pixelation(offsetCoord * uImageSize2, PIXEL_SIZE, uImageSize2)); // r1
    // float4 tex1 = tex2D(uImage1, normalize_with_pixelation(offsetCoord * uImageSize1, PIXEL_SIZE, uImageSize1)); // r2
    // float4 tex0 = tex2D(uImage0, normalize_with_pixelation(texCoord * uImageSize0, PIXEL_SIZE, uImageSize0)); // r3

    float4 tex2 = tex2D(uImage2, offsetCoord); // r1
    float4 tex1 = tex2D(uImage1, offsetCoord); // r2
    float4 tex0 = tex2D(uImage0, texCoord); // r3

    // Perform the logic
    float diff = tex2.x - (texCoord.x * scaleBias.x + scaleBias.y);
    diff = saturate(diff + diff);
    float factor = tex1.x * diff;

    float4 outColor = tex0 * factor;
    outColor *= color;
    outColor *= uOpacity;

    return float4(quantize_color(outColor.rgb, QUANTIZE_RESOLUTION), outColor.a);
}

// float4 final_fractal_vertex(float2 coords : SV_POSITION, float4 color : COLOR0) : COLOR0
float4 final_fractal_vertex(float2 texCoord : TEXCOORD0, float4 color : COLOR0) : COLOR0
{
    // texCoord = pixelate(texCoord, 2.0);
    // texCoord = floor(texCoord / 2.0) * 2.0;

    // float2 resolution = uSource.xy;
    // float2 position = uSource.zw;

    // Make coords relative.
    // coords -= position;

    // float2 texCoord = normalize_with_pixelation(coords, PIXEL_SIZE, resolution);

    texCoord = normalize_with_pixelation(texCoord * uImageSize0, PIXEL_SIZE, uImageSize0);

    float reciprocalW = 1.0 / uShaderSpecificData.w;

    // Constants used in assembly shader
    const float3 c3 = float3(1.5, -0.5, 1.0);

    float offsetX = texCoord.x * c3.x + c3.y; // t0.x * 1.5 - 0.5

    // Modified coordinate for texture manipulation
    float warpedX = texCoord.x * uShaderSpecificData.x + uShaderSpecificData.y;
    float warpY = frac(abs(warpedX));
    float cmpX = (warpedX >= 0.0f) ? warpY : -warpY;

    float texCoordY = texCoord.y;

    // Adjusted X using reciprocal of w
    float adjustedX = warpedX * -reciprocalW + c3.z;

    float2 lookupCoord = float2(cmpX, texCoordY);

    // Sample textures
    float4 tex1 = tex2D(uImage1, lookupCoord); // r2
    float4 tex0 = tex2D(uImage0, lookupCoord); // r1

    float4 baseColor = tex0.x * color;
    baseColor *= uOpacity;

    // Intensity mask based on horizontal distance
    float diff = tex1.x - offsetX;
    diff = saturate(diff + diff); // clamp to [0,1]

    baseColor *= diff;

    float4 finalColor = adjustedX * baseColor;
    return float4(quantize_color(finalColor.rgb, QUANTIZE_RESOLUTION), finalColor.a);
}

#define TECHNIQUES
#include "VanillaVertexStripShader.effect.uniforms.hlsl"
