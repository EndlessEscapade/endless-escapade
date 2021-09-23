sampler s0 : register(s0);
sampler uImage1 : register(s1);

texture noiseTexture;

sampler noiseSampler = sampler_state
{
	Texture = (noiseTexture);
};

float4 newColor;

float lerpVal;

float GetValue(float xCoord, float yCoord)
{
    float2 strip = float2(xCoord, yCoord);
    float height = tex2D(noiseSampler, strip).r;
    return height;
}

float4 HydrosEmergeFloat(float2 coords : TEXCOORD0) : COLOR0
{
	float4 fetchColor = tex2D(noiseTexture, coords);

	float val = GetValue(coords.x, coords.y);

	if (fetchColor.a == 0)
    {
        return float4(0, 0, 0, 0);
    }

	return lerp(fetchColor, (newColor.r, newColor.g, newColor.b, fetchColor.a), lerpVal);
}

technique HydrosEmerge
{
    pass P0
    {
		PixelShader = compile ps_2_0 HydrosEmergeFloat();
	}
};