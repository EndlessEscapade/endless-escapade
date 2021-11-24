float4 newColor1;
float4 newColor2;
float4 newColor3;
float4 newColor4;
float4 newColor5;

float4 lightColor;

matrix transformMatrix;

texture maskTexture;

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
    output.Position = mul(input.Position, transformMatrix);

    return output;
}

float4 TornSailFloat(VertexShaderOutput input) : COLOR0
{
    float4 clothColor = tex2D(maskSampler, input.TexCoords);
    
    float4 texColor = input.Color;
    
    float4 finalColor = float4(0, 0, 0, 1);
    
    //float alpha = 1;
    
    //if (clothColor.r < input.TexCoords.x)
    //{
    //    alpha = 0;
    //}
    
    /*if (texColor.r < 0.05)
    {
        return newColor1 * alpha;
    }
    else if (texColor.r < 0.66)
    {
        return newColor2 * alpha;
    }
    else if (texColor.r < 0.85)
    {
        return newColor3 * alpha;
    }
    else if (texColor.r < 0.95)
    {
        return newColor4 * alpha;
    }
    else
    {
        return newColor5 * alpha;
    }*/
    
    if (texColor.r < 0.02)
    {
        finalColor = newColor1;
    }
    else if (texColor.r < 0.66)
    {
        if (texColor.r * clothColor.r < 0.1)
        {
            if (clothColor.r < 0.99)
            {
                finalColor = newColor1;
            }
            else
            {
                finalColor = newColor2;
            }
        }
        else if (texColor.r * clothColor.r < 0.4)
        {
            finalColor = newColor2;
        }
        else
        {
            finalColor = newColor3;
        }
    }
    else if (texColor.r * clothColor.r < 0.85)
    {
        finalColor = newColor3;
    }
    else if (texColor.r * clothColor.r < 0.95)
    {
        finalColor = newColor4;
    }
    else
    {
        finalColor = newColor4;
    }
    
    return float4(finalColor.r * lightColor.r, finalColor.g * lightColor.r, finalColor.b * lightColor.r, 1);
}

technique TornSailShader
{
    pass P0
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 TornSailFloat();
    }
};