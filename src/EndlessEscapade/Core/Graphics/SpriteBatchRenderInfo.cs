using ReLogic.Content;

namespace EndlessEscapade.Core.Graphics;

public struct SpriteBatchRenderInfo
{
	/// <summary>
	///		The texture used for rendering by this structure.
	/// </summary>
	public Asset<Texture2D> Texture;

	/// <summary>
	///		The origin used for rendering by this structure.
	/// </summary>
	public Vector2 Origin;

	/// <summary>
	///		The color used for rendering by this structure.
	/// </summary>
	public Color Color {
		get => color * Opacity;
		set => color = value;
	}

	private Color color;

	public float Opacity {
		get => opacity;
		set => opacity = MathHelper.Clamp(value, 0f, 1f);
	}

	private float opacity;

	/// <summary>
	///		The source rectangle used for rendering by this structure.
	/// </summary>
	public Rectangle? SourceRectangle;

	/// <summary>
	///		The destination rectangle used for rendering by this structure.
	/// </summary>
	public Rectangle? DestinationRectangle;

	/// <summary>
	///		The sprite effects used for rendering by this structure.
	/// </summary>
	public SpriteEffects Effects;

	public SpriteBatchRenderInfo(
		Asset<Texture2D> texture,
		Color color,
		Rectangle? sourceRectangle = null,
		Vector2 origin = default,
		SpriteEffects effects = SpriteEffects.None
	) {
		Texture = texture;
		Color = color;
		SourceRectangle = sourceRectangle;
		Origin = origin;
		Effects = effects;
	}

	public SpriteBatchRenderInfo(
		Asset<Texture2D> texture,
		Color color,
		Rectangle destinationRectangle,
		Rectangle? sourceRectangle = null,
		SpriteEffects effects = SpriteEffects.None) {
		Texture = texture;
		Color = color;
		DestinationRectangle = destinationRectangle;
		SourceRectangle = sourceRectangle;
		Effects = effects;
	}
}
