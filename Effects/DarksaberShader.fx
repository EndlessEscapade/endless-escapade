//Lightning code
sampler uImage0 : register(s0);

float4 bladeColor;
float4 edgeColor;

float edgeThresh;

matrix transformMatrix;

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
    float4 texColor = input.Color;
    
    if (texColor.r > edgeThresh)
    {
        return lerp(bladeColor, edgeColor, (texColor.r - edgeThresh) * (1 / edgeThresh));
    }
    else
    {
        return bladeColor;
    }
}

technique LightningShader
{
    pass P0
    {
        PixelShader = compile ps_2_0 LightningShaderFloat();
    }
};