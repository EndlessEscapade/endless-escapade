//Lightning code
sampler uImage0 : register(s0);

texture noise;

float4 newColor;

float4 baseColor;

float lerpVal;

sampler noiseSampler = sampler_state
{
    Texture = (noise);
};

float4 ShadowWarpFloat(float2 coords : TEXCOORD0) : COLOR0
{
    float4 ogColor = tex2D(uImage0, float2(coords.x, coords.y));
    float4 texColor = tex2D(noiseSampler, float2(coords.x, coords.y));
    
    if (lerpVal > 0.5)
    {
        return lerp(float4(newColor.r, newColor.g, newColor.b, texColor.r) * ogColor.a, float4(ogColor.r * baseColor.r, ogColor.g * baseColor.g, ogColor.b * baseColor.b, ogColor.a), (lerpVal - 0.5) * 2);
    }
    else
    {
        return lerp(float4(0, 0, 0, 0), float4(newColor.r, newColor.g, newColor.b, texColor.r) * ogColor.a, lerpVal * 2);
    }
}

technique ShadowWarp
{
    pass P0
    {
        PixelShader = compile ps_2_0 ShadowWarpFloat();
    }
};