using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class SansSlime : EENPC
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
            AIType = NPCID.BlueSlime;
            AnimationType = NPCID.BlueSlime;
            // NPC.noGravity = false;
            NPC.dontTakeDamage = true;
        }

        public int yes;
        public int interval = 120;

        public override void AI()
        {
            yes++;
            if (yes == interval * 2)
            {
                CombatText.NewText(NPC.getRect(), Colors.RarityBlue, "*Boing*", false, false);
            }

            if (yes == interval * 4)
            {
                CombatText.NewText(NPC.getRect(), Colors.RarityBlue, "Ahh Yes The Mlemage", false, false);
            }

            if (yes == interval * 6)
            {
                CombatText.NewText(NPC.getRect(), Colors.RarityBlue, "Destroy the world now?", false, false);
            }
        }
    }
}