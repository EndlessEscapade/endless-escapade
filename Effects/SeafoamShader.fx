sampler uImage0 : register(s0);

texture foamTexture;
texture rippleTexture;

matrix WorldViewProjection;

float4 noColor; //    no color/transparent
float4 color1; //     darkest
float4 color2;
float4 color3;
float4 color4;
float4 color5; //     lightest

float2 offset;

sampler foamSampler = sampler_state
{
    Texture = (foamTexture);
};

sampler rippleSampler = sampler_state
{
    Texture = (rippleTexture);
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
    VertexShaderOutput output = (VertexShaderOutput)0;

    output.Color = input.Color;
    output.TexCoords = input.TexCoords;
    output.Position = mul(input.Position, WorldViewProjection);

    return output;
}

float4 SeafoamFloat(VertexShaderOutput input) : COLOR0
{
    float4 foamColor = tex2D(foamSampler, input.TexCoords + offset);
    float4 rippleColor = tex2D(rippleSampler, input.TexCoords + offset);
    
    float4 inputColor = input.Color;

    float opacityVal = (foamColor.r * 1.3f) * (inputColor.r + abs((input.TexCoords.y - 0.5f) * 0.2f));

    if (opacityVal < 0.1f) {
        return noColor;
    }
    if (opacityVal < 0.3f) {
        return color1;
    }
    if (opacityVal < 0.5f) {
        return color2;
    }
    if (opacityVal < 0.7f) {
        return color3;
    }
    if (opacityVal < 0.95f)
    {
        return color4;
    }

    return color5;
}

technique SeafoamShader
{
    pass P0
    {
        PixelShader = compile ps_2_0 SeafoamFloat();
    }
};