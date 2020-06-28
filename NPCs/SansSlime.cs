using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;

namespace EEMod.NPCs
{
    public class SansSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sans Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
        }

        public override void SetDefaults()
        {
            npc.width = 32;
            npc.height = 44;
            npc.damage = 12;
            npc.defense = 10000000;
            npc.lifeMax = 10000000;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 100f;
            npc.knockBackResist = 0.3f;
            npc.aiStyle = -1;
            npc.alpha = 60;
            aiType = NPCID.BlueSlime;
            animationType = NPCID.BlueSlime;
            npc.noGravity = false;
            npc.dontTakeDamage = true;
        }

        public int yes;
        public int interval = 120;
        public override void AI()
        {
            yes++;
            if (yes == interval*2)
                CombatText.NewText(npc.getRect(), Colors.RarityBlue, "*Boing*", false, false);
            if (yes == interval*4)
                CombatText.NewText(npc.getRect(), Colors.RarityBlue, "Ahh Yes The Mlemage", false, false);
            if (yes == interval*6)
                CombatText.NewText(npc.getRect(), Colors.RarityBlue, "Destroy the world now?", false, false);
        }

        public override void NPCLoot()
        {
            // this is still pretty useless to do
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<QuartzGem>(), Main.rand.Next(1, 3));
        }
    }
}
