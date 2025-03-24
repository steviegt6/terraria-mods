#define PIXEL_SIZE 2.

sampler uImage0 : register(s0);

float uTime;
float4 uSource;

// float greyness = 1;
// float3 inColor = float3(.3, .3, .3);
// float speed = .2;

float grayness;
float3 inColor;
float speed;

float uHoverIntensity;

float getFadeInWeight(float2 uv) // stolen from https://www.shadertoy.com/view/MlcSz2
{
  float edge = 3 * abs(sin(0.5));
  float4 v = smoothstep(0., edge, float4(uv, 1. - uv) );
  return v.x * v.y * v.z * v.w;
}

float getUniformFadeWeight(float2 uv, float2 center, float radius, float intensity) 
{
    float dist = length(uv - center);
    float edge = radius * abs(sin(0.5)); 
    return pow(clamp(smoothstep(0., edge, 1. - dist / radius) * intensity, 0.0, 1.0), 3.0);
}

float smoothMin(float a, float b, float k)
{
    float h = max(k - abs(a - b), 0.0) / k;
    return min(a, b) - h * h * k * 0.25;
}

float getEdgeFadeWeight(float2 coords, float2 size, float fadeWidth, float smoothFactor)
{
    coords = floor(coords / PIXEL_SIZE) * PIXEL_SIZE;
    
    float2 edgeDist = min(coords, size - coords); 
    float dist = smoothMin(edgeDist.x, edgeDist.y, smoothFactor); 

    float fadeAmount = smoothstep(fadeWidth * 0.05, fadeWidth, dist);

    return lerp(0.25, 1., fadeAmount);
}

float4 main(float4 sampleColor : COLOR0, float2 coords : SV_POSITION, float2 texCoords : TEXCOORD0) : COLOR0
{
    float2 resolution = uSource.xy;
    float2 position = uSource.zw;
    
    coords -= position;

    float2 uv = floor(coords.xy / PIXEL_SIZE) / (resolution.xy / PIXEL_SIZE);
    // float alp = getFadeInWeight(uv);
    
    // float2 center = float2(0.5, 0.5);
    // float fadeRadius = 0.4;
    // float intensity = 2.5; // Increase for more intense fade
    // float alp = getUniformFadeWeight(uv, center, fadeRadius, intensity);
    
    float fadeWidth = 20.0; // Increase for a smoother gradient
    float smoothFactor = 10.0; // Higher values smooth out the corners
    float alp = getEdgeFadeWeight(coords, resolution, fadeWidth, smoothFactor);
    
    for (float i = 1.; i < 10.; i++)
    {
        uv.x += .5 / i * sin(i * 3. * uv.y + uTime * speed);
        uv.y += .3 / i * cos(i * 3. * uv.x + uTime * speed);
    }
    
    float3 newCol = float3(
    inColor.r * cos(uv.x+uv.y+1.),
    inColor.g *sin(uv.x+uv.y+1.),
    inColor.b * cos(sin(uv.x+uv.y)+cos(uv.x+uv.y)));
    newCol = lerp(float3(0,0,0), newCol, alp);
    
    float3 colorResolution = float3(32., 32., 32.);
    float3 janding = floor((newCol) * colorResolution) / (colorResolution - 1.);
    
    // float grey =0.2126*janding.r+0.7152*janding.g+0.0722*janding.b;
    float grey = janding.r + janding.g + janding.b;
    float3 transitionalColor = float3(
        ((janding.r - grey) / 2.) * (grayness) + ((grey + janding.r) / 2.),
        ((janding.g - grey) / 2.) * (grayness) + ((grey + janding.g) / 2.),
        ((janding.b - grey) / 2.) * (grayness) + ((grey + janding.b) / 2.)
    );
    
    if (transitionalColor.b > .01 && transitionalColor.r < .01) {
        // transitionalColor.b = (transitionalColor.b / 2) + transitionalColor.b * uHoverIntensity;
        transitionalColor.b = max(transitionalColor.b / 4, transitionalColor.b * uHoverIntensity);
    }

    return float4(transitionalColor, tex2D(uImage0, texCoords).a);
}

technique Technique1
{
    pass PanelShader
    {
        PixelShader = compile ps_3_0 main();
    }
}