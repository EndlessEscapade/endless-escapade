using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using EEMod.Extensions;
using System;
using Terraria.ModLoader;

namespace EEMod.Seamap.SeamapContent
{
    public abstract class SeamapObject : Entity
    {
        public Texture2D texture;

        public float scale = 1f;

        public Color color = Color.White;

        public float rotation = 0f;

        public float alpha = 1f;

        public int whoAmI;

        public int[] ai = new int[3];

        public Rectangle rect => new Rectangle((int)position.X, (int)position.Y, width, height);

        public SeamapObject(Vector2 pos, Vector2 vel)
        {
            position = pos;
            velocity = vel;
        }

        public virtual void Update()
        {
            InternalUpdate();
        }

        private void InternalUpdate()
        {
            position += velocity;
        }

        public virtual bool PreDraw(SpriteBatch spriteBatch) => true;

        public void Draw(SpriteBatch spriteBatch)
        {
            if (PreDraw(spriteBatch))
            {
                Main.spriteBatch.Draw(texture, position.ForDraw(), new Rectangle(0, 0, width, height), color * alpha, rotation, texture.Bounds.Size() / 2, scale, SpriteEffects.None, 0f);
            }
        }

        public virtual void PostDraw(SpriteBatch spriteBatch)
        {

        }

        public virtual void OnSpawn()
        {
            
        }

        public virtual void Kill()
        {
            active = false;
            SeamapObjects.SeamapEntities[whoAmI] = null;
        }
    }
}
