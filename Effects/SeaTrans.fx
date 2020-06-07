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
    float realTime = uOpacity - 1000;
    realTime = clamp(uOpacity - 1000, 0, 100000);
    colour *= 1 - (coords.x * realTime)/200;
    return colour;
}

technique Technique1
{
    pass SeaTrans
    {
        PixelShader = compile ps_2_0 FilterMyShader();
    }
}