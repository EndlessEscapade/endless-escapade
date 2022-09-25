sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

float2 screenPosition;
float2 screenSize;
float2 texSize;

float alpha;

texture buffer;
sampler light = sampler_state
{
	Texture = (buffer);
};

float4 White(float2 coords : TEXCOORD0) : COLOR0
{
	float2 screenPercentage = float2(screenPosition.x / screenSize.x, screenPosition.y / screenSize.y);
	float4 texColor = tex2D(uImage0, coords);

	float percX = texSize.x / screenSize.x;
	float percY = texSize.y / screenSize.y;
	
	float2 alteredCoords = float2(coords.x * percX + screenPercentage.x, coords.y * percY + screenPercentage.y);
	float4 lightColor = tex2D(light, alteredCoords);

	float4 finalColor = texColor * lightColor;

	return float4(finalColor.r, finalColor.g, finalColor.b, alpha);
}

technique BasicColorDrawing
{
	pass LightingBuffer
	{
		PixelShader = compile ps_2_0 White();
	}
};