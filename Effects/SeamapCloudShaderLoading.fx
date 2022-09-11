//Thanks to triplefate for the cloud vignette effect
sampler uImage0 : register(s0);

texture cloudNoisemap; //controls the density of clouds in one "chunk"
texture densityNoisemap; //controls the density of clouds across the whole seamap

float2 arrayOffset;

float weatherDensity; //0-1, 0 being sunny and 1 being rainy

float4 cloudsColor1;
float4 cloudsColor2;
float4 cloudsColor3;
float4 cloudsColor4;

float stepsX;
float stepsY;

float2 wind;

float2 homeIslandPos;

float counter;

float2 vec;


sampler densityNoisemapSampler = sampler_state
{
    Texture = (densityNoisemap);
};

sampler cloudNoisemapSampler = sampler_state
{
    Texture = (cloudNoisemap);
};

float4 CloudShaderFloat(float2 coords : TEXCOORD0) : COLOR0
{
    coords = floor(coords * vec) / vec;
    
    float2 specialCoords = float2((coords.x + arrayOffset.x) / stepsX, ((coords.y * 0.6) + arrayOffset.y) / stepsY);
    
    float densityThresh = tex2D(densityNoisemapSampler, float2(0.5, 0.5) + (specialCoords / 2.0) + float2(-0.25, -0.25) + float2(cos(counter), sin(counter)) / 4.0).r; //ranges 0-1
    
    specialCoords *= 1 - specialCoords;
    
    float vignetteThresh = pow(specialCoords.y * specialCoords.x * 70, 1.1);
    
    vignetteThresh += min(max(sqrt((specialCoords.y - 0.98) / (specialCoords.x - 0.98)), 0), 1.0 / 40.0) * 6.0;
    
    float cloudColor = tex2D(cloudNoisemapSampler, (coords + wind) % 1).r; //ranges 0-1
    
      
    float doTheLerp = lerp(densityThresh + ((weatherDensity - 0.5) * -0.4), -0.1f + ((weatherDensity - 0.5) * -0.1), max(min(1 - vignetteThresh, 1), 0));
    
    
    if (cloudColor < doTheLerp + 0.1) //if no cloud
    {
        return float4(0, 0, 0, 0);
    }
    else if (cloudColor < doTheLerp + 0.4) //if no cloud
    {
        return lerp(float4(0, 0, 0, 0), cloudsColor1, (cloudColor - (doTheLerp + 0.1)) * 3.33);
    }
    else if (cloudColor < doTheLerp + 0.6) //if no cloud
    {
        return lerp(cloudsColor1, cloudsColor2, (cloudColor - (doTheLerp + 0.4)) * 5);
    }
    else if (cloudColor < doTheLerp + 0.75) //if no cloud
    {
        return lerp(cloudsColor2, cloudsColor3, (cloudColor - (doTheLerp + 0.6)) * 6.67);
    }
    else
    {
        return lerp(cloudsColor3, cloudsColor4, min((cloudColor - (doTheLerp + 0.75)) * 3, 1));
    }
}

technique SeamapCloudShaderLoading
{
    pass P0
    {
        PixelShader = compile ps_2_0 CloudShaderFloat();
    }
};