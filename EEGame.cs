using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.Graphics.Effects;

namespace EEMod
{
    public class EEGame
    {
        public GameElement[] elementArray = new GameElement[200];
        public virtual Vector2 sizeOfMainCanvas => Vector2.Zero;
        public virtual Vector2 centerOfMainCanvas => Main.LocalPlayer.Center;
        public virtual Color colourOfMainCanvas => Color.White;
        public virtual float speedOfStartUp => 16f;

        public Vector2 TopLeft => centerOfMainCanvas + new Vector2(-sizeOfMainCanvas.X / 2, -sizeOfMainCanvas.Y / 2);
        public Vector2 TopRight => centerOfMainCanvas + new Vector2(sizeOfMainCanvas.X / 2, -sizeOfMainCanvas.Y / 2);
        public Vector2 BottomLeft => centerOfMainCanvas + new Vector2(-sizeOfMainCanvas.X / 2, sizeOfMainCanvas.Y / 2);
        public Vector2 BottomRight => centerOfMainCanvas + new Vector2(sizeOfMainCanvas.X / 2, sizeOfMainCanvas.Y / 2);

        public float colourOfStartUp = 0;
        public bool gameActive;
        public EEGame()
        {
            Initialize();
        }
        public virtual void Initialize()
        {

        }
        public virtual void OnDeactivate()
        {

        }
        public virtual void StartGame() => gameActive = true;
        public virtual void EndGame() => gameActive = false;
        public virtual int AddUIElement(Vector2 size, Color color, Vector2 Center)
        {
            for (int i = 0; i < elementArray.Length; i++)
            {
                if (elementArray[i] == null)
                {
                    elementArray[i] = new GameElement(size, color, Center);
                    return i;
                    break;
                }
            }
            return 0;
        }
        public virtual void Update()
        {
            if (gameActive)
            {
                Main.spriteBatch.Draw(Main.magicPixel, centerOfMainCanvas - Main.screenPosition, new Rectangle(0, 0, (int)sizeOfMainCanvas.X, (int)sizeOfMainCanvas.Y), colourOfMainCanvas * colourOfStartUp, 0f, new Rectangle(0, 0, (int)sizeOfMainCanvas.X, (int)sizeOfMainCanvas.Y).Size() / 2, 1, SpriteEffects.None, 0f);
                colourOfStartUp += (1 - colourOfStartUp) / speedOfStartUp;
                foreach (GameElement GE in elementArray)
                {
                    if (GE != null)
                    {
                        GE.Update();
                    }
                }
            }
            else
            {
                OnDeactivate();
                Main.spriteBatch.Draw(Main.magicPixel, centerOfMainCanvas - Main.screenPosition, new Rectangle(0, 0, (int)sizeOfMainCanvas.X, (int)sizeOfMainCanvas.Y), colourOfMainCanvas * colourOfStartUp, 0f, new Rectangle(0, 0, (int)sizeOfMainCanvas.X, (int)sizeOfMainCanvas.Y).Size() / 2, 1, SpriteEffects.None, 0f);
                colourOfStartUp += (-colourOfStartUp) / speedOfStartUp;
            }
        }
    }
}
