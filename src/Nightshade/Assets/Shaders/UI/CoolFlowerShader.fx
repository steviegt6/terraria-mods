#define PIXEL_SIZE 2.

sampler uImage0 : register(s0);

struct rmRes
{
    float3 p;
    int i;
    bool h;
};

// uniform float3 iResolution;
// uniform float iTime;

// float3 uResolution;
float4 uSource;
float uTime;

#define iResolution float3(uSource.xy, 0.)
#define iTime uTime

static float4 gl_FragCoord;
static float4 fragColor;

/*struct SPIRV_Cross_Output
{
    float4 fragColor : COLOR0;
};*/

static float t;
static int m;

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

float2x2 rot(float a)
{
    return float2x2(float2(cos(a), sin(a)), float2(-sin(a), cos(a)));
}

float opSmoothUnion(float d1, float d2, float k)
{
    float h = clamp(0.5f + ((0.5f * (d2 - d1)) / k), 0.0f, 1.0f);
    return lerp(d2, d1, h) - ((k * h) * (1.0f - h));
}

float2 repRot(float2 p, float aIt)
{
    float param = (((-(6.283185482025146484375f / aIt)) * floor(((atan2(p.x, p.y) / 6.283185482025146484375f) + 0.5f) * aIt)) - 3.1415927410125732421875f) - (6.283185482025146484375f / (aIt * 2.0f));
    return mul(rot(param), p);
}

float petalDcp(inout float2 uv, float w)
{
    uv.x = (abs(uv.x) + 0.25f) + (0.25f * w);
    return length(uv) - 0.5f;
}

float petal(inout float3 p, float m_1)
{
    float tt = mod(t, 3.1415927410125732421875f);
    float ouv = m_1 - 0.014999999664723873138427734375f;
    float w = m_1;
    float a = m_1;
    p.y -= 0.449999988079071044921875f;
    p.z -= 0.5f;
    float param = ouv * 2.0f;
    float3 _145 = p;
    float2 _147 = mul(rot(param), _145.zy);
    p.z = _147.x;
    p.y = _147.y;
    float2 param_1 = p.xy;
    float param_2 = w;
    float _158 = petalDcp(param_1, param_2);
    float pDcp = _158;
    p.x = abs(p.x);
    float param_3 = -0.25f;
    float3 _166 = p;
    float2 _168 = mul(rot(param_3), _166.xz);
    p.x = _168.x;
    p.z = _168.y;
    float c1 = length(p.yz) - 0.5f;
    return max(max(pDcp, abs(c1) - 0.00999999977648258209228515625f), p.z);
}

float flower(inout float3 p, float aIt, float m_1)
{
    float2 param = p.xy;
    float param_1 = aIt;
    float2 _220 = repRot(param, param_1);
    p.x = _220.x;
    p.y = _220.y;
    float3 param_2 = p;
    float param_3 = m_1;
    float _229 = petal(param_2, param_3);
    return _229;
}

float df(inout float3 pp)
{
    pp.y = -pp.y;
    float param = 1.0160000324249267578125f;
    float3 _239 = pp;
    float2 _241 = mul(rot(param), _239.xz);
    pp.x = _241.x;
    pp.z = _241.y;
    float param_1 = -0.63999998569488525390625f;
    float3 _249 = pp;
    float2 _251 = mul(rot(param_1), _249.xy);
    pp.x = _251.x;
    pp.y = _251.y;
    float dd = 10000000000.0f;
    float ee = 10000000000.0f;
    float3 p = pp;
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
        float3 pp_1 = float3(log(r) - (t * (0.100000001490116119384765625f + ((g + 1.0f) * 0.05099999904632568359375f))), atan2(p.x, p.y), p.z / r);
        float e = dot(pp_1.xy, float2(0.92387950420379638671875f, 0.3826834261417388916015625f));
        float f = dot(pp_1.xy, float2(0.3826834261417388916015625f, -0.92387950420379638671875f));
        float k = 1.2021000385284423828125f;
        e = mod(e, k) - (k * 0.5f);
        float l = 0.64999997615814208984375f;
        f += 1.2999999523162841796875f;
        float i = mod(floor(f / l) + g, 3.0f);
        f = mod(f, l) - (l * 0.5f);
        float d = (length(float2(e, pp_1.z)) - (0.014999999664723873138427734375f / r)) * r;
        bool j = i == 0.0f;
        float param_2 = dd;
        float param_3 = d;
        float param_4 = 0.100000001490116119384765625f;
        dd = opSmoothUnion(param_2, param_3, param_4);
        float3 param_5 = float3(e, f, pp_1.z + 0.0599999986588954925537109375f) / 0.25f.xxx;
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
    rmRes _469 = { c + (r * 0.0f), 0, false };
    rmRes s = _469;
    s.i = 0;
    for (; s.i < 500; s.i++)
    {
        float3 param = s.p;
        float _485 = df(param);
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

float3 normal(float3 p)
{
    float3 param = p;
    float _434 = df(param);
    float d = _434;
    float2 u = float2(0.0f, 0.00019999999494757503271102905273438f);
    float3 param_1 = p + u.yxx;
    float _443 = df(param_1);
    float3 param_2 = p + u.xyx;
    float _449 = df(param_2);
    float3 param_3 = p + u.xxy;
    float _455 = df(param_3);
    return normalize(float3(_443, _449, _455) - d.xxx);
}

void frag_main()
{
    // fragColor = float4(float2(gl_FragCoord.xy / iResolution.xy), 0.,1.);
    // return;

    float2 fragCoord = gl_FragCoord.xy - uSource.zw;
    fragCoord = floor(fragCoord / PIXEL_SIZE) * PIXEL_SIZE;
    // fragCoord /= PIXEL_SIZE;
    
    // float2 st = (fragCoord - (iResolution.xy * 0.5f)) / iResolution.x.xx;
    float2 st = (fragCoord - 0.5f * iResolution.xy) / iResolution.x;
    // float2 st = floor(fragCoord.xy / PIXEL_SIZE) / (iResolution.x / PIXEL_SIZE);
    
    t = iTime;
    float3 c = float3(0.0f, 0.0f, -10.0f);
    float3 r = normalize(float3(st, 1.0f));
    float3 param = c;
    float3 param_1 = r;
    rmRes _551 = rm(param, param_1);
    rmRes res = _551;
    // float3 sky = float3(0.954999983310699462890625f, 0.912000000476837158203125f, 0.9309999942779541015625f) - (dot(st, st) * 0.20000000298023223876953125f).xxx;
    float3 sky = float3(0., 0., 0.);
    float3 color = sky;
    
    float alpha = 1.;
    
    if (res.h)
    {
        float3 param_2 = res.p;
        float3 _574 = normal(param_2);
        float3 n = _574;
        float d = max(0.0f, dot(n, float3(0.0f, 0.99503719806671142578125f, -0.099503718316555023193359375f)));
        float s = pow(max(0.0f, dot(r, reflect(float3(0.0f, 0.99503719806671142578125f, -0.099503718316555023193359375f), n))), 1.0f);
        
        /*color = lerp(float3(0.5f, 0.763000011444091796875f, 0.915000021457672119140625f), 1.0f.xxx, d.xxx);
        bool3 _607 = (m == 1).xxx;
        color *= float3(_607.x ? float3(0.9049999713897705078125f, 0.17000000178813934326171875f, 0.291999995708465576171875f).x : float3(0.8849999904632568359375f, 0.882000029087066650390625f, 0.944999992847442626953125f).x,
                        _607.y ? float3(0.9049999713897705078125f, 0.17000000178813934326171875f, 0.291999995708465576171875f).y : float3(0.8849999904632568359375f, 0.882000029087066650390625f, 0.944999992847442626953125f).y,
                        _607.z ? float3(0.9049999713897705078125f, 0.17000000178813934326171875f, 0.291999995708465576171875f).z : float3(0.8849999904632568359375f, 0.882000029087066650390625f, 0.944999992847442626953125f).z);*/
        
        // shading of some sort
        color = lerp(float3(0.885,0.882,0.945), float3(1., 1., 1.), d);
        
        // actual colors; flower : stem
        color *= m == 1 ? float3(1., 0., 177. / 255.) /*float3(0.,0.,1.)*/ : float3(1. / 255., 1. / 255., 129. / 255.) /*float3(0.,1.,0.)*/;
        
        alpha = smoothstep(20.0f, 25.0f, distance(res.p, c)).x;
        color = lerp(color, sky, alpha.xxx);
        color = lerp(color, sky, smoothstep(0.5f, 3.0f, dot(st, st) * 10.0f).xxx);
    
        fragColor = float4(color, .01f);
    }
    else
    {
        fragColor = float4(color, 0.0f);
    }
    
    // fragColor = float4(fragCoord / iResolution.xy,0.,1.);
}

float4 main(float2 coords : SV_POSITION, float2 texCoords : TEXCOORD0) : COLOR0
{
    if (tex2D(uImage0, texCoords).a == 0)
    {
        return float4(0., 0., 0., 0.);
    }

    gl_FragCoord = float4(coords, 0., 0.) + float4(0.5f, 0.5f, 0.0f, 0.0f);
    frag_main();
    //SPIRV_Cross_Output stage_output;
    //stage_output.fragColor = float4(fragColor);
    //return stage_output;
    return float4(fragColor);
}

/*float4 main(float4 coords : SV_POSITION)
{
    float2 st = (fragCoord.xy - iResolution.xy * 0.5) / iResolution.x;
    t = iTime;

    float3 c = float3(0.0, 0.0, -10.0), r = normalize(float3(st, 1.0));

    rmRes res = rm(c, r);

    float3 sky = (float3(0.955, 0.912, 0.931) - dot(st, st) * 0.2);
    float3 color = sky;

    if (res.h)
    {
        float3 n = normal(res.p);
        const float3 ld = normalize(float3(0.0, 1.0, -0.1));
        float d = max(0.0, dot(n, ld));
        float s = pow(max(0.0, dot(r, reflect(ld, n))), 1.0);
        color = lerp(float3(0.500, 0.763, 0.915), float3(1.0), d);
        color *= m == 1 ? float3(0.905, 0.170, 0.292) : float3(0.885, 0.882, 0.945);
        color = lerp(color, sky, smoothstep(20.0, 25.0, distance(res.p, c)));
        color = lerp(color, sky, smoothstep(0.5, 3.0, dot(st, st) * 10.0));
    }

    return float4(color, 1.0);
}*/

technique Technique1
{
    pass FlowerShader
    {
        PixelShader = compile ps_3_0 main();
    }
}
