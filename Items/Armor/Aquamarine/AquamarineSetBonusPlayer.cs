using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using EEMod.Extensions;

namespace EEMod.Items.Armor.Aquamarine
{
    public class AquamarineSetBonusPlayer : ModPlayer
    {
        public bool aquamarineSetBonus = true;

        public int aquamarineCooldown;
        public Vector2 aquamarineVel;

        public bool isLight;

        public override void UpdateEquips()
        {
            base.UpdateEquips();
            if (aquamarineSetBonus)
            {
                if (isLight)
                {
                    Player.gravity = 0;

                    if (Math.Abs(Player.velocity.X) <= 0.01) aquamarineVel.X = -aquamarineVel.X * 1.25f;

                    if (Math.Abs(Player.velocity.Y) <= 0.01) aquamarineVel.Y = -aquamarineVel.Y * 1.25f;

                    Player.velocity = aquamarineVel;

                    aquamarineCooldown++;
                    if (aquamarineCooldown >= 600 || (Player.controlUp && aquamarineCooldown >= 30))
                    {
                        isLight = false;
                        aquamarineCooldown = 30;
                        Player.gravity = 1;
                    }
                }
                else
                {
                    aquamarineCooldown--;
                    if (Player.controlUp && aquamarineCooldown <= 0)
                    {
                        isLight = true;

                        aquamarineVel = Vector2.Normalize(Main.MouseWorld - Player.Center) * 24;
                    }
                }
            }
        }

        public override void ResetEffects()
        {
            base.ResetEffects();
            aquamarineSetBonus = false;
        }

        // TODO: fix the remove layers
        public override void ModifyDrawLayerOrdering(IDictionary<PlayerDrawLayer, PlayerDrawLayer.Position> positions)
        {
            base.ModifyDrawLayerOrdering(positions);
            if (isLight)
            {
                for (int i = 0; i < positions.Count; i++)
                {
                    // layers[i].visible = false;
                }
            }
        }

        // TODO: port code to a player layer maybe
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
            if (isLight)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Projectiles/Nice").Value, Player.Center.ForDraw(), new Rectangle(0, 0, 174, 174), Color.White * 0.75f, Main.GameUpdateCount / 300f, new Rectangle(0, 0, 174, 174).Size() / 2, 0.5f, SpriteEffects.None, default);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            }
        }
    }
}
