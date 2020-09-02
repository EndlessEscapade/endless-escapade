sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float4 FilterMyShader(float2 coords : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(uImage0, coords);
    float aPixelX = 1 / uScreenResolution.x;
    float aPixelY = 1 / uScreenResolution.y;
    float disX = (uImageOffset.x - coords.x) * (uScreenResolution.x/ uScreenResolution.y);
    float disY = uImageOffset.y - coords.y;
    float dist = sqrt(disX * disX + disY * disY);
    if (dist < (uColor.r + uColor.g + uColor.b) / 10)
    {
        colour.r *= 1 + ((uIntensity / (dist + uIntensity)) * uColor.r);
        colour.g *= 1 + ((uIntensity / (dist + uIntensity)) * uColor.g);
        colour.b *= 1 + ((uIntensity / (dist + uIntensity)) * uColor.b);
    }
    return colour;
}

technique Technique1
{
    pass LightSource
    {
        PixelShader = compile ps_2_0 FilterMyShader();
    }
}