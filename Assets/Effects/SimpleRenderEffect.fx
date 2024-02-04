#pragma warning( disable : 4717 ) 

// This file contains a simple effect that can be used for rendering
// the effect expects at least VertexPositionColorTexture


// the 3 parameters here are REQUIRED
float2 uScreenPosition;
float4x4 view; // Main.GameViewMatrix.TransformationMatrix
float4x4 projection;

sampler2D texture0 : register(s0);

float4 mainvs(in float4 inPosition : POSITION0) : SV_Position
{
    return float4(mul(mul(inPosition - float4(uScreenPosition.xy, 0, 0), view), projection));
}

float4 mainps(in float4 color : COLOR0, in float2 texCoords : TEXCOORD0) : COLOR0
{
    return tex2D(texture0, texCoords) * color;
}

technique DefaultTechnique
{
    pass DefaultPass
    {
        VertexShader = compile vs_2_0 mainvs();
        PixelShader = compile ps_2_0 mainps();
    }
    pass VertexShaderOnly
    {
        VertexShader = compile vs_2_0 mainvs();
    }
    pass PixelShaderOnly
    {
        PixelShader = compile ps_2_0 mainps();
    }
}