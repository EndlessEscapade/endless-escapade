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
float4 A(float2 coords : TEXCOORD0, uniform int passNum) : COLOR0
{
    float trailLength = 4;
    float4 colour = tex2D(uImage0, coords);
    float aPixelX = 1 / uScreenResolution.x;
    float aPixelY = 1 / uScreenResolution.y;
    float distance = sqrt(pow(coords.x - 1, 2) + pow(coords.y, 2));
    float2 extra = float2(aPixelX * (trailLength * passNum), -aPixelY * (trailLength * passNum)) ;
    float mult2 = 1 + ((0.3 - distance) * 0.1);
        for (float i = 0; i < trailLength; i++)
        {
            float2 base = coords - float2((-aPixelX) * i, aPixelY * i);
            float2 offsetBase = coords - float2(((-aPixelX) * i) + aPixelX, aPixelY * i);
            float difference = abs(tex2D(uImage0, base + extra).rgb - tex2D(uImage0, offsetBase + extra).rgb);
            if (difference > .4f)
            {
               // float mult = ((float)(i + (passNum * trailLength)) / (float)(uOpacity * trailLength)) + uIntensity;
                colour *= (1 - (uIntensity * 0.01));
            }
            else 
            {
                if(mult2 > 1)
                colour *= mult2;
            }
        }
    return colour;
}

technique Technique1
{
    pass Filter1 { PixelShader = compile ps_2_0 A(0); }
}