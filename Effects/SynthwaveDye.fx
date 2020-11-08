sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;

float4 SynthwaveDyeShader(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 colour = tex2D(uImage0, coords);
	float4 ecksdee = lerp(float4(1, 0, 0, 1), float4(0, 0, 1, 1), sin((uTime + coords.x)/1.5f));
	return (((colour) + ecksdee) / 2) * colour.a;
}

technique SynthwaveDyeTechnique
{
	pass SynthwaveDyeShader
	{
		PixelShader = compile ps_2_0 SynthwaveDyeShader();
	}
}