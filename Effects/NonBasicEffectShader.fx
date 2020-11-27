matrix WorldViewProjection;

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

float2 coordMultiplier;
float2 coordOffset;
float strength;
float progress;
float progress2;
//custom passes
texture imageTexture;
sampler imageSampler = sampler_state
{
    Texture = (imageTexture);
    AddressU = Wrap;
    AddressV = Wrap;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;
    float4 pos = mul(input.Position, WorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;

	output.TextureCoordinates = input.TextureCoordinates;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	input.Color.r += sin(input.TextureCoordinates.x * 4 + progress);
	input.Color.g -= cos(input.TextureCoordinates.x * 4 + progress);
	input.Color.b += cos(input.TextureCoordinates.x * 4 + progress);
	return input.Color * sin(input.TextureCoordinates.y * 3.14159265);
}
float hue2rgb(float p, float q, float t){
            if(t < 0) t += 1;
            if(t > 1) t -= 1;
            if(t < 0.166f) return p + (q - p) * 6 * t;
            if(t < 0.5f) return q;
            if(t < 0.66f) return p + (q - p) * (2/3 - t) * 6;
            return p;
        }
float4 hslToRgb(float h, float s, float l){
    float r, g, b;
        float q = l < 0.5 ? l * (1 + s) : (l + s) - (l * s);
        float p = (2 * l) - q;
        r = hue2rgb(p, q, h + 0.33f); 
        g = hue2rgb(p, q, h);
        b = hue2rgb(p, q, h - 0.33f); 
	return float4(r,g,b,1);
}

float4 MainPSA(VertexShaderOutput input) : COLOR
{
	input.Color *= hslToRgb(0.64f + (0.18f * (float)sin((input.TextureCoordinates.y * 4) + progress)), 1, 0.7f);
	return input.Color;
}
float4 MainPS3(VertexShaderOutput input) : COLOR
{
	input.Color.r += sin(input.TextureCoordinates.x * 5);
	return input.Color;
}
float4 Extras(VertexShaderOutput input) : COLOR
{
	return input.Color * sin(input.TextureCoordinates.y * 3.14159265);
}


float4 BasicImage(VertexShaderOutput input) : COLOR
{
    float alpha = abs((1.0 - strength) + tex2D(imageSampler, coordOffset + input.TextureCoordinates * coordMultiplier).r * strength);
	return input.Color * alpha;
}

technique BasicColorDrawing
{
	pass DefaultPass
	{
		VertexShader = compile vs_2_0 MainVS();
	}
	pass RainbowLightPass
	{
		PixelShader = compile ps_2_0 MainPS();
	}
	pass AquaLightPass
	{
		PixelShader = compile ps_2_0 MainPSA();
	}
	pass BasicImagePass
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 BasicImage();
	}
}