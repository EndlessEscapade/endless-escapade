using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.UI;
using CustomSlot;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.UI.Elements;
using EEMod.Extensions;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.UI.Chat;
using ReLogic.Content;

namespace EEMod.UI.States
{
	public class DialogueUI : UIState
	{
		public static UIImage Portrait;
		public static UIElement DialoguePoint;
		public static string Dialogue;
		public static DialogueBox Background;
		public string CurrentDialogue = "";
		public int AddLetterTimer;
		public int CurrentLetter;
        public override void OnInitialize()
		{
			Background = new DialogueBox();
			Background.HAlign = 0.5f;
			Background.VAlign = 1f;

			Portrait = new UIImage(ModContent.Request<Texture2D>("EEMod/icon", AssetRequestMode.ImmediateLoad).Value);
			Portrait.HAlign = 0.9f;
			Portrait.VAlign = 0.9f;
;

			DialoguePoint = new UIElement();
			DialoguePoint.HAlign = 0.25f;
			DialoguePoint.VAlign = 0.175f;
			Background.Append(DialoguePoint);

			Append(Background);
			Append(Portrait);
		}
		public override void Update(GameTime gameTime)
		{
            base.Update(gameTime);
        }
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			if (CurrentLetter < Dialogue.Length && ++AddLetterTimer >= 3)
			{
				AddLetterTimer -= 3;
				var letter = Dialogue[CurrentLetter];
				CurrentLetter++;
				CurrentDialogue += letter;
				if (letter == '.' || letter == '?')
				{
					AddLetterTimer -= 10;
				}
				if (letter == ',')
				{
					AddLetterTimer -= 7;
				}
			}
			var dimensions = DialoguePoint.GetDimensions();
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Terraria.GameContent.FontAssets.MouseText.Value, CurrentDialogue, new Vector2(dimensions.X, dimensions.Y), Color.White, 0f, Vector2.Zero, new Vector2(1.15f, 1.15f));
		}
	}
	public class DialogueBox : UIImageButton
	{
		public Texture2D Texture;
		public Color ThemeColor;
		public DialogueBox() : base(ModContent.Request<Texture2D>("EEMod/UI/DialogueBoxBackground"))
		{
			Texture = ModContent.Request<Texture2D>("EEMod/UI/DialogueBoxBackground", AssetRequestMode.ImmediateLoad).Value;
			ThemeColor = Color.LightBlue;
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{ 
			var dimensions = GetDimensions();
			int x = (int)(dimensions.X + Texture.Size().X / 2);
			int y = (int)(dimensions.Y);
			spriteBatch.Draw(Texture, new Vector2(x, y), null, ThemeColor * 0.9f, 0f, Texture.Size(), 1f, SpriteEffects.None, 0f);
		}
	}
}