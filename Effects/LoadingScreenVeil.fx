sampler uImage0 : register(s0);

float leftBound;
float rightBound;

float4 VeilFloat(float2 coords : TEXCOORD0) : COLOR0
{
    float4 ogColor = tex2D(uImage0, float2(coords.x, coords.y));

    if (coords.x < leftBound)
    {
        ogColor.a -= min(max((leftBound - coords.x) * 5, 0), 1);
    }
    
    if (coords.x > rightBound)
    {
        ogColor.a -= min(max((coords.x - rightBound) * 5, 0), 1);
    }
    
    return ogColor;
}

technique LoadingScreenVeil
{
    pass P0
    {
        PixelShader = compile ps_2_0 VeilFloat();
    }
};