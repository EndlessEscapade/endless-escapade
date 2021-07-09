using EEMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

namespace EEMod.NPCs.CoralReefs
{
    public class Sandpiper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 15;
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 50;
            npc.defense = 6;
            npc.damage = 20;
            npc.aiStyle = 68;
            animationType = NPCID.Duck;
            npc.knockBackResist = 10;
            npc.value = Item.buyPrice(0, 0, 5, 0);
            npc.HitSound = new LegacySoundStyle(3, 1, Terraria.Audio.SoundType.Sound);
            npc.DeathSound = new LegacySoundStyle(4, 1, Terraria.Audio.SoundType.Sound);
        }
    }
}