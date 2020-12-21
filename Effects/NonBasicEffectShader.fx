matrix WorldViewProjection;
texture noiseTexture;
sampler noiseSampler = sampler_state
{
	Texture = (noiseTexture);
};
texture spotTexture;
sampler spotSampler = sampler_state
{
	Texture = (spotTexture);
};
texture polkaTexture;
sampler polkaSampler = sampler_state
{
	Texture = (polkaTexture);
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
float GetHeight(float2 Coord)
{
	return tex2D(noiseSampler, Coord).r;
}
float4 SideFallOff(VertexShaderOutput input) : COLOR
{
	return input.Color * sin(input.TextureCoordinates.y * 3.14159265);
}
float4 Web(VertexShaderOutput input) : COLOR
{
	float2 coords2 = float2(input.TextureCoordinates.x/3 + 0.1f,(input.TextureCoordinates.y/15) + 0.1f);
	float2 coords = float2(input.TextureCoordinates.x / 2, (input.TextureCoordinates.y / 10));
	float polkaColor = tex2D(polkaSampler, coords).r;
	float polkaColor2 = tex2D(polkaSampler, coords2).r;
	input.Color *= polkaColor;
	input.Color *= polkaColor2;
	return input.Color * sin(GetHeight(input.TextureCoordinates.y)) * sin(input.TextureCoordinates.y * 5);
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

float4 Basic(VertexShaderOutput input) : COLOR
{
	float2 coords = float2(input.TextureCoordinates.x,input.TextureCoordinates.y);
	float4 spotColor = tex2D(spotSampler, coords).r;
	input.Color *= GetHeight(coords/2 + float2(sin(progress)/5 + 0.5f,cos(progress)/5 + 0.5f))*3;
	input.Color *= 1 + coords.x * abs(sin(input.TextureCoordinates.y * 20))*5;
	input.Color *= sin(input.TextureCoordinates.y * 3.14f); 
	input.Color.rgb -= float3(1 + spotColor.r, 1 - spotColor.g, spotColor.b);
	return input.Color;
}
float4 WaterPogPass(VertexShaderOutput input) : COLOR
{
	float2 coords = float2(input.TextureCoordinates.x,input.TextureCoordinates.y);
	float4 spotColor = tex2D(spotSampler, coords).r;
	input.Color *= GetHeight(coords / 2 + float2(sin(progress) / 5 + 0.5f,cos(progress) / 5 + 0.5f)) * 3;
	input.Color *= 1 + coords.x * abs(sin(input.TextureCoordinates.y * 20)) * 5;
	input.Color *= sin(input.TextureCoordinates.y * 3.14f);
	input.Color.rgb -= float3(1 + spotColor.r, 1 - spotColor.g, spotColor.b);
	return input.Color;
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
	pass Edge
	{
		PixelShader = compile ps_2_0 Basic();
	}
	pass AquaLightPass
	{
		PixelShader = compile ps_2_0 MainPSA();
	}
	pass WebPass
	{
		PixelShader = compile ps_2_0 Web();
	}
	pass SideFallOff
	{
		PixelShader = compile ps_2_0 SideFallOff();
	}
	pass BasicImagePass
	{
		VertexShader = compile vs_2_0 MainVS();
		PixelShader = compile ps_2_0 BasicImage();
	}
}