using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Furniture.GoblinFort;
using System.Collections.Generic;
using ReLogic.Content;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.GameContent;

namespace EEMod.NPCs.Goblins.Scrapwizard
{
	public class ScrapwizardHealthBar : ModBossBar
	{
		private int bossHeadIndex = -1;

		public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame)
		{
			// Display the previously assigned head index
			if (bossHeadIndex != -1)
			{
				return TextureAssets.NpcHeadBoss[bossHeadIndex];
			}
			return null;
		}

		public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float lifePercent, ref float shieldPercent)
		{
			// Here the game wants to know if to draw the boss bar or not. Return false whenever the conditions don't apply.
			// If there is no possibility of returning false (or null) the bar will get drawn at times when it shouldn't, so write defensive code!

			NPC npc = Main.npc[info.npcIndexToAimAt];
			if (!npc.active)
				return false;

			// We assign bossHeadIndex here because we need to use it in GetIconTexture
			bossHeadIndex = npc.GetBossHeadTextureIndex();

			lifePercent = Utils.Clamp(npc.life / (float)npc.lifeMax, 0f, 1f);

			if (npc.ModNPC is Scrapwizard body)
			{
				// We did all the calculation work on RemainingShields inside the body NPC already so we just have to fetch the value again
				shieldPercent = Utils.Clamp(body.guardShield, 0f, 1f);
			}

			return true;
		}
	}
}