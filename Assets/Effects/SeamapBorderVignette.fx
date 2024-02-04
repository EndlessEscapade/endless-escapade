#pragma warning( disable : 4717 ) 

sampler uImage0 : register(s0);

float4 color;

float4 VignetteFloat(float2 coords : TEXCOORD0) : COLOR0
{
    coords *= 1 - coords;
    
    return lerp(float4(0, 0, 0, 0), color, 1 - pow(coords.y * coords.x * 300, 1.3));
}

technique SeamapBorderVignette
{
    pass P0
    {
        PixelShader = compile ps_2_0 VignetteFloat();
    }
};