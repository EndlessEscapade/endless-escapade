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
    return float2((int)(num.x * scale) / (float)scale, (int)(num.y * scale) / (float)scale);
}

float3 Colour;
float waveSpeed;
float3 LightColour;
float4 WaterShader(float4 position : SV_POSITION, float2 coords : TEXCOORD0) : COLOR0
{
    float xRes = 1 / (1980);
    float yRes = 1 / (1080);
    float2 Center = float2(0.5f, 0.5f);
    float2 newCoord = Round(coords, 500);
    float sina = abs(sin(newCoord.x * 10 + xCoord * 60 - newCoord.y*(30+sin(newCoord.x*20))));
    float2 alteredCoords = newCoord;
    float2 pixelPos = alteredCoords + GetNoisePixel(alteredCoords) + float2(xCoord, yCoord)*(waveSpeed + sina);
    float4 waterMap = tex2D(waterMapSampler, pixelPos);
    float2 noisePos = alteredCoords * 0.1f + float2(xCoord + 0.3f, yCoord + 0.3f);
    float pix = GetNoisePixel(noisePos);
    float4 colour;
    colour.rgb = Colour;
    colour.a = 1;
    float targetAlt = (1 + sina / 10);
    float4 target = float4(0.5f / targetAlt, 0.9f / targetAlt, targetAlt,1);
    colour = lerp(colour,target,(pix * waterMap.b*2)* (1 + sina / 10));
    colour.rgb *= LightColour*LightColour;
    return colour;
}

technique Technique1
{
    pass WaterShader
    {
        PixelShader = compile ps_2_0 WaterShader();
    }
}