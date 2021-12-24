using Terraria.ModLoader;
using Terraria;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.ID;
using EEMod.NPCs.Friendly;

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
				/*1*/ "Can you help me repair that ship?",
				/*2*/ "What made you stop?",
				/*3*/ "Bye!",
				/*4*/ $"That ship sitting broken off the pier used to be one of the best vessels in the land. ",
				/*5*/ $"I'm past my days of sailing, but you look like you want to see the seven seas. ",
				/*6*/ $"If you brought me [c/E4A214:{"150 Wood"}] [i:{ItemID.Wood}] and [c/E4A214:{"20 Silk"}] [i:{ItemID.Silk}] along with a solid payment of [c/E4A214:{"5 gold coins"}] [i:{ItemID.GoldCoin}], I'd get her fixed right up for you.",
				/*7*/ "Here you go!",
				/*8*/ "No, bye."
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
				case (1):
					//PresentResponses(new int[1] { 4 });
					SayPiece(4);
					break;
				case (4):
					//PresentResponses(new int[1] { 5 });
					SayPiece(5);
					break;
				case (5):
					//PresentResponses(new int[1] { 6 });
					SayPiece(6);
					break;
				case (6):
					PresentResponses(new int[2] { 7, 8 });
					break;
				case (7):
                    (Main.npc[AssociatedNPC].ModNPC as Sailor).ticker = 0;
					(Main.npc[AssociatedNPC].ModNPC as Sailor).cutsceneOpacity = 0;
					(Main.npc[AssociatedNPC].ModNPC as Sailor).cutsceneActive = true;
					CloseDialogue();
					break;
				default:
					CloseDialogue();
					break;
			}
		}
	}
}
