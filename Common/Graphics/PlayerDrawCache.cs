using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Graphics;

[Autoload(Side = ModSide.Client)]
public sealed class PlayerDrawCache : ILoadable
{
    public static RenderTarget2D Texture { get; private set; }

    void ILoadable.Load(Mod mod) {
        Main.QueueMainThreadAction(
            () => {
                Texture = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            }
        );

        On_Main.CheckMonoliths += CheckMonolithsHook;

        Main.OnResolutionChanged += ResolutionChangedHook;
    }

    void ILoadable.Unload() {
        Main.QueueMainThreadAction(
            () => {
                Texture?.Dispose();
                Texture = null;
            }
        );

        Main.OnResolutionChanged -= ResolutionChangedHook;
    }

    private static void CheckMonolithsHook(On_Main.orig_CheckMonoliths orig) {
        orig();

        if (Main.gameMenu) {
            return;
        }

        var device = Main.graphics.GraphicsDevice;

        var spriteBatch = Main.spriteBatch;
        var oldBindings = device.GetRenderTargets();

        device.SetRenderTarget(Texture);
        device.Clear(Color.Transparent);

        spriteBatch.Begin(default, default, Main.DefaultSamplerState, default, Main.Rasterizer, default, Main.GameViewMatrix.TransformationMatrix);

        var player = Main.LocalPlayer;

        if (player.active) {
            Main.PlayerRenderer.DrawPlayer(Main.Camera, player, player.position, player.fullRotation, player.fullRotationOrigin);
        }

        spriteBatch.End();

        device.SetRenderTargets(oldBindings);
    }

    private static void ResolutionChangedHook(Vector2 resolution) {
        Main.QueueMainThreadAction(
            () => {
                Texture?.Dispose();
                Texture = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)resolution.X, (int)resolution.Y);
            }
        );
    }
}
