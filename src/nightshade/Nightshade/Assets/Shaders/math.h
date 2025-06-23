#ifndef MATH_HLSL
#define MATH_HLSL

/* 
 * MATH_HLSL
 * This file provides mathematical constants and helper functions for
 * mathematical operations.
 */

#ifndef M_PI
/**
 * π (pi)
 */
#define M_PI 3.14159265f
#endif

#ifndef M_TAU
/**
 * τ (tau); 2π
 */
#define M_TAU 6.283185307f
#endif

/**
 * Constructs a 2D rotation matrix.
 * 
 * This function returns a 2x2 matrix that rotates a vector by `angle` radians.
 * It follows the standard 2D rotation matrix formula:
 * 
 *     R = | cos(a)  sin(a) |
 *         | -sin(a) cos(a) |
 * 
 * @param angle The angle (in radians) to rotate by.
 * @return A float2x2 matrix representing the rotation transformation.
 */
float2x2 rotation_matrix(float angle)
{
    float c = cos(angle);
    float s = sin(angle);
    return float2x2(float2(c, s), float2(-s, c));
}

#endif // MATH_HLSL
