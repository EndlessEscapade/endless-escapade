sampler s0 : register(s0);
sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
texture noiseTexture;
sampler noiseSampler = sampler_state
{
    Texture = (noiseTexture);
};

float yCoord;
float xDis;
float t;
float2 pos;
float progress;
float alpha;
float GetHeight(float2 pos)
{
    return tex2D(noiseSampler, pos).r;
}
float4 NoiseSurfacing(float2 coords : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(uImage0, coords);
    float2 centrecoords = coords - float2(0.5f, 0.5f);
    float angle = atan2(centrecoords.y, centrecoords.x);
    float angles = angle - progress;
    float2 circularSample = float2(0.5f + sin(angles)/5, 0.5f + cos(angles) / 5);
    colour *= (1 -distance(coords,float2(0.5f,0.5f))*GetHeight(circularSample - float2(cos(pos.x), cos(pos.y))/10)*5) * alpha;
    return colour;
}

technique BasicColorDrawing
{
    pass P0
    {
        PixelShader = compile ps_2_0 NoiseSurfacing();
    }
};