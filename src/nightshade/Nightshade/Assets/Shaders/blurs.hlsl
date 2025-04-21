#ifndef BLURS_HLSL
#define BLURS_HLSL

// my solution was slower than https://www.shadertoy.com/view/fsV3R3, so using this one

float gaussian(float2 i, float sigma2, float pisigma2) {
    float top = exp(-((i.x * i.x) + (i.y * i.y)) / sigma2);
    float bot = pisigma2;
    return top / bot;
}

float2 AspectCorrectedGBlurScale(float2 resolution, float blurIntensity) {
    return (1.0f.xx / resolution) * blurIntensity;
}

float4 gauss_blur(sampler sp, float2 uv, float2 scale, int samples) {
    const float pi = radians(180.);
    const float sigma = float(samples) * 0.25;
    const float sigma2 = 2. * sigma * sigma;
    const float pisigma2 = pi * sigma2;

    float2 offset = float2(0, 0);
    float weight = gaussian(offset, sigma2, pisigma2);
    float4 color_av = tex2D(sp, uv) * weight;
    float accum = weight;
    
    for (int x = 0; x <= samples / 2; ++x) {
        for (int y = 1; y <= samples / 2; ++y) {
            offset = float2(x, y);
            weight = gaussian(offset, sigma2, pisigma2);
            color_av += tex2D(sp, uv + scale * offset) * weight;
            accum += weight;

            color_av += tex2D(sp, uv - scale * offset) * weight;
            accum += weight;

            offset = float2(-y, x);
            color_av += tex2D(sp, uv + scale * offset) * weight;
            accum += weight;

            color_av += tex2D(sp, uv - scale * offset) * weight;
            accum += weight;
        }
    }
    float4 final = color_av / accum;
    // gamma correction
    final = pow(final, (1.0 / 2.2).xxxx);
    return final;
}

float4 gauss_bloom(sampler sp, float2 uv, float2 scale, int samples, float4 exposure, float thresh) {
    float4 bright = clamp(gauss_blur(sp, uv, scale, samples) - thresh.xxxx, 0.0f.xxxx, 1.0f.xxxx) * (1.0f / (1.0f - thresh));
    return bright * exposure;
}

#endif // BLURS_HLSL
