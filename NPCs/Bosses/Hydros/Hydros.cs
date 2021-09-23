using EEMod.Items.Materials;
using EEMod.Items.TreasureBags;
using EEMod.Items.Weapons.Mage;
using EEMod.Items.Weapons.Summon.Minions;
using EEMod.Items.Weapons.Melee;
using EEMod.Items.Weapons.Ranger.Guns;
using EEMod.Items.Weapons.Melee.Yoyos;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace EEMod.NPCs.Bosses.Hydros
{
    [AutoloadBossHead]
    public class Hydros : EENPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void FindFrame(int frameHeight) //Frame counter
        {
            //NPC.TargetClosest(true);
            /*Player player = Main.player[NPC.target];
            if (NPC.frameCounter++ > 4)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y = NPC.frame.Y + frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 7)
            {
                NPC.frame.Y = 0;
                return;
            }*/
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.lifeMax = 1600;
            NPC.defense = 12;
            NPC.damage = 20;
            NPC.knockBackResist = 0;

            NPC.value = Item.buyPrice(0, 3, 5, 0);

            NPC.HitSound = new LegacySoundStyle(3, 1, Terraria.Audio.SoundType.Sound);
            NPC.DeathSound = new LegacySoundStyle(4, 1, Terraria.Audio.SoundType.Sound);
            BossBag = ItemType<HydrosBag>();
            NPC.width = 314;
            NPC.height = 162;

            NPC.boss = true;
            NPC.noGravity = true;

            NPC.noTileCollide = true;
        }

        public override void AI()
        {
            
        }

        public override void OnKill()
        {
            if (!Main.expertMode)
            {
                int randVal = Main.rand.Next(5);
                switch (randVal)
                {
                    case 1:
                        Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemType<CyanoburstTome>(), 1);
                        break;

                    case 2:
                        Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemType<Triggerfish>(), 1);
                        break;

                    case 3:
                        //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Hydroshot>(), 1);
                        break;

                    case 4:
                        Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemType<EnchantedCoral>(), 1);
                        break;
                }
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemType<HydrosScales>(), Main.rand.Next(28, 56));
                EEWorld.EEWorld.downedHydros = true;
            }
            else
            {
                NPC.DropBossBags();
            }
        }

        /*public override bool CheckDead()
        {
            int goreIndex = Gore.NewGore(new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) - 24f), default(Vector2), mod.GetGoreSlot("Gores/HydrosGore"), 1f);
            Main.gore[goreIndex].scale = 1.5f;
            Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;

            return true;
        }*/
    }
}