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
    float step = 300000;
    float4 colour = tex2D(uImage0, coords);
    float4 colour2 = tex2D(uImage0, coords + float2(sin(uOpacity)/((step /uOpacity)+10), cos(uOpacity)/ ((step / uOpacity) + 10)));
    float4 colour3 = tex2D(uImage0, coords - float2(sin(uOpacity)/ ((step / uOpacity) + 10), cos(uOpacity) / ((step / uOpacity) + 10)));
    float4 colour4 = tex2D(uImage0, coords + float2(-sin(uOpacity) / ((step / uOpacity) + 10),cos(uOpacity)/((step /uOpacity)+10)));
    float4 colour5 = tex2D(uImage0, coords + float2(sin(uOpacity) / ((step / uOpacity) + 10), -cos(uOpacity) / ((step / uOpacity) + 10)));
    double distance = sqrt(pow(coords.x - 0.5, 2) + pow(coords.y - 0.5, 2));
    float4 total = colour + colour2 + colour3 + colour4 + colour5;
    float4 average = (total / 5);
    colour = average;
    float greyEquavelant = (colour.r + colour.g + colour.b) / 3;
    float3 greyEquavelant4 = float3(greyEquavelant, greyEquavelant, greyEquavelant);
    float3 gradientFunction = (greyEquavelant4 - colour.rgb) / (1000/ uOpacity);
    colour.rgb += gradientFunction;
    colour *= (1.3f-(uOpacity/1000)) - distance;
    return colour;
}

technique Technique1
{
    pass P1
    {
        PixelShader = compile ps_2_0 FilterMyShader();
    }
}