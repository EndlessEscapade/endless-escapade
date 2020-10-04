sampler s0 : register(s0);
sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
texture noiseTexture;
texture tentacle;
sampler noiseSampler = sampler_state
{
    Texture = (noiseTexture);
};
sampler tent = sampler_state
{
    Texture = (tentacle);
};

float yCoord;
float xCoord;
float t;
float3 lightColour;
float GetHeight(float2 Coord)
{
    float2 strip = float2(Coord.x, Coord.y);
    float height = tex2D(noiseSampler, strip).r;
    return height;
}
float4 FilterMyShader(float2 coords : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(uImage0, coords);
    float4 colour2 = tex2D(tent, coords);
    float height = GetHeight(coords * 0.1f + float2(xCoord + 0.3f, yCoord + 0.3f));
    float lerp = height * GetHeight(float2(xCoord * coords.x, yCoord * coords.y));
    
    if (distance(float2(0.5f, 0.5f), coords) < 0.4f + lerp*0.2f)
    {
        //colobur = float4(0.1f, 0.1f, 0.5f, 1);
        colour.b += lerp*4;
        colour.g += lerp;
        colour.b += lerp;
        float a = (1.5f - distance(float2(0.7f - xCoord, 0.4f - yCoord), coords) * 1.2f) * 2 * height;
        if (distance(float2(0.5f, 0.5f), coords) > 0.385f + lerp * 0.2f)
            colour = float4(0.01f, 0.01f, 1, 10);
        colour.rgb += (lightColour.b * colour2 * a)/3.1f;
        colour.rgb *= lightColour + 0.5f;
       
    }
    else
    {
        float b = 1 - distance(float2(0.5f, 0.5f),coords)*1.6f;
        colour = float4(0, 0, b, b);
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