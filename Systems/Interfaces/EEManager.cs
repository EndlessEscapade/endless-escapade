using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public class ComponentManager<T> where T : Entity, IComponent
    {
        List<T> Objects = new List<T>();
        public void Update()
        {
            foreach (T TV in Objects)
            {
                TV.Update();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (T TV in Objects)
            {
                TV.Draw(spriteBatch);
            }
        }
        public void Clear() => Objects.Clear();
        public void AddElement(T TV)
        {
            foreach (T TOV in Objects)
            {
                if (TOV.position == TV.position)
                    return;
            }
            Objects.Add(TV);
        }
    }

}
