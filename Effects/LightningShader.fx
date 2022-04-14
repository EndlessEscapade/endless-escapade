//Lightning code
sampler uImage0 : register(s0);

texture maskTexture;

float4 newColor;

matrix transformMatrix;

sampler maskSampler = sampler_state
{
    Texture = (maskTexture);
};

struct VertexShaderInput
{
    float4 Position : POSITION;
    float2 TexCoords : TEXCOORD0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION;
    float2 TexCoords : TEXCOORD0;
    float4 Color : COLOR0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    
    output.Color = input.Color;
    output.TexCoords = input.TexCoords;
    output.Position = mul(input.Position, transformMatrix);

    return output;
}

float4 LightningShaderFloat(VertexShaderOutput input) : COLOR0
{
    float4 texColor = tex2D(maskSampler, float2(input.TexCoords.x, input.TexCoords.y));
    
    return float4(lerp(0, lerp(newColor.r, 1.5, texColor.r - 0.5), texColor.r), lerp(0, lerp(newColor.g, 1.5, texColor.g - 0.5), texColor.g), lerp(0, lerp(newColor.b, 1.5, texColor.b - 0.5), texColor.b), texColor.r);
}

technique LightningShader
{
    pass P0
    {
        PixelShader = compile ps_2_0 LightningShaderFloat();
    }
};