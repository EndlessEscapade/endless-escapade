sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;
texture noise;
texture noiseN;
texture water;
float yCoord;
float xCoord;


sampler noiseSampler = sampler_state
{
    Texture = (noise);
};

sampler noiseSampler2 = sampler_state
{
    Texture = (noiseN);
};

sampler waterMapSampler = sampler_state
{
    Texture = (water);
};

float GetNoisePixelUnPixelated(float2 Coord)
{
    float2 pos = float2(Coord.x, Coord.y);
    float height = tex2D(noiseSampler2, pos).r;
    return height;
}

float GetNoisePixel(float2 Coord)
{
    float2 pos = float2(Coord.x, Coord.y);
    float height = tex2D(noiseSampler, pos).r;
    return height;
}

float2 Round(float2 num,int scale)
{
    return float2((int)(num.x * scale) / scale, round(num.y * scale) / scale);
}

float3 Colour;
float waveSpeed;
float4 WaterShader(float4 position : SV_POSITION, float2 coords : TEXCOORD0) : COLOR0
{
    float stateX;
    float stateY;
    float xRes = 1 / (1980);
    float yRes = 1 / (1080);
    float2 Center = float2(0.5f, 0.5f);
    float dist = distance(Center, coords.x);

    //Keep just in case;
		if (round(coords.x * (1980)) % 2 != 0)
		{
			stateX = -xRes;
		}
		else
		{
			stateX = 0;
		}
		if (round(coords.y * (1080) % 2 != 0)
		{
			stateY = -yRes;
		}
		else
		{
			stateY = 0;
		}
		float sina = abs(sin(coords.x * 20 + xCoord * 30 - coords.y*(30+sin(coords.x*5)))) + GetNoisePixel(coords)/5;
		float2 finalState = float2(stateX, stateY);
		float2 alteredCoords = finalState + coords;


    float2 pixelPos = alteredCoords + GetNoisePixel(alteredCoords) + float2(xCoord, yCoord)*(waveSpeed + sina);
    float4 waterMap = tex2D(waterMapSampler, pixelPos);
    float2 noisePos = alteredCoords * 0.1f + float2(xCoord + 0.3f, yCoord + 0.3f);
    float pix = GetNoisePixel(noisePos);
    float4 colour;
    colour.rgb = Colour;
    colour.a = 1;
    float4 target = float4(0.5f / (1 + sina / 6), 0.9f / (1 + sina / 6), 1* (1 +sina/ 6),1);
    colour = lerp(colour,target,(pix * waterMap.b)*1.2f);
    return colour;
}

technique Technique1
{
    pass WaterShader
    {
        PixelShader = compile ps_2_0 WaterShader();
    }
}