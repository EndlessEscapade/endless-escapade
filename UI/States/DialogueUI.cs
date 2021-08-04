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

namespace EEMod.UI.States
{
	public class DialogueUI : UIState
	{
		public static UIPanel Portrait;
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

			Portrait = new UIPanel();
			Portrait.Width.Set(120, 0);
			Portrait.Height.Set(120, 0);
			Portrait.HAlign = 0.05f;
			Portrait.VAlign = 0.3f;
			Portrait.BackgroundColor = Color.Red * 0.85f;
			Background.Append(Portrait);

			DialoguePoint = new UIElement();
			DialoguePoint.HAlign = 0.25f;
			DialoguePoint.VAlign = 0.175f;
			Background.Append(DialoguePoint);

			Append(Background);
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
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, CurrentDialogue, new Vector2(dimensions.X, dimensions.Y), Color.White, 0f, Vector2.Zero, new Vector2(1.15f, 1.15f));
		}
	}
	public class DialogueBox : UIImageButton
	{
		public Texture2D Texture;
		public Color ThemeColor;
		public DialogueBox() : base(ModContent.GetTexture("EEMod/UI/DialogueBoxBackground"))
		{
			Texture = ModContent.GetTexture("EEMod/UI/DialogueBoxBackground");
			ThemeColor = Color.LightBlue;
			SetVisibility(0.9f, 0.9f);
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{ 
			var dimensions = GetDimensions();
			int x = (int)(dimensions.X + Texture.Size().X);
			int y = (int)(dimensions.Y + Texture.Size().Y);
			spriteBatch.Draw(Texture, new Vector2(x, y), null, ThemeColor, 0f, Texture.Size(), 1.1f, SpriteEffects.None, 0f);
		}
	}
}