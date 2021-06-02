using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Seamap.SeamapContent
{
    internal interface ISeamapEntity
    {
        void Update();

        void Draw(SpriteBatch spriteBatch);
    }
}
