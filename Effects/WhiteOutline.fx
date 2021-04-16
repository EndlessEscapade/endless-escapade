sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float alpha;
float3 color;
float4 White(float2 coords : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(uImage0, coords);
    colour = float4(color.x, color.y, color.z, 2) * alpha * colour.a;
    colour *= (alpha + 0.2f);
    return colour;
}

technique BasicColorDrawing
{
    pass WhiteSprite
    {
        PixelShader = compile ps_2_0 White();
    }
};