sampler uImage0 : register(s0);

texture maskTexture;

matrix WorldViewProjection;

float4 noColor; //    no color/transparent
float4 color1; //     darkest
float4 color2;
float4 color3;
float4 color4; //     lightest

float2 offset;

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
    VertexShaderOutput output = (VertexShaderOutput)0;

    output.Color = input.Color;
    output.TexCoords = input.TexCoords;
    output.Position = mul(input.Position, WorldViewProjection);

    return output;
}

float4 SeafoamFloat(VertexShaderOutput input) : COLOR0
{
    float4 texColor = tex2D(maskSampler, input.TexCoords + offset);
    float4 inputColor = input.Color;

    float opacityVal = (texColor.r * 1.5f) * (inputColor.r - abs((input.TexCoords.y - 0.5f) * 0.2f));

    if (opacityVal < 0.15f) {
        return noColor;
    }
    if (opacityVal < 0.2f) {
        return lerp(noColor, color1, (opacityVal - 0.15f) * 20.0f);
    }
    if (opacityVal < 0.25f) {
        return color1;
    }
    if (opacityVal < 0.3f) {
        return lerp(color1, color2, (opacityVal - 0.25f) * 20.0f);
    }
    if (opacityVal < 0.45f) {
        return color2;
    }
    if (opacityVal < 0.5f) {
        return lerp(color2, color3, (opacityVal - 0.45f) * 20.0f);
    }
    if (opacityVal < 0.7f) {
        return color3;
    }
    if (opacityVal < 0.75f) {
        return lerp(color3, color4, (opacityVal - 0.7f) * 20.0f);
    }

    return color4;
}

technique SeafoamShader
{
    pass P0
    {
        PixelShader = compile ps_2_0 SeafoamFloat();
    }
};