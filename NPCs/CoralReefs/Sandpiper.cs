using EEMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

namespace EEMod.NPCs.CoralReefs
{
    public class Sandpiper : EENPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 15;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 50;
            NPC.defense = 6;
            NPC.damage = 20;
            NPC.aiStyle = 68;
            NPC.knockBackResist = 10;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.HitSound = new LegacySoundStyle(3, 1, Terraria.Audio.SoundType.Sound);
            NPC.DeathSound = new LegacySoundStyle(4, 1, Terraria.Audio.SoundType.Sound);
        }
    }
}