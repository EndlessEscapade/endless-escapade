sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float alpha;
float shineSpeed;
texture tentacle;
sampler tent = sampler_state
{
    Texture = (tentacle);
};
float4 White(float2 coords : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(uImage0, coords);
    float4 colour2 = tex2D(tent, coords);
    float pos = alpha - coords.x;
    float4 white = float4(1, 1, 1,1);
    if (colour.a > 0)
    {
        float clamper = clamp(0.6f - distance(alpha* shineSpeed, coords.x)*2,0,1)* colour2.r;
        colour.rgb = lerp(colour,white, clamper);
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