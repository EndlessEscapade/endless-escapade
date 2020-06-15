using Terraria;
using Terraria.ModLoader;
using EEMod.Projectiles;

namespace EEMod.Buffs.Buffs
{
	public class QuartzCrystal : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Quartz Shard");
			Description.SetDefault("The Quartz Shard fights for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            EEPlayer modPlayer = player.GetModPlayer<EEPlayer>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<QuartzCrystalProjectile>()] > 0)
			{
				modPlayer.quartzCrystal = true;
			}
			if (!modPlayer.quartzCrystal)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}
