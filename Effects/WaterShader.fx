sampler uImage0 : register(s0);

texture noiseTex;
texture distortionMap;

float4 baseWaterColor;
float4 highlightColor;

float sinVal;

float width;
float height;

sampler noiseSampler = sampler_state
{
    Texture = (noiseTex);
};

float4 WaterShaderFloat(float2 coords : TEXCOORD0) : COLOR0
{
    float2 vec = float2(width, height) / 2;
    coords = floor(coords * vec) / vec;
    
    float4 noiseColor1 = tex2D(noiseSampler, float2(coords.x + sinVal, (coords.y) + (1.49 * sinVal)));
    float4 noiseColor2 = tex2D(noiseSampler, float2(coords.x - (1.01 * sinVal), (coords.y) + sinVal));
    
    float lerpFloat = (noiseColor1.r + noiseColor2.r) / 2.0;
    
    if (lerpFloat < 0.4)
        return lerp(baseWaterColor, highlightColor, 1.0 / 0.6);
    
    if (lerpFloat > 0.9)
        return lerp(baseWaterColor, highlightColor, 1.0 / 0.1);
    
    //if (lerpFloat == 0)
    //    return baseWaterColor;
    
    return lerp(baseWaterColor, highlightColor, (1.0 / (1 - lerpFloat)));
}

technique WaterShader
{
    pass P0
    {
        PixelShader = compile ps_2_0 WaterShaderFloat();
    }
};