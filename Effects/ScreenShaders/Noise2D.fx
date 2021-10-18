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
texture noiseTexture;
sampler noiseSampler = sampler_state
{
    Texture = (noiseTexture);
};
float GetHeight(float2 Coord)
{
    float2 strip = float2(Coord.x, Coord.y);
    float height = tex2D(noiseSampler, strip).r;
    return height;
}
float4 FilterMyShader(float2 coords : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(uImage0, coords);
    if (abs(GetHeight(coords) - uOpacity) < 0.01f)
        colour.b += 1;
    if (GetHeight(coords) < uOpacity)
    {
        colour *= (1 - uOpacity);
    }
    return colour;
}

technique Technique1
{
    pass P1
    {
        PixelShader = compile ps_2_0 FilterMyShader();
    }
}