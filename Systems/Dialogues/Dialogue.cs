using System;
using System.Reflection;
using NVorbis;
using Terraria.ModLoader;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using Terraria;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using EEMod.UI.States;
using EEMod.Extensions;
using Microsoft.Xna.Framework;

namespace EEMod.Systems
{
	public class Dialogue
	{
		public List<string> DialoguePieces = new List<string>();
		public List<Texture2D> Portraits = new List<Texture2D>();
		public string Name;
		public int AssociatedNPC;
		public int CurrentPortrait;
		public Color ThemeColor;
		public virtual void StartDialogue()
		{
			EEMod.UI.SetState("DialogueInterface", "DialogueUI");
			DialogueUI.Background.ThemeColor = ThemeColor;
		}
		public virtual void StartDialogueRequiringNPC(int associatedNPC)
		{
			EEMod.UI.SetState("DialogueInterface", "DialogueUI");
			AssociatedNPC = associatedNPC;
			DialogueUI.Background.ThemeColor = ThemeColor;
			DialogueUI.Portrait.SetImage(Portraits[0]);
			DialogueUI.Dialogue = (DialoguePieces[0].FormatString(100));
			DialogueUI.Dialogue = DialoguePieces[0].FormatString(100);
		}
		public virtual void SayPiece(int piece) 
		{
			DialogueUI.Dialogue = (DialoguePieces[piece].FormatString(100));
		}
		public virtual void PresentResponses(int[] responses) { }
		public virtual void CloseDialogue() 
		{
			EEMod.UI.RemoveState("DialogueInterface");
		}
		public virtual void OnDialoguePieceFinished(int piece) { }
    }
}
