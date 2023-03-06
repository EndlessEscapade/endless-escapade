using EndlessEscapade.Common.FishermansLogUI;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items;

public class FishermansLog : ModItem
{
    public override void SetStaticDefaults() {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults() {
        Item.width = 32;
        Item.height = 32;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.maxStack = 1;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(gold: 1);
    }

    public override bool? UseItem(Player player) {
        if (FishermansLogUIState.Visible) return false;

        SoundEngine.PlaySound(new SoundStyle("EndlessEscapade/Assets/Sounds/UI/FishermansLogOpen"));
        FishermansLogUIState.Visible = true;
        return true;
    }
}