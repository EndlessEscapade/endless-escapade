//Lightning code
sampler uImage0 : register(s0);

texture maskTexture;

float4 newColor;

sampler maskSampler = sampler_state
{
    Texture = (maskTexture);
};

float4 LaserShaderFloat(float2 coords : TEXCOORD0) : COLOR0
{
    float4 texColor = tex2D(maskSampler, float2(coords.x, coords.y));
    
    return float4(lerp(0, lerp(newColor.r, 1.5, texColor.r - 0.5), texColor.r), lerp(0, lerp(newColor.g, 1.5, texColor.g - 0.5), texColor.g), lerp(0, lerp(newColor.b, 1.5, texColor.b - 0.5), texColor.b), texColor.r);
}

technique LaserShader
{
    pass P0
    {
        PixelShader = compile ps_2_0 LaserShaderFloat();
    }
};