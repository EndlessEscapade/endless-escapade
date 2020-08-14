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
        public override Vector2 sizeOfMainCanvas => new Vector2(500,500);
        public override Vector2 centerOfMainCanvas => Main.LocalPlayer.Center;
        public override Color colourOfMainCanvas => Color.White;
        public override float speedOfStartUp => 16f;
        float pauseShaderTImer;
        public override void Initialize()
        {
            int puck = AddUIElement(new Vector2(50,50),Color.Black, centerOfMainCanvas);
            int ball = AddUIElement(new Vector2(20, 20), Color.Red, centerOfMainCanvas);
            elementArray[puck].AttatchToMouse(16f);
            elementArray[puck].BindElementToGame(this);
            elementArray[puck].AttachCollisionComponents(true,false,false);
            elementArray[ball].BindElementToGame(this);
            elementArray[ball].AttachCollisionComponents(false, true,true,0.97f,2f);
           
        }
        public override void OnDeactivate()
        {
            if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:Pause"].IsActive())
            {
                Filters.Scene.Deactivate("EEMod:Pause");
            }
        }
        public override void Update()
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
            base.Update();
        }
    }
}
