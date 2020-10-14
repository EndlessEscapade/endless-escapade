using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace EEMod.Skies
{
    public class SavingSky : CustomSky
    {
        public bool Active;
        public float Intensity;

        public override void OnLoad()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override Color OnTileColor(Color inColor)
        {
            return new Color(255, 255, 255);
        }

        private Texture2D texture;
        private Texture2D texture2;
        private Rectangle frame;
        private int Countur;
        private int frames;
        private int frameSpeed;

        public void DrawSky()
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    texture2 = EEMod.instance.GetTexture("LoadingScreenImages/LoadingScreen1");
                    break;
                case 1:
                    texture2 = EEMod.instance.GetTexture("LoadingScreenImages/LoadingScreen2");
                    break;
                case 2:
                    texture2 = EEMod.instance.GetTexture("LoadingScreenImages/LoadingScreen3");
                    break;
                default:
                    texture2 = EEMod.instance.GetTexture("LoadingScreenImages/LoadingScreen1");
                    break;
            }
            switch (EEMod.loadingChooseImage)
            {
                case 0:
                {
                    texture = EEMod.instance.GetTexture("NPCs/DuneShambler");
                    frames = 6;
                    frameSpeed = 15;
                    break;
                }

                case 1:
                {
                    texture = EEMod.instance.GetTexture("LoadingScreenImages/GiantSquid");
                    frames = 3;
                    frameSpeed = 30;
                    break;
                }
                case 2:
                {
                    texture = EEMod.instance.GetTexture("LoadingScreenImages/Clam");
                    frames = 3;
                    frameSpeed = 30;
                    break;
                }
                case 3:
                {
                    texture = EEMod.instance.GetTexture("LoadingScreenImages/Hydros");
                    frames = 8;
                    frameSpeed = 25;
                    break;
                }
                case 4:
                {
                    texture = EEMod.instance.GetTexture("LoadingScreenImages/Seahorse");
                    frames = 5;
                    frameSpeed = 20;
                    break;
                }
            }
            if (Countur++ > frameSpeed)
            {
                Countur = 0;
                frame.Y += texture.Height / frames;
            }
            if (frame.Y >= texture.Height / frames * (frames - 1))
            {
                frame.Y = 0;
            }
            Vector2 position = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 + 30);
            Main.spriteBatch.Draw(texture2, Vector2.Zero, new Color(204, 204, 204));
            Main.spriteBatch.Draw(texture, position, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames), new Color(15, 15, 15), 0, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames).Size() / 2, 1, SpriteEffects.None, 0);
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    texture2 = EEMod.instance.GetTexture("LoadingScreenImages/LoadingScreen1");
                    break;
                case 1:
                    texture2 = EEMod.instance.GetTexture("LoadingScreenImages/LoadingScreen2");
                    break;
                case 2:
                    texture2 = EEMod.instance.GetTexture("LoadingScreenImages/LoadingScreen3");
                    break;
                default:
                    texture2 = EEMod.instance.GetTexture("LoadingScreenImages/LoadingScreen1");
                    break;
            }
            switch (EEMod.loadingChooseImage)
            {
                case 0:
                {
                    texture = EEMod.instance.GetTexture("NPCs/CoralReefs/SeaDragon");
                    frames = 4;
                    frameSpeed = 10;
                    break;
                }

                case 1:
                {
                    texture = EEMod.instance.GetTexture("NPCs/CoralReefs/Grebyser");
                    frames = 3;
                    frameSpeed = 15;
                    break;
                }
                case 2:
                {
                    texture = EEMod.instance.GetTexture("NPCs/CoralReefs/CrescentJelly");
                    frames = 9;
                    frameSpeed = 5;
                    break;
                }
                case 3:
                {
                    texture = EEMod.instance.GetTexture("NPCs/CoralReefs/GlisteningReefs/Lionfish");
                    frames = 8;
                    frameSpeed = 10;
                    break;
                }
                case 4:
                {
                    texture = EEMod.instance.GetTexture("NPCs/CoralReefs/MechanicalReefs/MechanicalShark");
                    frames = 6;
                    frameSpeed = 10;
                    break;
                }
            }
            if (Countur++ > frameSpeed)
            {
                Countur = 0;
                frame.Y += texture.Height / frames;
            }
            if (frame.Y >= texture.Height / frames * (frames - 1))
            {
                frame.Y = 0;
            }
            Vector2 position = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 + 30);
            Main.spriteBatch.Draw(texture2, new Rectangle(Main.screenWidth/2, Main.screenHeight/2, texture2.Width, texture2.Height), texture2.Bounds, new Color(204, 204, 204), 0, origin: new Vector2(texture2.Width/2, texture2.Height/2), SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture, position, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames), new Color(0, 0, 0), 0, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames).Size() / 2, 1, SpriteEffects.None, 0);
        }

        public override float GetCloudAlpha()
        {
            return 0f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
        }

        public override void Deactivate(params object[] args)
        {
        }

        public override void Reset()
        {
        }

        public override bool IsActive()
        {
            return EEMod.isSaving;
        }
    }

    public class SavingSkyData : ScreenShaderData
    {
        public SavingSkyData(string passName) : base(passName)
        {
        }

        private void UpdateSavingSky()
        {
        }

        public override void Apply()
        {
            if (EEMod.isSaving)
            {
                UpdateSavingSky();
                base.Apply();
            }
        }
    }
}