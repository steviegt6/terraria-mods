// This is an adapted version of the shader "Plants growing from nowhere"
// written by `paperu`.
// The original source can be found @ <https://www.shadertoy.com/view/cdVXDK>.
// It has been translated to HLSL 3.0 through SPIR-V and hand-tweaked to match
// our use case.
// I am not sure what the license is and do not consider it to be licensed under
// AGPL-3.0 despite the rest of the codebase being so for such purposes.  Tread
// carefully.
// You can find the original author here: <paperu.net>.

#include "CoolFlowerShader.uniforms.hlsl";
#include "../common.hlsl"
#include "../raymarching.hlsl"
#include "../glsl.hlsl"

sampler uImage0 : register(s0);

struct rmRes
{
    float3 p;
    int i;
    bool h;
};

static int m;

static const float3 flower_color = float3(1., 0., 177. / 255.);
static const float3 stem_color = float3(1. / 255., 1. / 255., 129. / 255.);

#define ORIENTED(f) float2(cos((f)), sin((f)))
#define MATRIX_MUL(f, m) f = mul(f, m)

float2 apply_repetitive_rotation(float2 p, float angular_iteration)
{
    // Compute the angular offset based on the number of iterations (aIt).
    float angle = (-(M_TAU / angular_iteration) * floor((atan2(p.x, p.y) / M_TAU + 0.5f) * angular_iteration)) - M_PI - (M_TAU / (angular_iteration * 2.0f));

    // Apply rotation matrix to the point with the computed angle.
    return mul(rotation_matrix(angle), p);
}

float2 repRot(float2 p, float aIt)
{
    float param = (((-(6.283185482025146484375f / aIt)) * floor(((atan2(p.x, p.y) / 6.283185482025146484375f) + 0.5f) * aIt)) - 3.1415927410125732421875f) - (6.283185482025146484375f / (aIt * 2.0f));
    return mul(rotation_matrix(param), p);
}

float petal_distance(inout float2 uv, float width_modulation_factor)
{
    // Adjust x-coordinate for petal symmetry and width scaling.
    uv.x = abs(uv.x) + 0.25f + (0.25f * width_modulation_factor);

    // Compute distance from the petal boundary.
    return length(uv) - 0.5f;
}

float petal(inout float3 position, float modulation_factor)
{
    // Compute petal transformation parameters.
    float rotationAngle = (modulation_factor - 0.015f) * 2.0f;

    // Offset the petal base in the y and z directions.
    position.y -= 0.45f;
    position.z -= 0.5f;

    // Rotate around the y-axis using a computed angle.
    position.zy = mul(rotation_matrix(rotationAngle), position.zy);

    // Compute primary petal distance function.
    float distance = petal_distance(position.xy, modulation_factor);

    // Reflect across the x-axis for symmetry.
    position.x = abs(position.x);

    // Rotate around the x-axis slightly to refine petal shape.
    position.xz = mul(rotation_matrix(-0.25f), position.xz);

    // Compute the cylindrical distance function.
    float c1 = length(position.yz) - 0.5f;

    // Return the final petal shape, combining petal distortion and cylindrical boundary.
    return max(max(distance, abs(c1) - 0.01f), position.z);
}

float flower(float3 position, float angular_iterations, float modulation_factor)
{
    // Apply rotational repetition to the xy-plane
    position.xy = apply_repetitive_rotation(position.xy, angular_iterations);

    // Compute the petal deformation based on the transformed position
    return petal(position, modulation_factor);
}

float distance_function(in float3 position)
{
    position.y = -position.y;
    float param = 1.0160000324249267578125f;
    float3 _239 = position;
    float2 _241 = mul(rotation_matrix(param), _239.xz);
    position.x = _241.x;
    position.z = _241.y;
    float param_1 = -0.63999998569488525390625f;
    float3 _249 = position;
    float2 _251 = mul(rotation_matrix(param_1), _249.xy);
    position.x = _251.x;
    position.y = _251.y;
    float dd = 10000000000.0f;
    float ee = 10000000000.0f;
    float3 p = position;
    bool b = false;
    float3 _275;
    for (float g = 0.0f; g < 3.0f; g += 1.0f)
    {
        bool _273 = b;
        bool _274 = !_273;
        b = _274;
        if (_274)
        {
            _275 = p.xzy;
        }
        else
        {
            _275 = p.zxy;
        }
        p = _275;
        float r = length(p.xy);
        float3 pp_1 = float3(log(r) - (uTime * (0.100000001490116119384765625f + ((g + 1.0f) * 0.05099999904632568359375f))), atan2(p.x, p.y), p.z / r);
        float e = dot(pp_1.xy, float2(0.92387950420379638671875f, 0.3826834261417388916015625f));
        float f = dot(pp_1.xy, float2(0.3826834261417388916015625f, -0.92387950420379638671875f));
        float k = 1.2021000385284423828125f;
        e = glsl_mod(e, k) - (k * 0.5f);
        float l = 0.64999997615814208984375f;
        f += 1.2999999523162841796875f;
        float i = glsl_mod(floor(f / l) + g, 3.0f);
        f = glsl_mod(f, l) - (l * 0.5f);
        float d = (length(float2(e, pp_1.z)) - (0.014999999664723873138427734375f / r)) * r;
        bool j = i == 0.0f;
        float param_2 = dd;
        float param_3 = d;
        float param_4 = 0.100000001490116119384765625f;
        dd = smooth_union_sdf(param_2, param_3, param_4);
        float3 param_5 = float3(e, f, pp_1.z + 0.0599999986588954925537109375f) / float3(0.25f, 0.25f, 0.25f);
        float param_6 = smoothstep(-1.0f, 1.0f, r * r) * (j ? 5.0f : 2.0f);
        float param_7 = smoothstep(1.0f, -0.0f, r * r);
        float _397 = flower(param_5, param_6, param_7);
        float ff = (_397 * 0.25f) * r;
        ee = min(ee, ff);
        if (ee == ff)
        {
            m = int(j);
        }
    }
    float ff_1 = min(dd, ee);
    if (ff_1 == dd)
    {
        m = 0;
    }
    return ff_1 * 0.800000011920928955078125f;
}

rmRes rm(float3 c, float3 r)
{
    rmRes _469 = {c + (r * 0.0f), 0, false};
    rmRes s = _469;
    s.i = 0;
    for (; s.i < 500; s.i++)
    {
        float3 param = s.p;
        float _485 = distance_function(param);
        float d = _485;
        if (d < 0.00019999999494757503271102905273438f)
        {
            s.h = true;
            break;
        }
        if (distance(c, s.p) > 30.0f)
        {
            break;
        }
        s.p += (r * d);
    }
    return s;
}

#define MAX_D 30.0f
#define MIN_D 0.0f
#define LIM 0.0002f
#define MAX_IT 500

float3 calculate_normal(in float3 position)
{
    float distance_at_point = distance_function(position);
    float epsilon = LIM; // Small offset for finite differencing.

    // Compute finite differences in x, y, and z directions.
    float3 offset_x = float3(epsilon, 0.0f, 0.0f);
    float3 offset_y = float3(0.0f, epsilon, 0.0f);
    float3 offset_z = float3(0.0f, 0.0f, epsilon);

    float3 normal = float3(
        distance_function(position + offset_x) - distance_at_point,
        distance_function(position + offset_y) - distance_at_point,
        distance_function(position + offset_z) - distance_at_point
    );

    return normalize(normal);
}

float4 main(float2 coords : SV_POSITION, float2 tex_coords : TEXCOORD0) : COLOR0
{
    // Very simple check.  If the image we're using to draw over is transparent
    // at the given pixel, skip trying to process it and return transparent.
    // TODO: Handle partial transparency?
    if (tex2D(uImage0, tex_coords).a == 0)
    {
        return float4(0., 0., 0., 0.);
    }

    float2 resolution = uSource.xy;
    float2 position = uSource.zw;

    // Make coords relative.
    coords -= position;

    float2 fragCoord = floor((coords + 0.5f) / uPixel) * uPixel;

    float2 st = (fragCoord - 0.5f * resolution.xy) / resolution.x;

    float3 c = float3(0.0f, 0.0f, -10.0f);
    float3 r = normalize(float3(st, 1.0f));
    float3 param = c;
    float3 param_1 = r;
    rmRes _551 = rm(param, param_1);
    rmRes res = _551;
    float3 sky = float3(0., 0., 0.);
    float3 color = sky;

    if (res.h)
    {
        float3 param_2 = res.p;
        float3 _574 = calculate_normal(param_2);
        float3 n = _574;
        float d = max(0.0f, dot(n, float3(0.0f, 0.99503719806671142578125f, -0.099503718316555023193359375f)));
        float s = pow(max(0.0f, dot(r, reflect(float3(0.0f, 0.99503719806671142578125f, -0.099503718316555023193359375f), n))), 1.0f);

        // shading of some sort
        color = lerp(float3(0.885, 0.882, 0.945), float3(1., 1., 1.), d);

        // actual colors; flower : stem
        color *= m == 1 ? flower_color : stem_color;

        float alpha = smoothstep(20.0f, 25.0f, distance(res.p, c)).x;
        color = lerp(color, sky, alpha.xxx);
        color = lerp(color, sky, smoothstep(0.5f, 3.0f, dot(st, st) * 10.0f).xxx);

        return float4(color, .01f);
    }

    return float4(color, 0.0f);
}

#ifdef FX
technique Technique1
{
    pass FlowerShader
    {
        PixelShader = compile ps_3_0 main();
    }
}
#endif // FX
