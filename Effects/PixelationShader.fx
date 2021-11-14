//Lightning code
sampler uImage0 : register(s0);

float width;
float height;

float4 PixelationFloat(float2 coords : TEXCOORD0) : COLOR0
{
    //float4 pixelColor = tex2D(uImage0, float2((width / 2), (height / 2) * ));
    
    float2 vec = float2(width, height) / 2;
    coords = floor(coords * vec) / vec;
    
    float4 pixelColor = tex2D(uImage0, coords);
    
    return pixelColor;
}

technique PixelationShader
{
    pass P0
    {
        PixelShader = compile ps_2_0 PixelationFloat();
    }
};