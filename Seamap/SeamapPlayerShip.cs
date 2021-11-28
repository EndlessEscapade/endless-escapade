using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.ID;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.ID;
using EEMod.Seamap.SeamapAssets;

namespace EEMod.Seamap.SeamapContent
{
    public class EEPlayerShip : SeamapObject
    {
        public static readonly Vector2 start = new Vector2(1700, 900);
        public float[] anchorLerp = new float[12];
        public Rectangle frame;
        public int frames;
        public float ShipHelthMax = 7;
        public float shipHelth = 7;
        public int cannonDelay = 60;
        public Vector2 otherBoatPos;
        public Vector2 currentLightningPos;
        public float intenstityLightning;

        public float flash = 0;
        public float markerPlacer = 0;

        public EEPlayerShip(Vector2 pos, Vector2 vel) : base(pos, vel)
        {
            position = pos;
            velocity = vel;

            width = 44;
            height = 52;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/ShipMount").Value;
        }

        public override void Update()
        {
            float boatSpeed = 1;

            #region Player controls(movement and shooting)
            if (!Main.gamePaused)
            {
                position += velocity;
                if (Main.LocalPlayer.controlUp)
                {
                    velocity.Y -= 0.1f * boatSpeed;
                }
                if (Main.LocalPlayer.controlDown)
                {
                    velocity.Y += 0.1f * boatSpeed;
                }
                if (Main.LocalPlayer.controlRight)
                {
                    velocity.X += 0.1f * boatSpeed;
                }
                if (Main.LocalPlayer.controlLeft)
                {
                    velocity.X -= 0.1f * boatSpeed;
                }
                if (Main.LocalPlayer.controlUseItem && cannonDelay <= 0)
                {
                    //Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_BySourceId(ModContent.ProjectileType<FriendlyCannonball>()), position + Main.screenPosition, -Vector2.Normalize(position + Main.screenPosition - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyCannonball>(), 0, 0);
                    
                    SoundEngine.PlaySound(SoundID.Item61);
                    cannonDelay = 60;
                }
                cannonDelay--;
            }

            Vector2 v = new Vector2(boatSpeed * 4);

            velocity = Vector2.Clamp(velocity, -v, v);

            if (!Main.gamePaused)
            {
                velocity *= 0.98f;
            }
            #endregion

            flash += 0.01f;
            if (flash == 2)
            {
                flash = 10;
            }

            base.Update();
        }

        #region Drawing "Disembark" text
        internal void DrawSubText()
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            float alpha = modPlayer.subTextAlpha;
            Color color = Color.White;

            if (Main.worldName == KeyID.Sea)
            {
                string text = "Disembark?";
                color *= alpha;
                Vector2 textSize = Terraria.GameContent.FontAssets.MouseText.Value.MeasureString(text);
                float textPositionLeft = position.X - textSize.X / 2;
                Main.spriteBatch.DrawString(Terraria.GameContent.FontAssets.MouseText.Value, text, new Vector2(textPositionLeft, position.Y + 20) - Main.screenPosition, color * (1 - (modPlayer.cutSceneTriggerTimer / 180f)), 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            }
        }
        #endregion
    }
}
