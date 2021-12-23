using Terraria.ModLoader;
using Terraria;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;

namespace EEMod.Systems
{
	public class SailorDialogue : Dialogue
	{
		public override void StartDialogueRequiringNPC(int associatedNPC)
		{
			Portraits.Add(ModContent.Request<Texture2D>("EEMod/Systems/Dialogues/SailorPortrait", AssetRequestMode.ImmediateLoad).Value);
			Name = Main.npc[associatedNPC].FullName;
			AssociatedNPC = associatedNPC;
			ThemeColor = new Color(125, 175, 255);
			LockPlayerMovement = true;
			//Placeholder dialogue for the showcase until I get back
			DialoguePieces = new List<string>()
			{
				/*0*/ "Sometimes I keep thinking about me old days, always sailing 'round the world.",
				/*1*/ "I'd like to see what you have for sale (Open Shop).",
				/*2*/ "What made you stop?",
				/*3*/ "(Close Dialogue)",
			};
			if (!Main.player[Main.myPlayer].GetModPlayer<DialoguePlayer>().HasTalkedToSailor)
            {
				FirstPiece(0);
				Main.player[Main.myPlayer].GetModPlayer<DialoguePlayer>().HasTalkedToSailor = true;
			}
            else
            {
				FirstPiece(0);
			}
			base.StartDialogueRequiringNPC(associatedNPC);
		}
		public override void OnDialoguePieceFinished(int piece)
		{
			switch (piece)
            {
                case (0):
					PresentResponses(new int[3] { 2, 1, 3 });
					break;
                default:
					CloseDialogue();
					break;
			}
		}
	}
}
