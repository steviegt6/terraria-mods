#include "../common.hlsl"
#include "../pixelation.hlsl"

sampler uImage0 : register(s0);

float uTime;
float4 uSource;
float uHoverIntensity;
float uPixel;
float uColorResolution;
float uGrayness;
float3 uInColor;
float uSpeed;

/**
 * Calculates a smooth fade effect near the edges of a given region.
 *
 * @param coords The screen-space coordinates.
 * @param size The dimensions of the screen or target area.
 * @param fade_width The width of the fade effect.
 * @param smooth_factor Controls the smooth transition.
 * @return The fade weight, determining transparency near edges.
 */
float get_edge_fade_weight(float2 coords, float2 size, float fade_width, float smooth_factor)
{
    coords = floor(coords / uPixel) * uPixel;

    float2 edge_distance = min(coords, size - coords);
    float distance = smooth_min(edge_distance.x, edge_distance.y, smooth_factor);

    float fade_amount = smoothstep(fade_width * 0.05, fade_width, distance);

    return lerp(0.25, 1., fade_amount);
}

float4 main(float2 coords : SV_POSITION, float2 tex_coords : TEXCOORD0) : COLOR0
{
    float2 resolution = uSource.xy;
    float2 position = uSource.zw;

    coords -= position;

    // Normalize the coordinates but with optional pixelation.
    float2 uv = normalize_with_pixelation(coords, uPixel, resolution);

    // Apply an edge fade that gets darker when close to the bounds to add some
    // depth.
    float fade_width = 20.0;
    float smooth_factor = 10.0;
    float fade_weight = get_edge_fade_weight(coords, resolution, fade_width, smooth_factor);

    // Apply the distortion effect.
    for (float i = 1.; i < 10.; i++)
    {
        uv.x += .5 / i * sin(i * 3. * uv.y + uTime * uSpeed);
        uv.y += .3 / i * cos(i * 3. * uv.x + uTime * uSpeed);
    }

    // Determine the color based on our now-distorted coordinates.
    float3 new_color = float3(
        uInColor.r * cos(uv.x + uv.y + 1.),
        uInColor.g * sin(uv.x + uv.y + 1.),
        uInColor.b * cos(sin(uv.x + uv.y) + cos(uv.x + uv.y)));

    // Account for the fade.
    new_color = lerp(float3(0, 0, 0), new_color, fade_weight);

    float3 quantized_color = quantize_color(new_color, uColorResolution);

    // Get the grayscale value.
    float gray = get_grayscale_color(quantized_color);

    // Compute the final transitional color.
    float3 final_color = float3(
        (quantized_color.r - gray) / 2. * uGrayness + (gray + quantized_color.r) / 2.,
        (quantized_color.g - gray) / 2. * uGrayness + (gray + quantized_color.g) / 2.,
        (quantized_color.b - gray) / 2. * uGrayness + (gray + quantized_color.b) / 2.
    );

    // Unfortunate hardcoded portion to facilitate a hover effect.
    if (final_color.b > 0.01f && final_color.r < 0.01f)
    {
        final_color.b = max(final_color.b / 4.0f, final_color.b * uHoverIntensity);
    }

    // Take the original texture into account.  This is because we draw to a UI
    // panel which will have corners and stuff of that nature, so we don't want
    // to draw over that.  We don't reference the original texture for the
    // shader's effects, but we'll take alpha into account.
    float alpha = tex2D(uImage0, tex_coords).a;
    return float4(final_color, alpha);
}

#ifdef FX
technique Technique1
{
    pass PanelShader
    {
        PixelShader = compile ps_3_0 main();
    }
}
#endif // FX
