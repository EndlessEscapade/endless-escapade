using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace EndlessEscapade.Common.UIElements;
internal class FishermansLogGridElement : UIPanel
{
    internal int type;

    public FishermansLogGridElement(StyleDimension size, int type) : base() {
        this.type = type;

        Width = size;
        Height = size;

        BackgroundColor = Color.Transparent;
        BorderColor = Color.Black * 0.25f;

        OnClick += FishermansLogGridElement_OnClick;

        var ButtonIcon = this.AddElement(new UIHoverItemIcon(type, new Item(type).Name).With(e => {
            e.HAlign = 0.5f;
            e.VAlign = 0.5f;
        }));
    }

    private void FishermansLogGridElement_OnClick(UIMouseEvent evt, UIElement listeningElement) {
        // Open specific page
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);

        if (IsMouseHovering) {
            BackgroundColor = Color.Gold * 0.25f;
            BorderColor = Color.Gold;
        }
        else {
            BackgroundColor = Color.Transparent;
            BorderColor = Color.Black * 0.25f;
        }
    }
}
