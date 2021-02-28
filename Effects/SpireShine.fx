sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float alpha;
float shineSpeed;
texture tentacle;
float shaderLerp;
float3 lightColor;

sampler tent = sampler_state
{
	Texture = (tentacle);
};

float4 White(float2 coords : TEXCOORD0) : COLOR0
{
	float4 colour = tex2D(uImage0, coords);

	float4 colour2 = tex2D(tent, coords);
	float pos = alpha - coords.y;
	float4 white = float4(1, 1, 1, 0.7f);

	if (colour.a > 0)
	{
		float clamper = clamp(0.3f - distance(alpha * shineSpeed, coords.y), 0, 1.3) * colour2.r;
		colour.rgb = lerp(colour, white, clamper);
		colour.rgb *= shaderLerp;
	}

	colour.rgb *= lightColor.rgb;
	return colour;
}

technique BasicColorDrawing
{
	pass SpireHeartbeat
	{
		PixelShader = compile ps_2_0 White();
	}
};