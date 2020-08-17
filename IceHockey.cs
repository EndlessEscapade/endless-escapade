using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.Graphics.Effects;

namespace EEMod
{
    public class IceHockey : EEGame
    {
        public override Texture2D tex => TextureCache.AHT;
        public override Vector2 sizeOfMainCanvas => new Vector2(1000, 400);
        public override Vector2 centerOfMainCanvas => Main.LocalPlayer.Center;
        public override Color colourOfMainCanvas => Color.White;
        public override float speedOfStartUp => 16f;
        float pauseShaderTImer;
        public override void Initialize()
        {
            for (int i = 0; i < Main.ActivePlayersCount; i++)
            {
                    int puck = AddUIElement(new Vector2(30, 30), Color.White,centerOfMainCanvas + new Vector2(-300 + (i*600),0));
                    elementArray[puck].AttatchToMouse(16f, i);
                    elementArray[puck].BindElementToGame(this);
                    elementArray[puck].AttachCollisionComponents(true, false, false);
                if(i == 0)
                elementArray[puck].BindElementToTexture(TextureCache.BluePuck);
                if(i == 1)
                elementArray[puck].BindElementToTexture(TextureCache.RedPuck);
            }
            int ball = AddUIElement(new Vector2(30, 30), Color.White, centerOfMainCanvas);

            elementArray[ball].BindElementToGame(this);
            elementArray[ball].BindElementToTexture(TextureCache.Puck);
            elementArray[ball].AttachCollisionComponents(false, true, true, 0.97f, 2f);
            elementArray[ball].AttachTag("ball");
        }
        public override void OnDeactivate()
        {
            if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:Pause"].IsActive())
            {
                Filters.Scene.Deactivate("EEMod:Pause");
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:Pause"].IsActive())
            {
                Filters.Scene.Activate("EEMod:Pause").GetShader().UseOpacity(pauseShaderTImer);
            }
            Filters.Scene["EEMod:Pause"].GetShader().UseOpacity(pauseShaderTImer);
            pauseShaderTImer += 50;
            if (pauseShaderTImer > 1000)
            {
                pauseShaderTImer = 1000;
            }
            base.Update(gameTime);
        }
    }
}
