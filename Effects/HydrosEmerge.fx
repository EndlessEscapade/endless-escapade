//Lightning code
sampler uImage0 : register(s0);

texture noiseTexture;

float4 newColor;

float lerpVal;

float2 time;

float thresh;

float2 noiseBounds;
float2 imgBounds;

bool invert;

float frames;
float myFrame;

float2 offset;

float alpha;

sampler noiseSampler = sampler_state
{
    Texture = (noiseTexture);
};

float4 HydrosEmergeFloat(float2 coords : TEXCOORD0) : COLOR0
{
    float4 nativeColor = tex2D(uImage0, float2(coords.x, coords.y));

    float val = coords.y * frames;
    
    val %= 1;
    
    float4 noiseColor = tex2D(noiseSampler, (float2(coords.x, val) * (imgBounds / noiseBounds)) + float2(time.x, time.y) + offset);

    if (invert)
        noiseColor.r = 1 - noiseColor.r;
    
    if (nativeColor.a == 0)
    {
        return float4(0, 0, 0, 0);
    }
    
    float inverseNoiseColorR = 1 - noiseColor.r;

    float4 finalColor = lerp(nativeColor, lerp(lerp(float4(1, 1, 1, 1), newColor, (inverseNoiseColorR * 2)), newColor * noiseColor.r, inverseNoiseColorR), lerpVal) * alpha;
    
    float avg = (finalColor.r + finalColor.g + finalColor.b) / 3;
    
    float4 trueFinalColor = (finalColor.r + ((avg - finalColor.r) * 0.2f), finalColor.g + ((avg - finalColor.g) * 0.2f), finalColor.b + ((avg - finalColor.b) * 0.2f), finalColor.a);
    
    return trueFinalColor * finalColor;
}

technique HydrosEmerge
{
    pass P0
    {
        PixelShader = compile ps_2_0 HydrosEmergeFloat();
    }
};