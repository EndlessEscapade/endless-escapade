using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using ReLogic.Content;
using CustomSlot;
using EEMod.Items;

namespace EEMod.UI.States
{
    public class ShipLoadoutUI : UIState
    {
		public UIDragableImage Background;

        public CustomItemSlot CannonSlot { get; private set; }
		public CustomItemSlot CannonSlot2 { get; private set; }
		public CustomItemSlot FigureheadSlot { get; private set; }

		public override void OnInitialize()
        {
			Background = new UIDragableImage(ModContent.Request<Texture2D>("EEMod/UI/ShipLoadout", AssetRequestMode.ImmediateLoad).Value);
			Background.HAlign = 0.5f;
			Background.VAlign = 0.5F;

			//Reminder for myself if I forget the issue is right here
			CroppedTexture2D emptyTexture = new CroppedTexture2D(ModContent.Request<Texture2D>("EEMod/UI/CannonEmptyPlaceholder").Value); //Placeholder

			CannonSlot = new CustomItemSlot(0, backgroundTexture: ModContent.Request<Texture2D>("EEMod/UI/ShipLoadoutItemSlot", AssetRequestMode.ImmediateLoad).Value)
			{
				IsValidItem = item => item.GetGlobalItem<EEGlobalItem>().Tag == (int)ItemTags.Cannon,
				EmptyTexture = emptyTexture,
				ActualColor = Color.White,
				HoverText = "Cannon"
			};
			//There is always a less than a pixel offset on the X axis no matter if I put 104 or 105 :ppsdct:
			CannonSlot.Top.Set(104f, 0f);
			CannonSlot.Left.Set(104f, 0f);
			Background.Append(CannonSlot);

			CannonSlot2 = new CustomItemSlot(0, backgroundTexture: ModContent.Request<Texture2D>("EEMod/UI/ShipLoadoutItemSlot", AssetRequestMode.ImmediateLoad).Value)
			{
				IsValidItem = item => item.GetGlobalItem<EEGlobalItem>().Tag == (int)ItemTags.Cannon,// && other bool
				EmptyTexture = emptyTexture,
				ActualColor = Color.White,
				HoverText = "Locked" //bool ? "Second Cannon" : "Locked"
			};
			CannonSlot2.Top.Set(168f, 0f);
			CannonSlot2.Left.Set(104f, 0f);
			Background.Append(CannonSlot2);

			FigureheadSlot = new CustomItemSlot(0, backgroundTexture: ModContent.Request<Texture2D>("EEMod/UI/ShipLoadoutItemSlot", AssetRequestMode.ImmediateLoad).Value)
			{
				IsValidItem = item => item.GetGlobalItem<EEGlobalItem>().Tag == (int)ItemTags.Figurehead,
				EmptyTexture = emptyTexture,
				ActualColor = Color.White,
				HoverText = "Figurehead"
			};
			FigureheadSlot.Top.Set(104f, 0f);
			FigureheadSlot.Left.Set(360f, 0f);
			Background.Append(FigureheadSlot);

			Append(Background);
        }
        public override void Update(GameTime gameTime)
        {
			base.Update(gameTime);
        }
    }

	//Edited from RecipeBrowser's UIDragablePanel 
	public class UIDragableImage : UIImage
	{
		private static Asset<Texture2D> dragTexture;
		private Vector2 offset;
		private bool dragable;
		private bool dragging;

		private List<UIElement> additionalDragTargets;

		// TODO, move panel back in if offscreen? prevent drag off screen?
		public UIDragableImage(Texture2D texture, bool dragable = true, bool resizeableX = false, bool resizeableY = false) : base(ModContent.Request<Texture2D>("EEMod/UI/ShipLoadout", AssetRequestMode.ImmediateLoad).Value)
		{
			SetImage(texture);
			this.dragable = dragable;
			if (dragTexture == null)
			{
				dragTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/PanelBorder");
			}
			additionalDragTargets = new List<UIElement>();
		}

		public void AddDragTarget(UIElement element)
		{
			additionalDragTargets.Add(element);
		}

		public override void MouseDown(UIMouseEvent evt)
		{
			DragStart(evt);
			base.MouseDown(evt);
		}

		public override void MouseUp(UIMouseEvent evt)
		{
			DragEnd(evt);
			base.MouseUp(evt);
		}

		private void DragStart(UIMouseEvent evt)
		{
			if (evt.Target == this || additionalDragTargets.Contains(evt.Target))
			{
				if (dragable)
				{
					offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
					dragging = true;
				}
			}
		}

		private void DragEnd(UIMouseEvent evt)
		{
			if (evt.Target == this || additionalDragTargets.Contains(evt.Target))
			{
				dragging = false;
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetOuterDimensions();
			if (ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
				Main.LocalPlayer.cursorItemIconEnabled = false;
				Main.ItemIconCacheUpdate(0);
			}
			if (dragging)
			{
				Left.Set(Main.MouseScreen.X - offset.X, 0f);
				Top.Set(Main.MouseScreen.Y - offset.Y, 0f);
				Recalculate();
			}
			else
			{
				if (Parent != null && !dimensions.ToRectangle().Intersects(Parent.GetDimensions().ToRectangle()))
				{
					var parentSpace = Parent.GetDimensions().ToRectangle();
					Left.Pixels = Utils.Clamp(Left.Pixels, Width.Pixels - parentSpace.Right, 0); // TODO: Adjust automatically for Left.Percent (measure from left or right edge)
					Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
					Recalculate();
				}
			}
			base.DrawSelf(spriteBatch);
		}

		private void DrawDragAnchor(SpriteBatch spriteBatch, Texture2D texture, Color color)
		{
			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)(dimensions.X + dimensions.Width - 12), (int)(dimensions.Y + dimensions.Height - 12));
			spriteBatch.Draw(texture, new Rectangle(point.X - 2, point.Y - 2, 12 - 2, 12 - 2), new Rectangle(12 + 4, 12 + 4, 12, 12), color);
			spriteBatch.Draw(texture, new Rectangle(point.X - 4, point.Y - 4, 12 - 4, 12 - 4), new Rectangle(12 + 4, 12 + 4, 12, 12), color);
			spriteBatch.Draw(texture, new Rectangle(point.X - 6, point.Y - 6, 12 - 6, 12 - 6), new Rectangle(12 + 4, 12 + 4, 12, 12), color);
		}
	}
}