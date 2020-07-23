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
    float4 colour2 = tex2D(uImage0, coords + float2(1/ uScreenResolution.x,0));
    float4 colour3 = tex2D(uImage0, coords - float2(0, 1 / uScreenResolution.y));
    float4 currentColour;
    float distance = sqrt(pow(coords.x - uDirection.x, 2) + pow(coords.y - uDirection.y, 2));
    float4 originColour = tex2D(uImage0, uDirection);
    float difference = abs(colour3.r - colour.r) + abs(colour3.b - colour.b) + abs(colour3.g - colour.g);
    float difference2 = abs(colour2.r - colour.r) + abs(colour2.b - colour.b) + abs(colour2.g - colour.g);
        if (difference2 > .3f || difference > .3f)
        {
            colour *= 0;
        }
        for (float i = 0; i < 6; i++)
        {
           // colour = tex2D(uImage0, coords - float2(-1/ uScreenResolution.x, 1 / uScreenResolution.y));
            if (tex2D(uImage0, coords - float2((-1 / uScreenResolution.x) * i, (1 / uScreenResolution.y) * i)).g <= .01)
            {
                 colour *= i * 0.16;
            }
        }
    return colour;
}
float4 SecondPass(float2 coords : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(uImage0, coords);
    float4 colour2 = tex2D(uImage0, coords + float2(1 / uScreenResolution.x, 0));
    for (float i = 0; i < 6; i++)
    {
        colour = tex2D(uImage0, coords - float2((-1 / uScreenResolution.x) * i, (1 / uScreenResolution.y) * i));
        if (tex2D(uImage0, coords - float2((-1/ uScreenResolution.x) * i, (1 / uScreenResolution.y) * i)).g <= .01)
        {
            colour *= i * 0.16;
        }
    }
    return colour;
}
technique Technique1
{
    pass WhiteFlash
    {
        PixelShader = compile ps_2_0 FilterMyShader();
        FillMode = Solid;
    }
    pass SecondPass
    {
        PixelShader = compile ps_2_0 SecondPass();
        FillMode = Solid;
    }

}