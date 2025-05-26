#ifndef COLORS_HLSL
#define COLORS_HLSL

static const float gray_factor = 1.0f / 3.0f;

float get_grayscale_color(float3 color)
{
    return dot(color, float3(gray_factor, gray_factor, gray_factor));;
}

#endif // COLORS_HLSL