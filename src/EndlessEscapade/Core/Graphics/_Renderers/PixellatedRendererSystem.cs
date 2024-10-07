using System.Collections.Generic;

namespace EndlessEscapade.Core.Graphics;

[Autoload(Side = ModSide.Client)]
public sealed class PixellatedRendererSystem : ModSystem
{
	private static readonly List<Action> Actions = [];

	/// <summary>
	///     The render target used for drawing pixellated content.
	/// </summary>
	/// <remarks>
	///     This has half the screen size and is rendered at full
	///     screen size, which results in the pixellated effect.
	/// </remarks>
	public static RenderTarget2D Buffer { get; private set; }

	public override void Load() {
		base.Load();

		Main.QueueMainThreadAction(
			static () => {
				Buffer = new RenderTarget2D(
					Main.graphics.GraphicsDevice,
					Main.screenWidth / 2,
					Main.screenHeight / 2
				);
			}
		);

		On_Main.CheckMonoliths += CheckMonolithsHook;

		On_Main.DrawProjectiles += static (orig, self) => {
			DrawTarget();

			orig(self);
		};

		Main.OnResolutionChanged += ResizeTarget;
	}

	public override void Unload() {
		base.Unload();

		Main.OnResolutionChanged -= ResizeTarget;

		Main.QueueMainThreadAction(
			static () => {
				Buffer?.Dispose();
				Buffer = null;
			}
		);
	}

	/// <summary>
	///     Queues an action to be executed during the next render update.
	/// </summary>
	/// <param name="action">The action to queue.</param>
	public static void Queue(Action action) {
		Actions.Add(action);
	}

	private static void ResizeTarget(Vector2 size) {
		Main.RunOnMainThread(
			() => {
				Buffer?.Dispose();

				Buffer = new(
					Main.graphics.GraphicsDevice,
					(int)(size.X / 2f),
					(int)(size.Y / 2f)
				);
			}
		);
	}

	private static void DrawTarget() {
		if (Buffer?.IsDisposed == true) {
			return;
		}

		Main.spriteBatch.Begin(
			default,
			default,
			Main.DefaultSamplerState,
			default,
			Main.Rasterizer,
			default,
			Main.GameViewMatrix.TransformationMatrix
		);

		Main.spriteBatch.Draw(Buffer, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

		Main.spriteBatch.End();
	}

	private static void CheckMonolithsHook(On_Main.orig_CheckMonoliths orig) {
		orig();

		if (Main.gameMenu) {
			return;
		}

		var device = Main.graphics.GraphicsDevice;

		var bindings = device.GetRenderTargets();

		device.SetRenderTarget(Buffer);
		device.Clear(Color.Transparent);

		Main.spriteBatch.Begin(
			default,
			default,
			Main.DefaultSamplerState,
			default,
			Main.Rasterizer,
			default,
			Matrix.CreateScale(0.5f, 0.5f, 1f)
		);

		foreach (var action in Actions) {
			action?.Invoke();
		}

		Main.spriteBatch.End();

		device.SetRenderTargets(bindings);

		Actions.Clear();
	}
}