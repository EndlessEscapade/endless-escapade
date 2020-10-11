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
texture noise;
texture water;
sampler noiseSampler = sampler_state
{
    Texture = (noise);
};
sampler waterMapSampler = sampler_state
{
    Texture = (water);
};

float4 WaterShader(float4 position : SV_POSITION, float2 coords : TEXCOORD0) : COLOR0
{
    float4 waterMap = tex2D(noiseSampler, coords);
    float4 noiseMap = tex2D(waterMapSampler, coords);
    float4 Texture = tex2D(uImage0, coords);
    Texture += noiseMap * waterMap;
    return Texture;
}

technique Technique1
{
    pass WaterShader
    {
        PixelShader = compile ps_2_0 WaterShader();
    }
}