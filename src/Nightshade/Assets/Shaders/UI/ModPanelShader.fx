#define PIXEL_SIZE 2.

sampler uImage0 : register(s0);

float uTime;
float2 uResolution;

// float greyness = 1;
// float3 inColor = float3(.3, .3, .3);
// float speed = .2;

float grayness;
float3 inColor;
float speed;

float4 main(float4 sampleColor : COLOR0, float2 coords : SV_POSITION, float2 texCoords : TEXCOORD0) : COLOR0
{
    float2 uv = floor(coords.xy / PIXEL_SIZE) * PIXEL_SIZE / uResolution.xy;
    
    for (float i = 1.; i < 10.; i++)
    {
        uv.x += .5 / i * sin(i * 3. * uv.y + uTime * speed);
        uv.y += .3 / i * cos(i * 3. * uv.x + uTime * speed);
    }
    
    float3 newColor = float3(
        inColor.r * cos(uv.x + uv.y + 1.),
        inColor.g * sin(uv.x + uv.y + 1.),
        inColor.b * cos(sin(uv.x + uv.y) + cos(uv.x + uv.y))
    );
    
    float3 colorResolution = float3(32., 32., 32.);
    float3 janding = floor((newColor) * colorResolution) / (colorResolution - 1.);
    
    float grey = janding.r + janding.g + janding.b;
    float3 transitionalColor = float3(
        ((janding.r - grey) / 2.) * (grayness) + ((grey + janding.r) / 2.),
        ((janding.g - grey) / 2.) * (grayness) + ((grey + janding.g) / 2.),
        ((janding.b - grey) / 2.) * (grayness) + ((grey + janding.b) / 2.)
    );
    
    // temporary
    if (transitionalColor.b > .1 && transitionalColor.r < .1) {
        transitionalColor.b = 0;
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