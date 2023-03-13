using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace EndlessEscapade.Common.UIElements;

internal class UIHoverItemIcon : UIItemIcon
{
    internal string hoverText { get; set; }

    public UIHoverItemIcon(int itemType, string hoverText = null) : base(new Item(itemType), false) {
        this.hoverText = hoverText;
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);

        if (IsMouseHovering && hoverText != null) {
            Main.hoverItemName = hoverText;
        }
    }
}