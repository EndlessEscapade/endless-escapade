sampler uImage0 : register(s0);

texture dots;
texture noise;

float random;

float time;

float4 color;

sampler dotSampler = sampler_state
{
    Texture = (dots);
};

sampler noiseSampler = sampler_state
{
    Texture = (noise);
};

float4 PolkaFloat(float2 coords : TEXCOORD0) : COLOR0
{
    float4 ogColor = tex2D(uImage0, float2(coords.x, coords.y));

    float4 noiseColor = tex2D(noiseSampler, float2((coords.x * 6) % 1, (((coords.y + random) % 1) * 6) % 1));
    float4 texColor = tex2D(dotSampler, float2((coords.x * 6) % 1, (((coords.y + random) % 1) * 6) % 1));
    
    if (texColor.r < 0.5)
    {
        return (color * ogColor.r) - (((sin(coords.x * 2 - coords.y * 2 + time) * 0.5) + 0.5) * noiseColor.r * 0.5);
    }
    else
    {
        return float4(0, 0, 0, 0);
    }
}

technique PolkaDot
{
    pass P0
    {
        PixelShader = compile ps_2_0 PolkaFloat();
    }
};