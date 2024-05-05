#pragma warning( disable : 4717 ) 

sampler uImage0 : register(s0);

texture densityNoisemap; //controls the density of clouds across the whole seamap

float4 icyWaterColor;
float4 neutralWaterColor;
float4 tropicalWaterColor;

sampler densityNoisemapSampler = sampler_state
{
    Texture = (densityNoisemap);
};

float4 WaterShaderBaseFloat(float2 coords : TEXCOORD0) : COLOR0
{
    float4 densityThresh = tex2D(densityNoisemapSampler, coords);
    
    if(densityThresh.r < 0.5)
        return lerp(icyWaterColor, neutralWaterColor, (densityThresh.r - 0.25) * 4);
    else
        return lerp(neutralWaterColor, tropicalWaterColor, (densityThresh.r - 0.5) * 4);
}

technique WaterShaderBase
{
    pass P0
    {
        PixelShader = compile ps_3_0 WaterShaderBaseFloat();
    }
};