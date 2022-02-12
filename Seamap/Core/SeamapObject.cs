using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using EEMod.Extensions;
using Terraria.ModLoader;
using System.Diagnostics;
using ReLogic.Content;
using EEMod.Seamap.Core.Components;

namespace EEMod.Seamap.Core
{
    public abstract class SeamapObject : Entity
    {
        public ComponentManager Components { get; }

        public Texture2D texture;

        public float scale = 1f;

        public Color color = Color.White;

        public float rotation = 0f;

        public float alpha = 1f;

        public int[] ai = new int[3];

        public Rectangle rect => new Rectangle((int)position.X, (int)position.Y, width, height);

        public int spriteDirection;

        public virtual bool collides => false;

        protected SeamapObject()
        {
            Components = new ComponentManager(this);
            whoAmI = -1; // it will be assigned to a different value when it spawns
        }

        protected SeamapObject(Vector2 pos, Vector2 vel) : this()
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

        public virtual void UpdateComponents()
        {
            //foreach(Component component in this.components)
            //{
            //
            //}
        }

        /// <summary>
        /// Called before anything draws.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <returns></returns>
        public virtual bool PreDraw(SpriteBatch spriteBatch) => true;

        /// <summary>
        /// Allows for custom draw.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <returns>If this method overwrites default draw.</returns>
        public virtual bool CustomDraw(SpriteBatch spriteBatch) => false;

        public void Draw(SpriteBatch spriteBatch)
        {
            if (PreDraw(spriteBatch))
            {
                if (!CustomDraw(spriteBatch))
                {
                    Main.spriteBatch.Draw(texture, Center.ForDraw(), new Rectangle(0, 0, width, height), color * alpha, rotation, texture.Bounds.Size() / 2, scale, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
                }
            }
        }

        /// <summary>
        /// Called after the draw hooks.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void PostDraw(SpriteBatch spriteBatch)
        {

        }

        /// <summary>
        /// Called after the entity is added to the <see cref="SeamapObjects.SeamapEntities"/> array.
        /// </summary>
        public virtual void OnSpawn()
        {

        }

        /// <summary>
        /// Called when the entity is destroyed
        /// </summary>
        public virtual void OnKill()
        {

        }

        public void Kill()
        {
            SeamapObjects.DestroyObject(this);
        }
    }
}
