sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float alpha;
float4 White(float2 coords : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(uImage0, coords);
    if (colour.a > 0)
    {
        colour = float4(2, .1f, 2, 2) * alpha;
        colour *= abs(alpha - coords.x) + 0.2f;
    }
    return colour;
}

technique BasicColorDrawing
{
    pass WhiteSprite
    {
        PixelShader = compile ps_2_0 White();
    }
};