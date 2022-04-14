sampler s0 : register(s0);
sampler uImage1 : register(s1);
texture noiseTexture;
sampler noiseSampler = sampler_state
{
    Texture = (noiseTexture);
};

float yCoord;
float xDis;
float t;
float GetHeight(float xCoord)
{
    float2 strip = float2(xCoord + xDis, yCoord);
    float height = tex2D(noiseSampler, strip).r;
    return height;
}
float4 NoiseSurfacing(float2 coords : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(uImage1, coords);
    float height = GetHeight(GetHeight(coords.x / 2)/5);
    float newcolour = abs(height - coords.y);
    if (coords.y < height)
    {
        return float4(0,0,0,0);
    }
    float mult = (0.5 - abs(0.5 - coords.x))*2;
    return float4(newcolour * t * mult, newcolour * t * mult, newcolour * t * mult, newcolour * t * mult);
}

technique BasicColorDrawing
{
    pass P0
    {
        PixelShader = compile ps_2_0 NoiseSurfacing();
    }
};