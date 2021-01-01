sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float alpha;
float shineSpeed;
texture tentacle;
float3 lightColour;
float shaderLerp;
float2 FRAMEHEAD;
float XPROG;
float YPROG;
float2 Scaling;
sampler tent = sampler_state
{
    Texture = (tentacle);
};

texture headTexture;
sampler headTextureSampler = sampler_state
{
    Texture = (headTexture);
};



float4 White(float2 coords : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(uImage0, coords);
    float4 colour2 = tex2D(tent, coords);
    float diag = (coords.y / 2) * ((colour2.r) * (colour2.r) * 1.2f);
    float4 colour3 = tex2D(headTextureSampler, float2((coords.x - XPROG) * Scaling.x + 0.3f + diag, 0.3f - ((coords.y + YPROG) * Scaling.y)));
    float pos = alpha - coords.x;
    float4 white = float4(1, 1, 1,1);
    if (colour.a > 0)
    {
        float clamper = clamp(0.6f - distance(alpha* shineSpeed, coords.x)*2,0,1)* colour2.r;
        colour.rgb = lerp(colour,white, clamper);
        colour.rgb *= shaderLerp;
        colour.rgb += colour3.rgb*0.5f;
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