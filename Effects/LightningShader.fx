//Lightning code
sampler uImage0 : register(s0);

texture maskTexture;

float4 newColor;

sampler maskSampler = sampler_state
{
    Texture = (maskTexture);
};

float4 LightningShaderFloat(float2 coords : TEXCOORD0) : COLOR0
{
    float4 texColor = tex2D(maskSampler, float2(coords.x, coords.y));

    return float4(newColor.r * texColor.r, newColor.g * texColor.r, newColor.b * texColor.r, texColor.r);
}

technique LightningShader
{
    pass P0
    {
        PixelShader = compile ps_2_0 LightningShaderFloat();
    }
};