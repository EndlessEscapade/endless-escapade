using Terraria;
using Terraria.ModLoader;

namespace Prophecy.Buffs
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
			ProphecyPlayer modPlayer = player.GetModPlayer<ProphecyPlayer>();
			if (player.ownedProjectileCounts[mod.ProjectileType("QuartzCrystal")] > 0)
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
