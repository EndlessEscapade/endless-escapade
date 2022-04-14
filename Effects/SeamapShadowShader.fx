sampler uImage0 : register(s0);

float4 color;
float4 ShadowFloat(float2 coords : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(uImage0, coords);
    
    if(colour.a > 0)
        return color;
    else
        return float4(0, 0, 0, 0);

}

technique SeamapShadowShader
{
    pass P0
    {
        PixelShader = compile ps_2_0 ShadowFloat();
    }
};