using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.Tiles.Furniture.OrbHolder;
using EEMod.Items.Placeables.Ores;
using EEMod.Projectiles.Enemy;

namespace EEMod.NPCs
{
    public class Starweaver : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starweaver");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            // NPC.friendly = false;
            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;
            NPC.alpha = 0;
            NPC.lifeMax = 200;
            NPC.width = 32;
            NPC.height = 44;
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            NPC.damage = 10;
            NPC.knockBackResist = 0f;
        }

        private bool spawningStars;
        private int chosenConstellation;
        private int index;
        private Vector2[][] constellations = new Vector2[][]
        {
            new Vector2[] { new Vector2(-64, -64), new Vector2(-32, -50), new Vector2(0, -50), new Vector2(32, -64), new Vector2(24, -80), new Vector2(64, -96), new Vector2(64, -72) }
        };

        public override void AI()
        {
            Player target = Main.player[NPC.FindClosestPlayer()];

            NPC.ai[0]++;

            if(NPC.ai[0] % 900 == 0)
            {
                spawningStars = true;
                chosenConstellation = Main.rand.Next(0, constellations.Length);
                index = 0;
            }

            if(spawningStars && NPC.ai[0] % 60 == 0 && index < constellations[chosenConstellation].Length)
            {
                Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), NPC.Center + constellations[chosenConstellation][index], Vector2.Zero, ModContent.ProjectileType<StarweaverStar>(), 40, 2f);

                index++;
            }
        }
    }
}