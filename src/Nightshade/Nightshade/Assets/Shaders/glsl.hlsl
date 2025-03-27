#ifndef GLSL_HLSL
#define GLSL_HLSL

/* 
 * GLSL_HLSL
 * Utility macros and functions to simplify porting GLSL code to HLSL.
 * Some functions may be derived from SPIR-V output during GLSL-to-HLSL
 * conversion.
 */

/**
 * GLSL-style modulo operation.
 * 
 * In GLSL, `mod(x, y)` behaves differently from HLSL’s `%` operator, as it
 * ensures the result stays within the range `[0, y)`, similar to a fractional
 * remainder.
 * 
 * This implementation follows that behavior.
 * 
 * @param x Dividend.
 * @param y Divisor.
 * @return The remainder after division, following GLSL’s `mod` behavior.
 */
float glsl_mod(float x, float y)
{
    return x - y * floor(x / y);
}

/**
 * GLSL-style modulo operation for float2.
 */
float2 glsl_mod(float2 x, float2 y)
{
    return x - y * floor(x / y);
}

/**
 * GLSL-style modulo operation for float3.
 */
float3 glsl_mod(float3 x, float3 y)
{
    return x - y * floor(x / y);
}

/**
 * GLSL-style modulo operation for float4.
 */
float4 glsl_mod(float4 x, float4 y)
{
    return x - y * floor(x / y);
}

#endif // GLSL_HLSL