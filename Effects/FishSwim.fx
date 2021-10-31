
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
float uTime;
float alpha;

float4 FishSwim(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 newCoords = coords;
    newCoords.x = coords.x * 0.5f;
    newCoords.y = coords.y * 0.5f;
    float4 color = tex2D(uImage0, newCoords);
    float wave = 1 - frac(coords.x + uTime);
    color.rgb = color.rgb * wave;
    
    return color * sampleColor;
}

technique FishSwimm
{
    pass P0
    {
        PixelShader = compile ps_2_0 FishSwim();
    }
};