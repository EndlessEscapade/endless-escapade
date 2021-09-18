using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria.DataStructures;

namespace EEMod.Items.Accessories.InterstellarKelpBud
{
    public class InterstellarKelpBud : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Interstellar Kelp Bud");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.defense = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<KelpBudPlayer>().interstellarKelpBud = true;
        }
    }

    public class KelpBudPlayer : ModPlayer
    {
        public bool interstellarKelpBud = false;
        public bool inKelpRing = false;

        public override void NaturalLifeRegen(ref float regen)
        {
            if (inKelpRing)
            {
                regen *= 1.3f;
            }
        }
    }


    public class KelpBudNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (Main.rand.NextBool(5) && Main.LocalPlayer.GetModPlayer<KelpBudPlayer>().interstellarKelpBud)
            {
                Projectile.NewProjectile(new ProjectileSource_NPC(npc), npc.Center, new Vector2(0, 0.5f), ModContent.ProjectileType<KelpBudProjectile>(), 0, 0f);
            }
        }
    }

    public class KelpBudProjectile : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Interstellar Kelp Bud");
        }

        public override void SetDefaults()
        {
            Projectile.tileCollide = true;
            Projectile.width = 12;
            Projectile.height = 10;
            // Projectile.friendly = false;
            // Projectile.hostile = false;
            Projectile.hide = true;
            Projectile.timeLeft = 1240;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.1f;

            Projectile.ai[0]++;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        private Vector2 ringOffset = new Vector2(-6, 0);
        private bool isDying = false;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (Projectile.ai[0] < 120)
            {
                Texture2D tex = ModContent.Request<Texture2D>("EEMod/Items/Accessories/InterstellarKelpBud/KelpBudProjectile").Value;
                Main.spriteBatch.Draw(tex, Projectile.position + new Vector2(0, 10) - Main.screenPosition, tex.Bounds, Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16) * (1 - (Projectile.alpha / 255f)), 0f, tex.Bounds.Size() / 2, 1f, SpriteEffects.None, 0f);
            }
            else if (Projectile.ai[0] >= 60 && Projectile.ai[0] < 180)
            {
                Texture2D tex = ModContent.Request<Texture2D>("EEMod/Items/Accessories/InterstellarKelpBud/InterstellarKelpBudMid").Value;
                Main.spriteBatch.Draw(tex, Projectile.position + new Vector2(0, 8) - Main.screenPosition, tex.Bounds, Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16) * (1 - (Projectile.alpha / 255f)), 0f, tex.Bounds.Size() / 2, 1f, SpriteEffects.None, 0f);
            }
            else
            {
                Texture2D tex = ModContent.Request<Texture2D>("EEMod/Items/Accessories/InterstellarKelpBud/InterstellarKelpBudBig").Value;
                Main.spriteBatch.Draw(tex, Projectile.position + new Vector2(0, 4) - Main.screenPosition, tex.Bounds, Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16) * (1 - (Projectile.alpha / 255f)), 0f, tex.Bounds.Size() / 2, 1f, SpriteEffects.None, 0f);

                Texture2D ringTex = ModContent.Request<Texture2D>("EEMod/Textures/InverseMask").Value;

                float ringScale = (MathHelper.Clamp((float)Math.Sqrt((Projectile.ai[0] - 180) / 60f), 0f, 1f) + ((Projectile.ai[0] >= 270) ? (float)(Math.Sin(Main.GameUpdateCount / 60f) / 35f) : 0)) * MathHelper.Clamp((float)Math.Sqrt((1160 - Projectile.ai[0]) / 40f), 0f, 1f);

                Helpers.DrawAdditive(ringTex, Projectile.Center - Main.screenPosition + ringOffset, Color.Goldenrod * ((0.15f * (float)Math.Sin(Main.GameUpdateCount / 60f)) + 0.85f), ringScale);


                if (Projectile.ai[0] >= 270 && Projectile.ai[0] < 1160)
                {
                    float ringScale2 = MathHelper.Clamp((float)Math.Sqrt(Projectile.ai[0] % 90.0 / 80.0), 0f, 1f);

                    Helpers.DrawAdditive(ringTex, Projectile.Center - Main.screenPosition + ringOffset, Color.DarkGoldenrod * ((70 - (Projectile.ai[0] % 90)) / 80f), ringScale2);

                    Color chosen = Color.Lerp(Color.Gold, Color.DarkGoldenrod, Main.rand.NextFloat(1f));

                    EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.05f));

                    Vector2 location = new Vector2(Main.rand.Next(0, (int)(90 * ringScale)), 0).RotatedByRandom(6.28f) + Projectile.Center + ringOffset;
                    Vector2 tileLocation = location / 16;

                    if (!Main.tile[(int)tileLocation.X, (int)tileLocation.Y].IsActive)
                    {
                        EEMod.MainParticles.SpawnParticles(location, Vector2.Zero, Mod.Assets.Request<Texture2D>("Particles/BigPlusSign").Value, 30, Main.rand.NextFloat(1f, 1.5f), chosen, new SetMask(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/RadialGradient").Value, 0.6f));
                    }
                }

                if (Vector2.Distance(Main.player[Projectile.owner].Center, Projectile.Center + ringOffset) < 200 * ringScale)
                {
                    Main.player[Projectile.owner].GetModPlayer<KelpBudPlayer>().inKelpRing = true;
                }
                else
                {
                    // Main.player[Projectile.owner].GetModPlayer<KelpBudPlayer>().inKelpRing = false;
                }

                Lighting.AddLight(Projectile.Center, Color.Gold.ToVector3() * ringScale / 2f);

                if((Projectile.ai[0] >= 1160 && ringScale <= 0.05f) || isDying)
                {
                    isDying = true;

                    Projectile.ai[1]++;

                    Projectile.alpha += 16;

                    float ringScale2 = MathHelper.Clamp((float)Math.Sqrt((Projectile.ai[1] % 120) / 120), 0f, 1f);

                    Helpers.DrawAdditive(ringTex, Projectile.Center - Main.screenPosition + ringOffset, Color.DarkGoldenrod * (1 - (Projectile.alpha / 255f)), ringScale2);
                }
            }

            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override void Kill(int timeLeft)
        {
            // Main.player[Projectile.owner].GetModPlayer<KelpBudPlayer>().inKelpRing = false;
        }
    }
}