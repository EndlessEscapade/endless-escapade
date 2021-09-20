using EEMod.Extensions;
using EEMod.Systems;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public class PlayerClone : ModSystem
    {
        public RenderTarget2D playerDrawData;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                Main.QueueMainThreadAction(() =>
                {
                    playerDrawData = new RenderTarget2D(Main.graphics.GraphicsDevice, 500, 500);
                });
            }
        }

        public override void PreUpdateEntities()
        {
            RenderTargetBinding[] oldtargets2 = Main.graphics.GraphicsDevice.GetRenderTargets();
            Main.graphics.GraphicsDevice.SetRenderTarget(playerDrawData);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin();
            /*for (int i = 0; i <= Main.playerDrawData.Count; i++)
            {
                int num = -1;
                if (num != 0)
                {
                    Main.pixelShader.CurrentTechnique.Passes[0].Apply();
                    num = 0;
                }

                if (i != Main.playerDrawData.Count)
                {
                    DrawData value = Main.playerDrawData[i];
                    if (value.shader >= 0)
                    {
                        GameShaders.Hair.Apply(0, Main.LocalPlayer, value);
                        GameShaders.Armor.Apply(value.shader, Main.LocalPlayer, value);
                    }
                    else if (Main.LocalPlayer.head == 0)
                    {
                        GameShaders.Hair.Apply(0, Main.LocalPlayer, value);
                        GameShaders.Armor.Apply(Main.LocalPlayer.cHead, Main.LocalPlayer, value);
                    }
                    else
                    {
                        GameShaders.Armor.Apply(0, Main.LocalPlayer, value);
                        GameShaders.Hair.Apply((short)(-value.shader), Main.LocalPlayer, value);
                    }
                    if (!value.sourceRect.HasValue)
                    {
                        value.sourceRect = value.texture.Frame();
                    }
                    num = value.shader;
                    if (value.texture != null)
                    {
                        Main.spriteBatch.Draw(value.texture, value.position - Main.LocalPlayer.position.ForDraw() + playerDrawData.TextureCenter() / 2, value.sourceRect, Color.White, value.rotation, value.origin, value.scale, value.effect, 0f);
                    }
                }
            }*/
            Main.spriteBatch.End();
            Main.graphics.GraphicsDevice.SetRenderTargets(oldtargets2);
        }
    }
}