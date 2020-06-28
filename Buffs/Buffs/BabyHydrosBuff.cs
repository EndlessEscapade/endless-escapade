using Terraria;
using Terraria.ModLoader;
using EEMod.Projectiles;

namespace EEMod.Buffs.Buffs
{
	public class BabyHydrosBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Baby Hydros");
			Description.SetDefault("The Baby Hydros fights for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
	}
}
