sampler uImage0 : register(s0);

float width;
float height;
float fadeThresh;

float time;

float4 ReflectionFloat(float2 coords : TEXCOORD0) : COLOR0
{
    if (coords.y > fadeThresh)
    {
        float intensity = (coords.y - fadeThresh) * 5;
        
        float xVal = coords.x + ((((sin(time + (coords.y * 3))) * intensity) % (abs(intensity) - (abs(intensity) % 1))) / width);
    
        float2 offset = float2(max(min(xVal, 1), 0), coords.y);
    
        float4 color = tex2D(uImage0, offset);
        
        return color * (1 - coords.y);
    }
    else
    {
        return tex2D(uImage0, coords);
    }
}

technique SeamapReflectionShader
{
    pass P0
    {
        PixelShader = compile ps_2_0 ReflectionFloat();
    }
};