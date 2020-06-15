using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.NPCs.Bosses.Akumo
{
    public class AkumoSky : CustomSky
    {
        // Thanks OS for code!
        private bool isActive;

        private float intensity;

        private int akumoIndex = -1;

        public override void Update(GameTime gameTime)
        {
            if (isActive && intensity < 1f)
            {
                intensity += 0.01f;
                return;
            }
            if (!isActive && intensity > 0f)
            {
                intensity -= 0.01f;
            }
        }

        private float GetIntensity()
        {
            if (UpdateAkumoIndex())
            {
                float x = 0f;
                if (akumoIndex != -1)
                {
                    x = Vector2.Distance(Main.LocalPlayer.Center, Main.npc[akumoIndex].Center);
                }
                return 1f - Utils.SmoothStep(6000f, 6000f, x);
            }
            return 0f;
        }

        public override Color OnTileColor(Color inColor)
        {
            float num = GetIntensity();
            return new Color(Vector4.Lerp(new Vector4(1.0f, 0.0f, 0.0f, 1f), inColor.ToVector4(), 1f - num));
        }

        private bool UpdateAkumoIndex()
        {
            int num = ModContent.NPCType<Akumo>();
            if (akumoIndex >= 0 && Main.npc[akumoIndex].active && Main.npc[akumoIndex].type == num)
            {
                return true;
            }
            akumoIndex = Helpers.FirstNPCIndex(num);
            return akumoIndex != -1; //Helpers.FirstNPCIndex(num) != -1; //akumoIndex != -1;
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 0f && minDepth < 0f)
            {
                float scale = GetIntensity();
                spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Maroon * scale);
            }
        }

        public override float GetCloudAlpha()
        {
            return 0f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
        }

        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }

        public override void Reset()
        {
            isActive = false;
        }

        public override bool IsActive()
        {
            return isActive || intensity > 0f;
        }
    }
}
