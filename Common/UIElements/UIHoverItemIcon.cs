using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace EndlessEscapade.Common.UIElements;

internal class UIHoverItemIcon : UIItemIcon
{
    Item item;

    public UIHoverItemIcon(int itemType, bool blackedOut = false) : base(new Item(itemType), blackedOut) {
        item = new Item(itemType);
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);

        if (IsMouseHovering) {
            Main.hoverItemName = item.Name;
        }
    }
}