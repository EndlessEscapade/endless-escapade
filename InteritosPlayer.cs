using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using InteritosMod.IntWorld;
using InteritosMod.NPCs.Archon;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using InteritosMod.NPCs.Akumo;


namespace InteritosMod
{
    public class InteritosPlayer : ModPlayer
    {
        public bool FlameSpirit;
        public bool magmaRune;
        public bool dalantiniumHood;
        public bool hydriteVisage;
        public bool ZoneCoralReefs;
		private int opac;
        public override void UpdateBiomes()
		{
			ZoneCoralReefs = InteritosWorld.CoralReefsTiles > 200;
			if(ZoneCoralReefs)
			{
				opac++;
				if (opac > 100)
					opac = 100;
				Filters.Scene.Activate("InteritosMod:CR").GetShader().UseOpacity(opac);
			}
			else
			{
				opac--;
				if (opac < 0)
					opac = 0;
				Filters.Scene.Deactivate("InteritosMod:CR");
			}
		}

        public override void UpdateBiomeVisuals()
        {
            player.ManageSpecialBiomeVisuals("InteritosMod:Akumo", NPC.AnyNPCs(ModContent.NPCType<Akumo>()));
        }

        public override bool CustomBiomesMatch(Player other)
        {
            InteritosPlayer modOther = other.GetModPlayer<InteritosPlayer>();
            return ZoneCoralReefs == modOther.ZoneCoralReefs;
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            InteritosPlayer modOther = other.GetModPlayer<InteritosPlayer>();
            modOther.ZoneCoralReefs = ZoneCoralReefs;
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = ZoneCoralReefs;
            writer.Write(flags);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            ZoneCoralReefs = flags[0];
        }
    }
}
