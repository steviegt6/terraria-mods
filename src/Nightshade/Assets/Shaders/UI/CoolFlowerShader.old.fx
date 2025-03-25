// #define mix lerp
// 
// #define T 6.283185307
// #define P T*.5
// 
// float uTime;
// // float t;
// 
// #define t uTime
// 
// // xy: absolute coords
// // zw: resolution
// float4 uSource;
// 
// float2x2 rot(in float a) { return float2x2(cos(a),sin(a),-sin(a),cos(a)); }
// 
// float opSmoothUnion( float d1, float d2, float k ) {
//     float h = clamp( 0.5 + 0.5*(d2-d1)/k, 0.0, 1.0 );
//     return mix( d2, d1, h ) - k*h*(1.0-h); }
// 
// #define orientedVec2(a) float2(cos((a)),sin((a)))
// 
// float petalDcp(in float2 uv, in float w)
// {
// 	uv.x = abs(uv.x) + .25 + .25*w;
// 	return length(uv) - .5;
// }
// 
// float petal(in float3 p, in float m)
// {
// 	float tt = fmod(t, P);
// 
// 	float ouv = m - .015;
// 	float w = m;
// 	float a = m;
// 	const float b = .5;
// 	p.y -= .45;
// 	p.z -= b*1.;
// 	// p.zy *= rot(ouv*2.);
// 	p.zy = mul(rot(ouv * 2.), p.zy);
// 	float pDcp = petalDcp(p.xy, w);
// 	p.x = abs(p.x);
// 	// p.xz *= rot(-.25);
// 	p.xz = mul(rot(-0.25), p.xz);
// 	float c1 = length(p.yz) - b;
// 	return max(max(pDcp, abs(c1) - .01), p.z);
// }
// 
// float2 repRot(in float2 p, in float aIt)
// {
// 	// return p*rot(-(T/aIt)*floor((atan2(p.x, p.y)/T + .5)*aIt) - P - T/(aIt*2.));
// 	return mul(p, rot(-(T / aIt) * floor((atan2(p.x, p.y) / T + 0.5) * aIt) - P - T / (aIt * 2.)));
// }
// 
// float flower(in float3 p, in float aIt, in float m)
// {
// 	p.xy = repRot(p.xy, aIt);
// 	return petal(p, m);
// }
// 
// // int m;
// // float df(in float3 pp)
// // {
// //     pp.y = -pp.y;
// //     // pp.xz *= rot(1.016), pp.xy *= rot(-0.640);
// //     pp.xz = mul(rot(1.016), pp.xz);
// //     pp.xy = mul(rot(-0.640), pp.xy);
// //     
// //     float dd = 10e9, ee = 10e9;
// //     float3 p = pp;
// //     
// //     const float fsz = .25;
// //     const float2 n = orientedVec2(P * .125);
// //     
// //     bool b = false;
// //     for(float g = 0.; g < 3.; g++)
// //     {
// //         p = (b = !b) ? p.xzy : p.zxy;
// //         float r = length(p.xy);
// //         float3 pp = float3(log(r) - t*(.1+((g+1.)*.051)), atan2(p.x, p.y) /*+ cos(sqrt(r))*.2*/, p.z/r);
// //         float e = dot(pp.xy, n), f = dot(pp.xy, float2(n.y,-n.x));
// //         {float k = 1.2021; e = fmod(e, k) - k*.5;}
// //         float l = .65; f += 1.3; float i = fmod(floor(f/l) + g, 3.); f = fmod(f, l) - l*.5;
// //         float d = (length(float2(e, pp.z)) - 0.015/r)*r;
// //         bool j = i == 0.;
// //         dd = opSmoothUnion(dd, d, .1);
// //         float ff = flower(float3(e, f, pp.z + .06)/fsz, smoothstep(-1., 1., r*r)*(j ? 5. : 2.), smoothstep(1., -0., r*r))*fsz*r;
// //         ee = min(ee, ff);
// //         if(ee == ff) m = j ? 1 : 0;
// //     }
// //     
// //     float ff = min(dd, ee);
// //     if(ff == dd) m = 0;
// //     return ff*.8;
// // }
// 
// float df(in float3 pp, out int m)
// {
//     pp.y = -pp.y;
//     pp.xz = mul(rot(1.016), pp.xz);
//     pp.xy = mul(rot(-0.640), pp.xy);
//     
//     float dd = 10e9, ee = 10e9;
//     float3 p = pp;
//     
//     const float fsz = .25;
//     const float2 n = orientedVec2(P * .125);
//     
//     bool b = false;
//     for(float g = 0.; g < 3.; g++)
//     {
//         p = (b = !b) ? p.xzy : p.zxy;
//         float r = length(p.xy);
//         float3 pp = float3(log(r) - t*(.1+((g+1.)*.051)), atan2(p.x, p.y) /*+ cos(sqrt(r))*.2*/, p.z/r);
//         float e = dot(pp.xy, n), f = dot(pp.xy, float2(n.y,-n.x));
//         {float k = 1.2021; e = fmod(e, k) - k*.5;}
//         float l = .65; f += 1.3; float i = fmod(floor(f/l) + g, 3.); f = fmod(f, l) - l*.5;
//         float d = (length(float2(e, pp.z)) - 0.015/r)*r;
//         bool j = i == 0.;
//         dd = opSmoothUnion(dd, d, .1);
//         float ff = flower(float3(e, f, pp.z + .06)/fsz, smoothstep(-1., 1., r*r)*(j ? 5. : 2.), smoothstep(1., -0., r*r))*fsz*r;
//         ee = min(ee, ff);
//         if(ee == ff) m = j ? 1 : 0; // Modify `m` through reference
//     }
//     
//     float ff = min(dd, ee);
//     if(ff == dd) m = 0;
//     return ff * 0.8;
// }
// 
// #define MAX_D 30.
// #define MIN_D 0.
// #define LIM .0002
// #define MAX_IT 500
// 
// // float3 normal(in float3 p) {
// //     float d = df(p);
// //     float2 u = float2(0., LIM);
// //     return normalize(float3(df(p + u.yxx), df(p + u.xyx), df(p + u.xxy)) - d);
// // }
// 
// float3 normal(in float3 p) {
//     int tmpM;
//     float d = df(p, tmpM);
//     float2 u = float2(0., LIM);
//     return normalize(float3(df(p + u.yxx, tmpM), df(p + u.xyx, tmpM), df(p + u.xxy, tmpM)) - d);
// }
// 
// // struct rmRes
// // {
// //     float3 p;
// //     int i;
// //     bool h;
// // }
// 
// struct rmRes
// {
//     float3 p;
//     int i;
//     bool h;
//     int m;
// };
// 
// // rmRes rm(in float3 c, in float3 r)
// // {
// //     rmRes s = rmRes(c + r * MIN_D, 0, false);
// //     float d;
// //     for (s.i = 0; s.i < MA_IT; s.i++)
// //     {
// //         d = df(s.p);
// //         if (d < LIM) { s.h = true; break; }
// //         if (distance(c, s.p) > MAX_D) break;
// //         s.p += d * r;
// //     }
// //     
// //     return s;
// // }
// 
// // rmRes rm(in float3 c, in float3 r)
// // {
// //     int tmpM;
// //     rmRes s = rmRes(c + r * MIN_D, 0, false);
// //     float d;
// //     for (s.i = 0; s.i < MAX_IT; s.i++)
// //     {
// //         d = df(s.p, tmpM);
// //         if (d < LIM) { s.h = true; break; }
// //         if (distance(c, s.p) > MAX_D) break;
// //         s.p += d * r;
// //     }
// //     
// //     return s;
// // }
// 
// rmRes rm(in float3 c, in float3 r)
// {
//     int tmpM;
//     rmRes s = { c + r * MIN_D, 0, false, 0 }; // Initialize m to 0
//     float d;
//     for (s.i = 0; s.i < MAX_IT; s.i++)
//     {
//         d = df(s.p, tmpM);
//         if (d < LIM) { s.h = true; s.m = tmpM; break; } // Store `m`
//         if (distance(c, s.p) > MAX_D) break;
//         s.p += d * r;
//     }
//     
//     return s;
// }
// 
// float4 main(float2 coords : SV_POSITION) : COLOR0
// {
//     float2 resolution = uSource.xy;
//     float2 position = uSource.zw;
// 
//     // mak relative
//     coords -= position;
//     
//     float2 st = (coords.xy - resolution.xy * .5) / resolution.x;
//     
//     float3 c = float3(0., 0., -10.);
//     float3 r = normalize(float3(st, 1.));
//     
//     // rmRes res = rm(c, r, out int m);
//     rmRes res = rm(c, r);
//     
//     // todo: make transparent lol
//     float3 sky = (float3(0.955, 0.912, 0.931) - dot(st, st) * .2);
//     float3 color = sky;
//     
//     if (res.h)
//     {
//         float3 n = normal(res.p);
//         const float3 ld = normalize(float3(0., 1., -.1));
//         float d = max(0., dot(n, ld));
//         float s = pow(max(0., dot(r, reflect(ld, n))), 1.);
//         color = mix(float3(0.500,0.763,0.915), float3(1., 1., 1.), d);
//         // color *= m == 1 ? float3(0.905,0.170,0.292) : float3(0.885,0.882,0.945);
//         color *= res.m == 1 ? float3(0.905, 0.170, 0.292) : float3(0.885, 0.882, 0.945);
//         color = mix(color, sky, smoothstep(20., 25., distance(res.p, c)));
//         color = mix(color, sky, smoothstep(0.5, 3., dot(st,st)*10.));
//     }
//     
//     return float4(color, 1.0);
// }

// Author: paperu (Translated to HLSL 3)
// Title: Plants growing from nowhere

#define T 6.283185307
#define P (T * 0.5)
#define MAX_D 30.0
#define MIN_D 0.0
#define LIM 0.0002
#define MAX_IT 500

float uTime; // t
#define t uTime

// xy: absolute coords
// zw: resolution
float4 uSource;

float2x2 rot(float a) { return float2x2(cos(a), sin(a), -sin(a), cos(a)); }

float opSmoothUnion(float d1, float d2, float k) {
    float h = saturate(0.5 + 0.5 * (d2 - d1) / k);
    return lerp(d2, d1, h) - k * h * (1.0 - h);
}

#define orientedVec2(a) float2(cos(a), sin(a))

float petalDcp(float2 uv, float w) {
    uv.x = abs(uv.x) + 0.25 + 0.25 * w;
    return length(uv) - 0.5;
}

float petal(float3 p, float m) {
    float ouv = m - 0.015;
    p.y -= 0.45;
    p.z -= 0.5;
    p.zy = mul(rot(ouv * 2.0), p.zy);
    float pDcp = petalDcp(p.xy, m);
    p.x = abs(p.x);
    p.xz = mul(rot(-0.25), p.xz);
    float c1 = length(p.yz) - 0.5;
    return max(max(pDcp, abs(c1) - 0.01), p.z);
}

float2 repRot(float2 p, float aIt) {
    return mul(p, rot(-(T / aIt) * floor((atan2(p.x, p.y) / T + 0.5) * aIt) - P - T / (aIt * 2.0)));
}

float flower(float3 p, float aIt, float m) {
    p.xy = repRot(p.xy, aIt);
    return petal(p, m);
}

float df(float3 pp, out int m) {
    pp.y = -pp.y;
    pp.xz = mul(rot(1.016), pp.xz);
    pp.xy = mul(rot(-0.640), pp.xy);
    
    float dd = 10e9, ee = 10e9;
    float3 p = pp;
    const float fsz = 0.25;
    const float2 n = orientedVec2(P * 0.125);
    bool b = false;
    
    for (float g = 0.0; g < 3.0; g++) {
        p = (b = !b) ? p.xzy : p.zxy;
        float r = length(p.xy);
        float3 pp = float3(log(r) - uTime * (0.1 + ((g + 1.0) * 0.051)), atan2(p.x, p.y), p.z / r);
        float e = dot(pp.xy, n), f = dot(pp.xy, float2(n.y, -n.x));
        e = fmod(e, 1.2021) - 0.60105;
        float l = 0.65; f += 1.3; float i = fmod(floor(f / l) + g, 3.0); f = fmod(f, l) - 0.325;
        float d = (length(float2(e, pp.z)) - 0.015 / r) * r;
        bool j = (i == 0.0);
        dd = opSmoothUnion(dd, d, 0.1);
        float ff = flower(float3(e, f, pp.z + 0.06) / fsz, smoothstep(-1.0, 1.0, r * r) * (j ? 5.0 : 2.0), smoothstep(1.0, 0.0, r * r)) * fsz * r;
        ee = min(ee, ff);
        if (ee == ff) m = j ? 1 : 0;
    }
    float ff = min(dd, ee);
    if (ff == dd) m = 0;
    return ff * 0.8;
}

float3 normal(float3 p) {
    int tmpM;
    float d = df(p, tmpM);
    float2 u = float2(0.0, LIM);
    return normalize(float3(df(p + u.yxx, tmpM), df(p + u.xyx, tmpM), df(p + u.xxy, tmpM)) - d);
}

struct rmRes {
    float3 p;
    int i;
    bool h;
    int m;
};

rmRes rm(float3 c, float3 r) {
    rmRes s = { c + r * MIN_D, 0, false, 0 };
    int tmpM;
    float d;
    
    for (s.i = 0; s.i < MAX_IT; s.i++) {
        d = df(s.p, tmpM);
        if (d < LIM) { s.h = true; s.m = tmpM; break; }
        if (distance(c, s.p) > MAX_D) break;
        s.p += d * r;
    }
    return s;
}

float4 main(float2 coords : SV_POSITION) : COLOR0 {
    float2 resolution = uSource.xy;
    float2 position = uSource.zw;
    // mak relative
    coords -= position;

    // float2 st = (coords.xy - float2(640, 360)) / 640.0;
    // float2 st = (coords.xy - resolution.xy) / resolution.x;
    float2 st = (coords.xy - 0.5 * resolution.xy) / min(resolution.x, resolution.y);
    float3 c = float3(0.0, 0.0, -10.0);
    float3 r = normalize(float3(st, 1.0));
    rmRes res = rm(c, r);
    float3 sky = float3(0.955, 0.912, 0.931) - dot(st, st) * 0.2;
    float3 color = sky;
    
    if (res.h) {
        float3 n = normal(res.p);
        const float3 ld = normalize(float3(0.0, 1.0, -0.1));
        float d = max(0.0, dot(n, ld));
        color = lerp(float3(0.500, 0.763, 0.915), float3(1.0, 1.0, 1.0), d);
        color *= res.m == 1 ? float3(0.905, 0.170, 0.292) : float3(0.885, 0.882, 0.945);
        color = lerp(color, sky, smoothstep(20.0, 25.0, distance(res.p, c)));
        color = lerp(color, sky, smoothstep(0.5, 3.0, dot(st, st) * 10.0));
    }
    
    return float4(color, 1.0);
}

technique Technique1
{
    pass FlowerShader
    {
        PixelShader = compile ps_3_0 main();
    }
}