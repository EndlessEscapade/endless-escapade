using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.Audio;

namespace CustomSlot {
    //Ported from https://github.com/abluescarab/tModLoader-CustomSlot
    public class CustomItemSlot : UIElement {
        public enum ArmorType {
            Head,
            Chest,
            Leg
        }

        public static class DefaultColors {
            public static readonly Color EmptyTexture = Color.White * 0.35f;
            public static readonly Color InventoryItemBack = Main.inventoryBack;
            public static readonly Color EquipBack = Color.White * 0.8f;

        }

        internal const int TickOffsetX = 6;
        internal const int TickOffsetY = 2;

        public Item Item;
        public Color ActualColor;
        private CroppedTexture2D _backgroundTexture;
        private float _scale;
        private ToggleVisibilityButton _toggleButton;
        private bool _forceToggleButton;

        public int Context { get; }
        public bool ItemVisible { get; set; }
        public string HoverText { get; set; }
        public Func<Item, bool> IsValidItem { get; set; }
        public CroppedTexture2D EmptyTexture { get; set; }
        public CustomItemSlot Partner { get; set; }

        public float Scale {
            get => _scale;
            set {
                _scale = value;
                CalculateSize();
            }
        }

        public CroppedTexture2D BackgroundTexture {
            get => _backgroundTexture;
            set {
                _backgroundTexture = value;
                CalculateSize();
            }
        }

        public bool ForceToggleButton {
            get => _forceToggleButton;
            set {
                _forceToggleButton = value;
                bool hasButton = _forceToggleButton;

                if(!hasButton) {
                    if(_toggleButton == null) return;

                    RemoveChild(_toggleButton);
                    _toggleButton = null;
                }
                else {
                    _toggleButton = new ToggleVisibilityButton();
                    Append(_toggleButton);
                }
            }
        }

        public CustomItemSlot(int context = ItemSlot.Context.InventoryItem, float scale = 1f,
            ArmorType defaultArmorIcon = ArmorType.Head) {
            Context = context;
            _scale = scale;
            _backgroundTexture = GetBackgroundTexture(context);
            EmptyTexture = GetEmptyTexture(context, defaultArmorIcon);
            ItemVisible = true;
            ForceToggleButton = false;

            Item = new Item();
            Item.SetDefaults();

            CalculateSize();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch) {
            DoDraw(spriteBatch);

            if(ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface) {
                Main.LocalPlayer.mouseInterface = true;

                if(_toggleButton != null && _toggleButton.ContainsPoint(Main.MouseScreen)) return;

                if(Main.mouseItem.IsAir || IsValidItem == null || IsValidItem(Main.mouseItem)) {
                    int tempContext = Context;

                    // fix if it's a vanity slot with no partner
                    if(Main.mouseRightRelease && Main.mouseRight) {
                        if(Context == ItemSlot.Context.EquipArmorVanity)
                            tempContext = ItemSlot.Context.EquipArmor;
                        else if(Context == ItemSlot.Context.EquipAccessoryVanity)
                            tempContext = ItemSlot.Context.EquipAccessory;
                    }

                    if(Partner != null && Main.mouseRightRelease && Main.mouseRight) {
                        SwapWithPartner();
                    }
                    else {
                        ItemSlot.Handle(ref Item, tempContext);
                    }

                    if(!string.IsNullOrEmpty(HoverText)) {
                        Main.hoverItemName = HoverText;
                    }
                }
            }
        }

        private void DoDraw(SpriteBatch spriteBatch) {
            Rectangle rectangle = GetDimensions().ToRectangle();
            Texture2D itemTexture = EmptyTexture.Texture;
            Rectangle itemRectangle = EmptyTexture.Rectangle;
            Color color = EmptyTexture.Color;
            float itemLightScale = 1f;

            if(Item.stack > 0) {
                itemTexture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
                itemRectangle = Main.itemAnimations[Item.type] != null ?
                    Main.itemAnimations[Item.type].GetFrame(itemTexture) : itemTexture.Frame();
                color = Color.White;

                ItemSlot.GetItemLight(ref color, ref itemLightScale, Item);
            }

            if(BackgroundTexture.Texture != null) {
                spriteBatch.Draw(
                    BackgroundTexture.Texture,
                    rectangle.TopLeft(),
                    BackgroundTexture.Rectangle,
                    ActualColor,
                    0f,
                    Vector2.Zero,
                    Scale,
                    SpriteEffects.None,
                    1f);
            }

            if(itemTexture != null) {
                // copied from ItemSlot.Draw()
                float oversizedScale = 1f;
                if(itemRectangle.Width > 32 || itemRectangle.Height > 32) {
                    if(itemRectangle.Width > itemRectangle.Height) {
                        oversizedScale = 32f / itemRectangle.Width;
                    }
                    else {
                        oversizedScale = 32f / itemRectangle.Height;
                    }
                }

                oversizedScale *= Scale;

                spriteBatch.Draw(
                    itemTexture,
                    rectangle.Center(),
                    itemRectangle,
                    Item.color == Color.Transparent ? Item.GetAlpha(color) : Item.GetColor(color),
                    0f,
                    new Vector2(itemRectangle.Center.X - itemRectangle.Location.X,
                                itemRectangle.Center.Y - itemRectangle.Location.Y),
                    oversizedScale * itemLightScale,
                    SpriteEffects.None,
                    0f);
            }

            // position based on vanilla code
            if(Item.stack > 1) {
                ChatManager.DrawColorCodedStringWithShadow(
                    spriteBatch,
                    Terraria.GameContent.FontAssets.ItemStack.Value,
                    Item.stack.ToString(),
                    GetDimensions().Position() + new Vector2(10f, 26f) * Scale,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    new Vector2(Scale),
                    -1f,
                    Scale);
            }
        }

        /// <summary>
        /// Swap the current item with its partner slot.
        /// </summary>
        private void SwapWithPartner() {
            // modified from vanilla code
            Utils.Swap(ref Item, ref Partner.Item);
            SoundEngine.PlaySound(SoundID.Grab);
            Recipe.FindRecipes();

            if(Item.stack <= 0) return;

            if(Context != 0) {
                if(Context - 8 <= 4 || Context - 16 <= 1) {
                    AchievementsHelper.HandleOnEquip(Main.LocalPlayer, Item, Context);
                }
            }
            else {
                AchievementsHelper.NotifyItemPickup(Main.LocalPlayer, Item);
            }
        }

        /// <summary>
        /// Calculate the size of the slot based on background texture and scale.
        /// </summary>
        internal void CalculateSize() {
            if(BackgroundTexture == CroppedTexture2D.Empty) return;

            float width = BackgroundTexture.Texture.Width * Scale;
            float height = BackgroundTexture.Texture.Height * Scale;

            Width.Set(width, 0f);
            Height.Set(height, 0f);
        }
        internal class ToggleVisibilityButton : UIElement {
            internal ToggleVisibilityButton() {
                Width.Set(Terraria.GameContent.TextureAssets.InventoryTickOn.Value.Width, 0f);
                Height.Set(Terraria.GameContent.TextureAssets.InventoryTickOn.Value.Height, 0f);
            }

            protected override void DrawSelf(SpriteBatch spriteBatch) {
                if(!(Parent is CustomItemSlot slot)) return;

                DoDraw(spriteBatch, slot);

                if(ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface) {
                    Main.LocalPlayer.mouseInterface = true;
                    Main.hoverItemName = Language.GetTextValue(slot.ItemVisible ? "LegacyInterface.59" : "LegacyInterface.60");

                    if(Main.mouseLeftRelease && Main.mouseLeft) {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        slot.ItemVisible = !slot.ItemVisible;
                    }
                    Recalculate();
                }
                /*
                Rectangle r = new Microsoft.Xna.Framework.Rectangle(0, 0, (int)((float)Main.inventoryBackTexture.Width * Main.inventoryScale), (int)((float)Main.inventoryBackTexture.Height * Main.inventoryScale));
                int P = Main.screenWidth - 92;
                int O = Main.minScreenH + 174;
                r.X = P + 1 * -47;
                r.Y = O + 4 * 47;

                ItemSlot.Draw(spriteBatch, Main.player[Main.myPlayer].miscEquips, 20, 4, r.TopLeft(), default(Microsoft.Xna.Framework.Color));
                */
            }

            private void DoDraw(SpriteBatch spriteBatch, CustomItemSlot slot) {
                Rectangle parentRectangle = Parent.GetDimensions().ToRectangle();
                Texture2D tickTexture =
                    slot.ItemVisible ? Terraria.GameContent.TextureAssets.InventoryTickOn.Value : Terraria.GameContent.TextureAssets.InventoryTickOff.Value;

                Left.Set(parentRectangle.Width - Width.Pixels + TickOffsetX, 0f);
                Top.Set(-TickOffsetY, 0f);

                spriteBatch.Draw(tickTexture, GetDimensions().Position(), Color.White * 0.7f);
            }
        }

        /// <summary>
        /// Get the background texture of a slot based on its context.
        /// </summary>
        /// <param name="context">slot context</param>
        /// <returns>background texture of the slot</returns>
        public static CroppedTexture2D GetBackgroundTexture(int context) {
            Texture2D texture;
            Color color = Main.inventoryBack;

            switch(context) {
                case ItemSlot.Context.EquipAccessory:
                case ItemSlot.Context.EquipArmor:
                case ItemSlot.Context.EquipGrapple:
                case ItemSlot.Context.EquipMount:
                case ItemSlot.Context.EquipMinecart:
                case ItemSlot.Context.EquipPet:
                case ItemSlot.Context.EquipLight:
                    color = DefaultColors.EquipBack;
                    texture = Terraria.GameContent.TextureAssets.InventoryBack3.Value;
                    break;
                case ItemSlot.Context.EquipArmorVanity:
                case ItemSlot.Context.EquipAccessoryVanity:
                    color = DefaultColors.EquipBack;
                    texture = Terraria.GameContent.TextureAssets.InventoryBack8.Value;
                    break;
                case ItemSlot.Context.EquipDye:
                    color = DefaultColors.EquipBack;
                    texture = Terraria.GameContent.TextureAssets.InventoryBack12.Value;
                    break;
                case ItemSlot.Context.ChestItem:
                    color = DefaultColors.InventoryItemBack;
                    texture = Terraria.GameContent.TextureAssets.InventoryBack5.Value;
                    break;
                case ItemSlot.Context.BankItem:
                    color = DefaultColors.InventoryItemBack;
                    texture = Terraria.GameContent.TextureAssets.InventoryBack2.Value;
                    break;
                case ItemSlot.Context.GuideItem:
                case ItemSlot.Context.PrefixItem:
                case ItemSlot.Context.CraftingMaterial:
                    color = DefaultColors.InventoryItemBack;
                    texture = Terraria.GameContent.TextureAssets.InventoryBack4.Value;
                    break;
                case ItemSlot.Context.TrashItem:
                    color = DefaultColors.InventoryItemBack;
                    texture = Terraria.GameContent.TextureAssets.InventoryBack7.Value;
                    break;
                case ItemSlot.Context.ShopItem:
                    color = DefaultColors.InventoryItemBack;
                    texture = Terraria.GameContent.TextureAssets.InventoryBack6.Value;
                    break;
                default:
                    texture = Terraria.GameContent.TextureAssets.InventoryBack.Value;
                    break;
            }

            return new CroppedTexture2D(texture, color);
        }

        /// <summary>
        /// Get the empty texture of a slot based on its context.
        /// </summary>
        /// <param name="context">slot context</param>
        /// <param name="armorType">type of equipment in the slot</param>
        /// <returns>empty texture of the slot</returns>
        public static CroppedTexture2D GetEmptyTexture(int context, ArmorType armorType = ArmorType.Head) {
            int frame = -1;

            switch(context) {
                case ItemSlot.Context.EquipArmor:
                    switch(armorType) {
                        case ArmorType.Head:
                            frame = 0;
                            break;
                        case ArmorType.Chest:
                            frame = 6;
                            break;
                        case ArmorType.Leg:
                            frame = 12;
                            break;
                    }
                    break;
                case ItemSlot.Context.EquipArmorVanity:
                    switch(armorType) {
                        case ArmorType.Head:
                            frame = 3;
                            break;
                        case ArmorType.Chest:
                            frame = 9;
                            break;
                        case ArmorType.Leg:
                            frame = 15;
                            break;
                    }
                    break;
                case ItemSlot.Context.EquipAccessory:
                    frame = 11;
                    break;
                case ItemSlot.Context.EquipAccessoryVanity:
                    frame = 2;
                    break;
                case ItemSlot.Context.EquipDye:
                    frame = 1;
                    break;
                case ItemSlot.Context.EquipGrapple:
                    frame = 4;
                    break;
                case ItemSlot.Context.EquipMount:
                    frame = 13;
                    break;
                case ItemSlot.Context.EquipMinecart:
                    frame = 7;
                    break;
                case ItemSlot.Context.EquipPet:
                    frame = 10;
                    break;
                case ItemSlot.Context.EquipLight:
                    frame = 17;
                    break;
            }

            if(frame == -1) return CroppedTexture2D.Empty;

            Texture2D extraTextures = Terraria.GameContent.TextureAssets.Extra[54].Value;
            Rectangle rectangle = extraTextures.Frame(3, 6, frame % 3, frame / 3);
            rectangle.Width -= 2;
            rectangle.Height -= 2;

            return new CroppedTexture2D(extraTextures, DefaultColors.EmptyTexture, rectangle);
        }
    }
}
