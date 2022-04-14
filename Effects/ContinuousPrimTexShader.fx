//Lightning code
sampler uImage0 : register(s0);

texture maskTexture;

matrix WorldViewProjection;

float4 darkColor;
float4 lightColor;

sampler maskSampler = sampler_state
{
    Texture = (maskTexture);
};

struct VertexShaderInput
{
    float2 TextureCoordinates : TEXCOORD0;
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float2 TextureCoordinates : TEXCOORD0;
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};
VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    float4 pos = mul(input.Position, WorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;

    output.TextureCoordinates = input.TextureCoordinates;

    return output;
};

float4 LoopTexFloat(VertexShaderOutput input) : COLOR0
{
    float4 texColor = tex2D(maskSampler, input.TextureCoordinates);
    
    return lerp(darkColor, lightColor, texColor.r);
}

technique ContinuousPrimTexShader
{
    pass P0
    {
        PixelShader = compile ps_2_0 LoopTexFloat();
    }
};