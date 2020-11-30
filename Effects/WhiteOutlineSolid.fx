sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float alpha;
float4 White(float2 coords : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(uImage0, coords);
    colour = float4(1, 1, 1, 0) * colour.a * alpha;
    float ydist = abs(0.5f - coords.y);
    if (ydist > 0.48f)
        colour *= 0;
    return colour;
}

technique BasicColorDrawing
{
    pass WhiteSprite
    {
        PixelShader = compile ps_2_0 White();
    }
};