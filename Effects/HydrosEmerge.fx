sampler uImage0 : register(s0);

texture noiseTexture;

sampler noiseSampler = sampler_state
{
    Texture = (noiseTexture);
};

float4 newColor;

float lerpVal;

float time;

float GetValue(float xCoord, float yCoord)
{
    float2 strip = float2(xCoord, yCoord);
    float height = tex2D(noiseSampler, strip).r;
    return height;
}

float4 HydrosEmergeFloat(float2 coords : TEXCOORD0) : COLOR0
{
    float4 nativeColor = tex2D(uImage0, coords);

    float4 noiseColor = tex2D(noiseSampler, coords + float2(time, time));

    float val = GetValue(coords.x, coords.y);

    if (nativeColor.a == 0)
    {
        return float4(0, 0, 0, 0);
    }

    return lerp(nativeColor, (newColor.r, newColor.g, newColor.b, GetValue(coords.x, coords.y)), lerpVal);
}

technique HydrosEmerge
{
    pass P0
    {
        PixelShader = compile ps_2_0 HydrosEmergeFloat();
    }
};