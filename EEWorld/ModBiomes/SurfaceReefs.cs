using EEMod.ID;
using EEMod.Subworlds.CoralReefs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace EEMod.EEWorld.ModBiomes
{
	public class SurfaceReefs : ModBiome
	{
		public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("EEMod", "CoralWaterStyle"); // Sets a water style for when inside this biome

		//public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("EEMod/");

		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/SurfaceReefs");
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

        public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Surface Reefs");
		}

		public override bool IsBiomeActive(Player player)
		{
			return /*(float)(player.Center.Y / Main.maxTilesY * 16f) < 1 / 10f && */SubworldLibrary.SubworldSystem.IsActive<CoralReefs>();

			//return true;
		}
	}
}