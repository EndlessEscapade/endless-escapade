namespace EndlessEscapade.Core.Graphics;

public readonly struct SpriteBatchSnapshot(
	SpriteSortMode spriteSortMode,
	BlendState blendState,
	SamplerState samplerState,
	DepthStencilState depthStencilState,
	RasterizerState rasterizerState,
	Effect effect,
	Matrix transformMatrix
)
{
	public readonly SpriteSortMode SpriteSortMode = spriteSortMode;

	public readonly BlendState BlendState = blendState;

	public readonly SamplerState SamplerState = samplerState;

	public readonly DepthStencilState DepthStencilState = depthStencilState;

	public readonly RasterizerState RasterizerState = rasterizerState;

	public readonly Effect Effect = effect;

	public readonly Matrix TransformMatrix = transformMatrix;
}
