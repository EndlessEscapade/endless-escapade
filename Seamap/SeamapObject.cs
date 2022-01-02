using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using EEMod.Extensions;
using System;
using Terraria.ModLoader;
using System.Diagnostics;

namespace EEMod.Seamap.SeamapContent
{
    public abstract class SeamapObject : Entity, ISeamapEntity
    {
        public Texture2D texture;

        public float scale = 1f;

        public Color color = Color.White;

        public float rotation = 0f;

        public float alpha = 1f;

        public int[] ai = new int[3];

        public Rectangle rect => new Rectangle((int)position.X, (int)position.Y, width, height);

        public int spriteDirection;

        public bool collides = false;

        public SeamapObject(Vector2 pos, Vector2 vel)
        {
            position = pos;
            velocity = vel;
        }

        public virtual void Update()
        {
            oldPosition = position;
            oldVelocity = velocity;

            position += velocity;
        }

        public virtual bool PreDraw(SpriteBatch spriteBatch) => true;

        public virtual bool CustomDraw(SpriteBatch spriteBatch) => false;

        public void Draw(SpriteBatch spriteBatch)
        {
            if (PreDraw(spriteBatch))
            {
                if (!CustomDraw(spriteBatch))
                {
                    Main.spriteBatch.Draw(texture, position.ForDraw(), new Rectangle(0, 0, width, height), color * alpha, rotation, texture.Bounds.Size() / 2, scale, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
                }
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
