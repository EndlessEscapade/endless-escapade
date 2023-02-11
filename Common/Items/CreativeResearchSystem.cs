using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Items;

public sealed class CreativeResearchSystem : ModSystem
{
    public override void PostSetupContent() {
        foreach (ModItem modItem in ModContent.GetContent<ModItem>()) {
            Item item = ContentSamples.ItemsByType[modItem.Type];

            bool isTile = item.createTile > -1;
            bool isWall = item.createWall > -1;

            bool isWeapon = item.damage > 0;
            bool isEquip = item.accessory || item.defense > 0;

            int sacrificeCount = 25;

            if (isEquip) {
                sacrificeCount = 1;
            }
            else if (isTile) {
                sacrificeCount = 100;
            }
            else if (isWall) {
                sacrificeCount = 400;
            }
            else if (isWeapon) {
                sacrificeCount = item.consumable ? 100 : 1;
            }

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[item.type] = sacrificeCount;
        }
    }
}