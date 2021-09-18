using EEMod.Extensions;
using EEMod.Systems;
using EEMod.Systems.Subworlds.EESubworlds;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public class SpiderWebs : Mechanic
    {
        public override void OnDraw(SpriteBatch spriteBatch)
        {
            if (CoralReefs.WebPositions.Count > 0)
            {
                sineInt = Main.GameUpdateCount * 0.003f;
                for (int i = 0; i < CoralReefs.WebPositions.Count; i++)
                {
                    Vector2 pos = CoralReefs.WebPositions[i] * 16;
                    HandleWebDraw(pos);
                }
            }
        }

        public override void OnUpdate()
        {

        }

        public override void OnLoad()
        {
           
        }

        protected override Layer DrawLayering => Layer.BehindTiles;

        float sineInt;
        void HandleWebDraw(Vector2 position)
        {
            Lighting.AddLight(position, new Vector3(0, 0.1f, 0.4f));
            Vector2 tilePos = position / 16;

            int spread = 13;
            int down = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X, (int)tilePos.Y, 1, 50);
            int up = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X, (int)tilePos.Y, -1, 50);
            int down2 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X - spread, (int)tilePos.Y, 1, 50);
            int up2 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X - spread, (int)tilePos.Y, -1, 50);
            int down3 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X + spread, (int)tilePos.Y, 1, 50);
            int up3 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X + spread, (int)tilePos.Y, -1, 50);


            Vector2 p1 = new Vector2(tilePos.X * 16, down * 16);
            Vector2 p1Mid = Helpers.TraverseBezier(p1, position, Vector2.Lerp(p1, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 2) * 40), 0.5f);

            Vector2 p2 = new Vector2(tilePos.X * 16, up * 16);
            Vector2 p2Mid = Helpers.TraverseBezier(p2, position, Vector2.Lerp(p2, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.5f) * 40), 0.5f);

            Vector2 p3 = new Vector2((tilePos.X - spread) * 16, down2 * 16);
            Vector2 p3Mid = Helpers.TraverseBezier(p3, position, Vector2.Lerp(p3, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.2f) * 40), 0.5f);

            Vector2 p4 = new Vector2((tilePos.X - spread) * 16, up2 * 16);
            Vector2 p4Mid = Helpers.TraverseBezier(p4, position, Vector2.Lerp(p4, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.8f) * 40), 0.5f);

            Vector2 p5 = new Vector2((tilePos.X + spread) * 16, down3 * 16);
            Vector2 p5Mid = Helpers.TraverseBezier(p5, position, Vector2.Lerp(p5, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.9f) * 40), 0.5f);

            Vector2 p6 = new Vector2((tilePos.X + spread) * 16, up3 * 16);
            Vector2 p6Mid = Helpers.TraverseBezier(p6, position, Vector2.Lerp(p6, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 2.2f) * 40), 0.5f);


            //Texture2D BlueLight = ModContent.GetInstance<EEMod>().GetTexture("Textures/LightBlue");
            Texture2D vineTexture = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/VineShorter").Value;
            Texture2D bigVineTexture = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/BigVine").Value;

            float cockandbol = 0.75f;

            if (p1.Y >= 1 && p5.Y >= 1)
                Helpers.DrawBezier(vineTexture, Lighting.GetColor((int)(p1Mid.X / 16), (int)(p1Mid.Y / 16)), p1Mid, p5Mid, Vector2.Lerp(p1Mid, p5Mid, 0.5f) + new Vector2(0, -40 + (float)Math.Sin(sineInt * 3) * 40), cockandbol, (float)Math.PI / 2, false, 1, false);

            if (p5.Y >= 1 && p6.Y >= 1)
                Helpers.DrawBezier(vineTexture, Lighting.GetColor((int)(p5Mid.X / 16), (int)(p5Mid.Y / 16)), p5Mid, p6Mid, Vector2.Lerp(p5Mid, p6Mid, 0.5f) + new Vector2(-40 + (float)Math.Sin(sineInt * 4) * 40, 0), cockandbol, (float)Math.PI / 2, false, 1, false);

            if (p6.Y >= 1 && p2.Y >= 1)
                Helpers.DrawBezier(vineTexture, Lighting.GetColor((int)(p6Mid.X / 16), (int)(p6Mid.Y / 16)), p6Mid, p2Mid, Vector2.Lerp(p6Mid, p2Mid, 0.5f) + new Vector2(0, 40 + (float)Math.Sin(sineInt * 3) * 40), cockandbol, (float)Math.PI / 2, false, 1, false);

            if (p2.Y >= 1 && p4.Y >= 1)
                Helpers.DrawBezier(vineTexture, Lighting.GetColor((int)(p2Mid.X / 16), (int)(p2Mid.Y / 16)), p2Mid, p4Mid, Vector2.Lerp(p2Mid, p4Mid, 0.5f) + new Vector2(0, 40 + (float)Math.Sin(sineInt * 4) * 40), cockandbol, (float)Math.PI / 2, false, 1, false);

            if (p4.Y >= 1 && p3.Y >= 1)
                Helpers.DrawBezier(vineTexture, Lighting.GetColor((int)(p4Mid.X / 16), (int)(p4Mid.Y / 16)), p4Mid, p3Mid, Vector2.Lerp(p4Mid, p3Mid, 0.5f) + new Vector2(40 + (float)Math.Sin(sineInt * 3) * 40, 0), cockandbol, (float)Math.PI / 2, false, 1, false);

            if (p3.Y >= 1 && p1.Y >= 1)
                Helpers.DrawBezier(vineTexture, Lighting.GetColor((int)(p3Mid.X / 16), (int)(p3Mid.Y / 16)), p3Mid, p1Mid, Vector2.Lerp(p3Mid, p1Mid, 0.5f) + new Vector2(0, -40 + (float)Math.Sin(sineInt * 4) * 40), cockandbol, (float)Math.PI / 2, false, 1, false);

            if (p1.Y >= 1)
            {
                Helpers.DrawBezier(bigVineTexture, Lighting.GetColor((int)(p1.X / 16), (int)(p1.Y / 16)), p1, position, Vector2.Lerp(p1, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 2) * 40), cockandbol, (float)Math.PI / 2, false, 1, false);
            }
            if (p2.Y >= 1)
            {
                Helpers.DrawBezier(bigVineTexture, Lighting.GetColor((int)(p2.X / 16), (int)(p2.Y / 16)), p2, position, Vector2.Lerp(p2, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.5f) * 40), cockandbol, (float)Math.PI / 2, false, 1, false);
            }
            if (p3.Y >= 1)
            {
                Helpers.DrawBezier(bigVineTexture, Lighting.GetColor((int)(p3.X / 16), (int)(p3.Y / 16)), p3, position, Vector2.Lerp(p3, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.2f) * 40), cockandbol, (float)Math.PI / 2, false, 1, false);
            }
            if (p4.Y >= 1)
            {
                Helpers.DrawBezier(bigVineTexture, Lighting.GetColor((int)(p4.X / 16), (int)(p4.Y / 16)), p4, position, Vector2.Lerp(p4, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.8f) * 40), cockandbol, (float)Math.PI / 2, false, 1, false);
            }
            if (p5.Y >= 1)
            {
                Helpers.DrawBezier(bigVineTexture, Lighting.GetColor((int)(p5.X / 16), (int)(p5.Y / 16)), p5, position, Vector2.Lerp(p5, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 1.9f) * 40), cockandbol, (float)Math.PI / 2, false, 1, false);
            }
            if (p6.Y >= 1)
            {
                Helpers.DrawBezier(bigVineTexture, Lighting.GetColor((int)(p6.X / 16), (int)(p6.Y / 16)), p6, position, Vector2.Lerp(p6, position, 0.5f) + new Vector2(0, 50 + (float)Math.Sin(sineInt * 2.2f) * 40), cockandbol, (float)Math.PI / 2, false, 1, false);
            }
        }
    }
}