using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using EEMod.Seamap.SeamapContent;
using Terraria;

namespace EEMod.Seamap.SeamapAssets
{
    public class Crate : SeamapObject
    {
        public override void OnSpawn()
        {
            texture = ModContent.GetTexture("EEMod/Seamap/SeamapAssets/Crate");

            width = 16;
            height = 18;
        }

        public Crate(Vector2 pos, Vector2 vel) : base(pos, vel) { }

        //public bool sinking = false;

        public override void Update()
        {
            base.Update();
            velocity = new Vector2(0.5f, 0);
            /*if (!sinking)
            {
                velocity = new Vector2(0.5f, 0);
            }
            else
            {
                Sink();
            }*/
            //velocity = new Vector2(0.5f, 0);
        }

        /*private int sinkTimer = 32;

        public void Sink()
        {
            velocity.X = 0;
            velocity.Y = 0.5f;
            alpha += 8;
            sinkTimer--;

            if (sinkTimer <= 0)
            {
                //Kill();
            }
        }*/
    }
}
