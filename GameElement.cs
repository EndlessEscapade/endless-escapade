using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria.ID;

namespace EEMod
{
    public class GameElement
    {
        public Vector2 sizeOfMainCanvas;
        public Vector2 centerOfElement;
        public Color colourOfMainCanvas;
        public EEGame parent;
        public Vector2 velocity;
        public Rectangle elementRect => new Rectangle((int)(UIPosRunTime.X - sizeOfMainCanvas.X / 2),
                                                      (int)(UIPosRunTime.Y - sizeOfMainCanvas.Y / 2),
                                                      (int)sizeOfMainCanvas.X,
                                                      (int)sizeOfMainCanvas.Y);
        public virtual float speedOfStartUp => 16f;
        float colourOfStartUp = 0;
        bool elementActive;
        bool isBoundToMouse = false;
        public virtual void StartElement() => elementActive = true;
        public virtual void EndElement() => elementActive = false;

        public virtual void Initialize()
        {

        }
        public void BindElementToGame(EEGame parent) => this.parent = parent;
        public void BindElementToTexture(Texture2D tex) => this.tex = tex;
        public GameElement(Vector2 size, Color colour, Vector2 Center)
        {
            sizeOfMainCanvas = size;
            colourOfMainCanvas = colour;
            centerOfElement = Center;
            StartElement();
        }

        public void AttachCollisionComponents(bool isSolid, bool collides, bool collidesWithEdges = false, float friction = 1, float bounce = 1f, int bounceBuffer = 30)
        {
            this.isSolid = isSolid;
            this.collides = collides;
            this.friction = friction;
            this.collidesWithEdges = collidesWithEdges;
            this.bounce = bounce;
            this.bounceBuffer = bounceBuffer;
        }
        public virtual void AttatchToMouse(float speed, int playerWhoAmI)
        {
            isBoundToMouse = true;
            SpeedOfMouseBinding = speed;
            this.playerWhoAmI = playerWhoAmI;
        }

        public Vector2 UIPosRunTime;
        public float SpeedOfMouseBinding = 20;
        public bool collides;
        public bool isSolid;
        public bool collidesWithEdges;
        public float friction;
        public float bounce;
        public int bounceBuffer;
        int BBTimer;
        int playerWhoAmI;
        public Vector2 mousePosition;
        public Texture2D tex = Main.magicPixel;
        int yeet;
        public virtual void Update()
        {

            if (isBoundToMouse)
            {
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && i != Main.myPlayer)
                    {
                        yeet = i;
                        break;
                    }
                }
                if (Main.netMode == NetmodeID.MultiplayerClient && playerWhoAmI != 0)
                {
                    MultiplayerMouseTracker.UpdateMyMouse();
                }
                Vector2 chosen = (playerWhoAmI != 0 ? MultiplayerMouseTracker.GetMousePos(yeet) : Main.MouseWorld);
                velocity = (chosen - UIPosRunTime) / SpeedOfMouseBinding;
            }
            if (BBTimer > 0)
            {
                BBTimer--;
            }
            if (elementActive)
                colourOfStartUp += (1 - colourOfStartUp) / speedOfStartUp;
            else
                colourOfStartUp += (-colourOfStartUp) / speedOfStartUp;
            if (parent != null)
            {
                if (collides)
                {
                    foreach (GameElement GE in parent.elementArray)
                    {
                        if (GE != null && GE.isSolid)
                        {
                            if (GE.elementRect.Intersects(elementRect) && BBTimer == 0)
                            {
                                velocity = GE.velocity * bounce;
                                if (Main.netMode == NetmodeID.MultiplayerClient && playerWhoAmI != 0)
                                {
                                    EENet.SendPacket(EEMessageType.MouseCheck, velocity, GE.velocity, bounce);
                                    Main.NewText(MultiplayerMouseTracker.velHolder);
                                }
                               
                                BBTimer = bounceBuffer;
                            }
                        }
                    }
                }
                if (collidesWithEdges)
                {
                    if (UIPosRunTime.Y <= parent.TopLeft.Y + sizeOfMainCanvas.Y / 2 || UIPosRunTime.Y >= parent.BottomLeft.Y - sizeOfMainCanvas.Y / 2)
                    {
                        velocity.Y *= -1;
                    }
                    if (UIPosRunTime.X <= parent.TopLeft.X + sizeOfMainCanvas.X / 2 || UIPosRunTime.X >= parent.TopRight.X - sizeOfMainCanvas.X / 2)
                    {
                        velocity.X *= -1;
                    }
                }
                velocity *= friction;
                UIPosRunTime += velocity + Main.LocalPlayer.velocity;
                Helpers.Clamp(ref UIPosRunTime.X, parent.TopLeft.X + sizeOfMainCanvas.X / 2, parent.TopRight.X - sizeOfMainCanvas.X / 2);
                Helpers.Clamp(ref UIPosRunTime.Y, parent.TopLeft.Y + sizeOfMainCanvas.Y / 2, parent.BottomLeft.Y - sizeOfMainCanvas.Y / 2);
            }

            Main.spriteBatch.Draw(tex, UIPosRunTime - Main.screenPosition, new Rectangle(0, 0, (int)sizeOfMainCanvas.X, (int)sizeOfMainCanvas.Y), colourOfMainCanvas * colourOfStartUp, 0f, new Rectangle(0, 0, (int)sizeOfMainCanvas.X, (int)sizeOfMainCanvas.Y).Size() / 2, 1, SpriteEffects.None, 0f);
        }
    }
}