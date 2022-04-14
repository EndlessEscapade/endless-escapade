//Lightning code
sampler uImage0 : register(s0);

float4 bladeColor;
float4 edgeColor;

float edgeThresh;

float ticks;

matrix transformMatrix;

texture noiseTexture;

sampler noiseSampler = sampler_state
{
    Texture = (noiseTexture);
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
    float4 texColor = input.Color;
    
    float4 noiseColor = tex2D(noiseSampler, float2(input.TexCoords.x, (input.TexCoords.y + ticks) % 1));
    float noiseVal = noiseColor.r;
    
    if (texColor.r > edgeThresh)
    {
        return lerp(bladeColor, edgeColor, min(max(((texColor.r - edgeThresh) * (1 / edgeThresh)) + ((noiseVal - 0.5) * 0.5f), 0), 1));
    }
    else
    {
        return bladeColor;
    }
}

technique DarksaberShader
{
    pass P0
    {
        PixelShader = compile ps_2_0 LightningShaderFloat();
    }
};