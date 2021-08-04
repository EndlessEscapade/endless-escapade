using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class GreenSlimeGoBurr : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sans Slime");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BlueSlime];
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 44;
            NPC.damage = 12;
            NPC.defense = 10000000;
            NPC.lifeMax = 10000000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 100f;
            NPC.knockBackResist = 0.3f;
            NPC.aiStyle = -1;
            NPC.alpha = 60;
            aiType = NPCID.BlueSlime;
            animationType = NPCID.BlueSlime;
            NPC.noGravity = false;
            NPC.dontTakeDamage = true;
        }

        public int yes;
        public int interval = 120;

        public override void AI()
        {
            yes++;
            if (yes == interval)
            {
                CombatText.NewText(NPC.getRect(), Colors.RarityYellow, "Haha Sans Go Burr");
            }

            if (yes == interval * 3)
            {
                CombatText.NewText(NPC.getRect(), Colors.RarityYellow, "Mlem");
            }

            if (yes == interval * 5)
            {
                CombatText.NewText(NPC.getRect(), Colors.RarityYellow, "Haha Funny Content");
            }

            if (yes == interval * 7)
            {
                CombatText.NewText(NPC.getRect(), Colors.RarityYellow, "mmmyes lets do that");
            }
        }
    }
}