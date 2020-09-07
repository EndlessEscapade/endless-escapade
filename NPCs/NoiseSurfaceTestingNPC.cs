using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class NoiseSurfaceTestingNPC : ModNPC
    {
        public override void SetStaticDefaults() => DisplayName.SetDefault("Glow");

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;
            npc.alpha = 20;
            npc.lifeMax = 550;
            npc.defense = 10;
            npc.width = 144;
            npc.height = 160;
            npc.noGravity = true;
            npc.buffImmune[BuffID.Confused] = true;
            npc.lavaImmune = false;
            npc.noTileCollide = false;
        }
    }
}