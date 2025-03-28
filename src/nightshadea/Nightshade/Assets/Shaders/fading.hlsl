#ifndef FADING_HLSL
#define FADING_HLSL

/* 
 * FADING_HLSL
 * Utility functions for rendering smooth fade effects, commonly used in 
 * signed distance functions (SDFs) for soft blending of shapes.
 */

/**
 * Smooth minimum function that blends between two values, reducing sharp edges.
 * 
 * @param a First value.
 * @param b Second value.
 * @param k Smoothing factor (higher values create a smoother transition).
 * @return A smoothly blended minimum of `a` and `b`.
 */
float smooth_min(float a, float b, float k)
{
    float h = max(k - abs(a - b), 0.0f) / k;
    return min(a, b) - h * h * k * 0.25f;
}

#endif // FADING_HLSL