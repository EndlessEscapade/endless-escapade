using System;
using Terraria.ModLoader;

namespace EndlessEscapade.Common
{
    public abstract class EENPC : ModNPC
    {
        public override void OnChatButtonClicked(bool firstButton, ref string shopName) {
            var isShop = false;
            OnChatButtonClicked(firstButton, ref isShop);
            if (isShop) {
                shopName = "Shop";
            }
        }
        public virtual void OnChatButtonClicked(bool firstButton, ref bool shop) {

        }
    }
}
