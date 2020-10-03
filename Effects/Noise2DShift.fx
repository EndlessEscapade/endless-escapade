sampler s0 : register(s0);
sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
texture noiseTexture;
sampler noiseSampler = sampler_state
{
    Texture = (noiseTexture);
};

float yCoord;
float xCoord;
float t;

float GetHeight(float2 Coord)
{
    float2 strip = float2(Coord.x, Coord.y);
    float height = tex2D(noiseSampler, strip).r;
    return height;
}
float4 FilterMyShader(float2 coords : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(uImage0, coords);
    float height = GetHeight(coords * 0.1f + float2(xCoord + 0.3f, yCoord + 0.3f));
    float lerp = height * GetHeight(float2(xCoord * coords.x, yCoord * coords.y));
    
    if (distance(float2(0.5f, 0.5f), coords) < 0.4f + lerp*0.1f)
    {
        colour = float4(0.1f, 0.1f, 0.5f, 1);
        colour += lerp;
        colour *= (1.5f - distance(float2(0.7f, 0.4f), coords)*1.8f)*2;
    }
    else
    {
        colour = 0;
    }

    return colour;
}

technique Technique1
{
    pass Noise2D
    {
        PixelShader = compile ps_2_0 FilterMyShader();
    }
}