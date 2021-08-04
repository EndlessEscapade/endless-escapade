using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class NoiseSurfaceTestingNPC : EENPC
    {
        public override void SetStaticDefaults() => DisplayName.SetDefault("Glow");

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;
            NPC.alpha = 20;
            NPC.lifeMax = 550;
            NPC.defense = 10;
            NPC.width = 144;
            NPC.height = 160;
            NPC.noGravity = true;
            NPC.buffImmune[BuffID.Confused] = true;
            NPC.lavaImmune = false;
            NPC.noTileCollide = false;
        }
    }
}