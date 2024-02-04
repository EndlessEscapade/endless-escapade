using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace EndlessEscapade.Common.Seamap;

public abstract class SeamapObject : Entity
{
    public int[] ai = new int[3];

    public float alpha = 1f;

    public Color color = Color.White;

    public bool netUpdate = true;

    public float rotation = 0f;

    public float scale = 1f;

    public int spriteDirection;

    public Texture2D texture;

    protected SeamapObject() {
        Components = new ComponentManager(this);
        whoAmI = -1; // it will be assigned to a different value when it spawns
    }

    protected SeamapObject(Vector2 pos, Vector2 vel) : this() {
        position = pos;
        velocity = vel;
    }

    public ComponentManager Components { get; }

    public Rectangle rect => new((int)position.X, (int)position.Y, width, height);

    public virtual bool collides => false;

    public virtual void Update() {
        oldPosition = position;
        oldVelocity = velocity;

        position += velocity;
    }

    public virtual void UpdateComponents() {
        //foreach(Component component in this.components)
        //{
        //
        //}
    }

    /// <summary>
    ///     Called before anything draws.
    /// </summary>
    /// <param name="spriteBatch"></param>
    /// <returns></returns>
    public virtual bool PreDraw(SpriteBatch spriteBatch) {
        return true;
    }

    /// <summary>
    ///     Allows for custom draw.
    /// </summary>
    /// <param name="spriteBatch"></param>
    /// <returns>If this method overwrites default draw.</returns>
    public virtual bool CustomDraw(SpriteBatch spriteBatch) {
        return false;
    }

    public void Draw(SpriteBatch spriteBatch) {
        if (PreDraw(spriteBatch)) {
            if (!CustomDraw(spriteBatch)) {
                Main.spriteBatch.Draw(texture,
                    Center - Main.screenPosition,
                    new Rectangle(0, 0, width, height),
                    color * alpha,
                    rotation,
                    texture.Bounds.Size() / 2,
                    scale,
                    spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    0f);
            }
        }
    }

    /// <summary>
    ///     Called after the draw hooks.
    /// </summary>
    /// <param name="spriteBatch"></param>
    public virtual void PostDraw(SpriteBatch spriteBatch) { }

    /// <summary>
    ///     Called after the entity is added to the <see cref="SeamapObjects.SeamapEntities" /> array.
    /// </summary>
    public virtual void OnSpawn() { }

    /// <summary>
    ///     Called when the entity is destroyed
    /// </summary>
    public virtual void OnKill() { }

    public void Kill() {
        OnKill();

        SeamapObjects.DestroyObject(this);
    }

    public virtual bool CheckCollision(Rectangle hitbox) {
        if (hitbox.Intersects(Hitbox)) {
            return true;
        }

        return false;
    }
}
