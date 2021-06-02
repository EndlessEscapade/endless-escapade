using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;

namespace EEMod.Seamap.SeamapContent
{
    class Island : SeamapObject
    {
        //Vector2 position;
        //float width;
        //float height;

        string name;
        Texture2D texture;
        int framecount;
        int framespid;
        bool cancollide;
        int framecounter;
        int frame;
        public Vector2 posToScreen => position - Main.screenPosition;
        public bool isColliding => IsCollidingWith(SeamapPlayerShip.localship);
        public bool IsCollidingWith(SeamapPlayerShip playership) => new Rectangle((int)playership.position.X, (int)playership.position.Y, (int)playership.width, (int)playership.height).Intersects(new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height));

        public Island(Vector2 position, Texture2D texture, string name, int framecount = 1, int framespid = 2, bool cancollide = false) : this(position, texture.Width, texture.Height / framecount, texture, name, framecount, framespid, cancollide)
        {
            if (name != null)
                if (!SeamapObjects.IslandsDict.ContainsKey(name))
                    SeamapObjects.IslandsDict.Add(name, this);
        }

        public Island(Vector2 position, int width, int height, Texture2D texture, string islandname, int framecount, int framespid, bool cancollide): base(position, Vector2.Zero)
        {
            this.position = position;
            this.width = width;
            this.height = height;
            this.texture = texture;
            this.name = islandname;
            this.framecount = framecount;
            this.framespid = framespid;
            this.cancollide = cancollide;
        }

        public void Update()
        {
            if(++framecounter > framespid)
            {
                framecounter = 0;
                if(++frame > framecount-1)
                {
                    frame = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 currentPos = position.ForDraw();
            Color drawColour = Lighting.GetColor((int)(position.X / 16f), (int)(position.Y / 16f)) * Main.LocalPlayer.GetModPlayer<EEPlayer>().seamapLightColor;
            drawColour.A = 255;
            spriteBatch.Draw(texture, position.ForDraw(), new Rectangle(0, texture.Height / framecount * frame, texture.Width, texture.Height / framecount), Color.White);
        }
    }
}
