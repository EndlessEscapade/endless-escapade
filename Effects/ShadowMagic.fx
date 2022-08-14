//Lightning code
sampler uImage0 : register(s0);

texture maskTexture;

float4 color1;
float4 color2;
float4 color3;
float4 color4;
float4 color5;

float myDist;

float xOffset;

float alpha;

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

float4 ShadowMagicFloat(VertexShaderOutput input) : COLOR0
{
    float4 texColor = tex2D(maskSampler, float2(((input.TexCoords.x * myDist) + xOffset), input.TexCoords.y));
    
    //float4 texColor = tex2D(maskSampler, float2(((input.TexCoords.x * myDist) + xOffset) % 1, input.TexCoords.y));
    
    //0 - 0.06 = basically no alpha to light saturated violet
    //0.06 - 0.2 = light saturated violet
    //0.2-0.5 = more saturated violet
    //0.5-0.9 = very pink violet
    //0.9-1.0 = violet to pure white
    
    float4 returnColor = float4(0,0,0,0);
    
    if (texColor.r < 0.12f)
    {
        returnColor = lerp(float4(0, 0, 0, 0), color1, texColor.r / 0.12f);
    }
    else if (texColor.r < 0.25f)
    {
        returnColor = lerp(color1, color2, (texColor.r - 0.12f) / 0.13f);
    }
    else if(texColor.r < 0.57f)
    {
        returnColor = lerp(color2, color3, (texColor.r - 0.25f) / 0.32f);
    }
    else if(texColor.r < 0.985f)
    {
        returnColor = lerp(color3, color4, (texColor.r - 0.57f) / 0.415f);
    }
    else
    {
        returnColor = lerp(color4, color5, (texColor.r - 0.985f) / 0.015f);
    }
    
    returnColor.a = sqrt(texColor.r) * alpha * input.Color.a;
    
    return returnColor;
}

technique ShadowMagic
{
    pass P0
    {
        PixelShader = compile ps_2_0 ShadowMagicFloat();
    }
};