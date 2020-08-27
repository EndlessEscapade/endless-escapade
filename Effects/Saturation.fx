sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float4 FilterMyShader(float2 coords : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(uImage0, coords);
    float aPixelX = 1 / uScreenResolution.x;
    float aPixelY = 1 / uScreenResolution.y;
    float disX = uImageOffset.x - coords.x;
    float disY = uImageOffset.y - coords.y;
    float dist = sqrt(disX * disX + disY * disY);
    colour.r *= uColor.r + ((1 - dist * 1.1) * uIntensity * 2);
    colour.g *= uColor.r + ((1 - dist*1.1)* uIntensity* 1.4f);
    colour.b *= uColor.r + ((1 - dist * 1.1) * uIntensity* 1);
    float greyEquavelant = (colour.r + colour.g + colour.b) / 3;
    float3 greyEquavelant4 = float3(greyEquavelant, greyEquavelant, greyEquavelant);
    float3 gradientFunction = (greyEquavelant4 - colour.rgb);
    colour.rgb -= gradientFunction/ uOpacity;
    colour *= uColor.g;
   
    return colour;
}

float4 DownPass(float2 coords : TEXCOORD0) : COLOR0
{
    int length = 8;
    float aPixelY = 1 / uScreenResolution.y;
    float4 colour = tex2D(uImage0, coords);
    if (coords.y > 0.6)
    {
        float4 newColours[8];
        for (float i = 0; i < length; i++)
        {
            newColours[i] = tex2D(uImage0, coords + float2(0, aPixelY * (i)));
            if (colour.r - newColours[i].r > 0.001)
            {
                colour += (newColours[i] - colour) * (1 - (i / length));
            }
        }
    }
    return colour;
}
float4 RightPass(float2 coords : TEXCOORD0) : COLOR0
{
    int length = 8;
    float aPixelX = 1 / uScreenResolution.x;
    float4 colour = tex2D(uImage0, coords);
    
    if (coords.y > 0.6)
    {
        float4 newColours[8];
        for (float i = 0; i < length; i++)
        {
            newColours[i] = tex2D(uImage0, coords + float2(aPixelX * (i), 0));
            if (colour.r - newColours[i].r > 0.001)
            {
                colour += (newColours[i] - colour) * (1 - (i / length));
            }
        }
    }
    return colour;
}
float4 DownPass2(float2 coords : TEXCOORD0) : COLOR0
{
    int length = 8;
    float aPixelY = 1 / uScreenResolution.y;
    float4 colour = tex2D(uImage0, coords);
    if (coords.y > 0.6)
    {
        float4 newColours[8];
        for (float i = 0; i < length; i++)
        {
            newColours[i] = tex2D(uImage0, coords + float2(0, aPixelY * (i + 8)));
            if (colour.r - newColours[i].r > 0.001)
            {
                colour += (newColours[i] - colour) * (1 - (i / length));
            }
        }
    }
    return colour;
}
float4 RightPass2(float2 coords : TEXCOORD0) : COLOR0
{
    int length = 8;
    float aPixelX = 1 / uScreenResolution.x;
    float4 colour = tex2D(uImage0, coords);

    if (coords.y > 0.6)
    {
        float4 newColours[8];
        for (float i = 0; i < length; i++)
        {
            newColours[i] = tex2D(uImage0, coords + float2(aPixelX * (i+8), 0));
            if (colour.r - newColours[i].r > 0.001)
            {
                colour += (newColours[i] - colour) * (1 - (i / length));
            }
        }
    }
    return colour;
}
technique Technique1
{
    pass Saturation
    {
        PixelShader = compile ps_2_0 FilterMyShader();
    }
    pass DownPass
    {
        PixelShader = compile ps_2_0 DownPass();
    }
    pass RightPass
    {
        PixelShader = compile ps_2_0 RightPass();
    }
    pass DownPass2
    {
        PixelShader = compile ps_2_0 DownPass2();
    }
    pass RightPass2
    {
        PixelShader = compile ps_2_0 RightPass2();
    }
}