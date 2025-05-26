#ifndef RAYMARCHING_HLSL
#define RAYMARCHING_HLSL

/**
 * Smooth union operation for signed distance functions (SDFs).
 * 
 * This function blends two distance values (`d1` and `d2`) using a smooth
 * transition controlled by `k`. It is useful for merging shapes without hard
 * intersections.
 * 
 * @param d1 Distance to the first shape.
 * @param d2 Distance to the second shape.
 * @param k Smoothing factor (higher values create a softer transition).
 * @return A smoothly blended distance field.
 */
float smooth_union_sdf(float d1, float d2, float k)
{
    float h = clamp(0.5f + 0.5f * (d2 - d1) / k, 0.0f, 1.0f);
    return lerp(d2, d1, h) - k * h * (1.0f - h);
}

#endif // RAYMARCHING_HLSL