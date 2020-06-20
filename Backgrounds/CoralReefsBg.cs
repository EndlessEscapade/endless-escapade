using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using EEMod.Backgrounds;
using Microsoft.Xna.Framework;

namespace EEMod.Backgrounds
{
    public class Surfacebg : ModSurfaceBgStyle
    {
        public override bool ChooseBgStyle()
        {
            return !Main.gameMenu && Main.LocalPlayer.GetModPlayer<EEPlayer>().ZoneCoralReefs;
        }
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            for (int i = 0; i < fades.Length; i++)
            {
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                    if (fades[i] > 1f)
                    {
                        fades[i] = 1f;
                    }
                }
                else
                {
                    fades[i] -= transitionSpeed;
                    if (fades[i] < 0f)
                    {
                        fades[i] = 0f;
                    }
                }
            }
        }
        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            return mod.GetBackgroundSlot("Backgrounds/CoralReefsSurfaceClose");
        }
        public override int ChooseMiddleTexture()
        {
            return mod.GetBackgroundSlot("Backgrounds/CoralReefsSurfaceMid");      
        }
        public override int ChooseFarTexture()
        {
            return mod.GetBackgroundSlot("Backgrounds/CoralReefsSurfaceFar");
        }

       /* public override bool PreDrawCloseBackground(SpriteBatch spriteBatch)
        {
            Texture2D texture = ModContent.GetTexture("EEMod/Backgrounds/CoralReefsSurfaceClose");
            spriteBatch.Draw(texture, Vector2.Zero, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0, new Rectangle(0, 0, texture.Width, texture.Height).Size() / 2, 1, SpriteEffects.None, 0);
            return true;
        } */
    }
}
