using EEMod.ID;
using EEMod.Items.Materials;
using EEMod.Items.Weapons.Mage;
using EEMod.Items.Weapons.Melee;
using EEMod.Items.Weapons.Ranger;
using EEMod.Items.Weapons.Summon;
using EEMod.NPCs.CoralReefs;
using EEMod.NPCs.CoralReefs.MechanicalReefs;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class SpawnPools : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            Player spawnplayer = spawnInfo.player;
            if (spawnplayer.GetModPlayer<EEPlayer>().ZoneCoralReefs) {
                pool[0] = 0f;
                if (spawnInfo.player.Center.Y < ((Main.maxTilesY / 20) + (Main.maxTilesY / 60) + (Main.maxTilesY / 60)) * 16)
                {
                    if (!spawnplayer.GetModPlayer<EEPlayer>().jellyfishMigration)
                    {
                        pool.Add(ModContent.NPCType<HermitCrab>(), 5f);
                        pool.Add(ModContent.NPCType<FlowerHatJelly>(), 5f);
                        pool.Add(ModContent.NPCType<SeaSlug>(), 5f);
                        pool.Add(ModContent.NPCType<OlvinicSnail>(), 5f);
                        pool.Add(NPCID.Shark, 5f);
                    }
                    else
                    {
                        pool.Add(ModContent.NPCType<FlowerHatJelly>(), 5f);
                        pool.Add(ModContent.NPCType<CombJelly>(), 5f);
                        pool.Add(ModContent.NPCType<CrownJelly>(), 5f);
                        pool.Add(ModContent.NPCType<OlvinicSnail>(), 5f);
                    }
                }
                else if (spawnplayer.Center.Y <= 12800 && spawnInfo.player.Center.Y >= ((Main.maxTilesY / 20) + (Main.maxTilesY / 60) + (Main.maxTilesY / 60)) * 16)
                {
                    pool.Add(ModContent.NPCType<Clam>(), 5f);
                    pool.Add(ModContent.NPCType<LunaJelly>(), 5f);
                    pool.Add(ModContent.NPCType<SeaSlug>(), 5f);
                    pool.Add(ModContent.NPCType<Seahorse>(), 5f);
                }
                else if (spawnplayer.Center.Y > 12800)
                {
                    pool.Add(ModContent.NPCType<Clam>(), 0.5f);
                    pool.Add(ModContent.NPCType<Ball>(), 0.5f);
                    pool.Add(ModContent.NPCType<GiantSquid>(), 0.5f);
                    pool.Add(ModContent.NPCType<SeaSlug>(), 0.5f);
                    pool.Add(ModContent.NPCType<ManoWar>(), 0.5f);
                }

                if (spawnInfo.player.GetModPlayer<EEPlayer>().reefMinibiome[(int)MinibiomeID.KelpForest])
                {
                    pool[0] = 0f;
                    pool.Add(ModContent.NPCType<GlowingKelpSpider>(), 0.5f);
                }
            }



            if (Main.ActiveWorldFileData.Name == KeyID.Island || Main.ActiveWorldFileData.Name == KeyID.Island2)
            {
                pool[0] = 0f;
                pool.Add(ModContent.NPCType<CoconutCrab>(), 0.5f);
                pool.Add(ModContent.NPCType<Cococritter>(), 0.5f);
            }
            if (Main.ActiveWorldFileData.Name == KeyID.Island || Main.ActiveWorldFileData.Name == KeyID.Island2 && !Main.dayTime)
            {
                pool[0] = 0f;
                pool.Add(ModContent.NPCType<CoconutSpider>(), 0.5f);
            }
        }
    }
}