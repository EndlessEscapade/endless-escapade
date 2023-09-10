using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems;

public class ItemResearchSystem : ModSystem
{
    public override void PostSetupContent() {
        foreach (var modItem in ModContent.GetContent<ModItem>()) {
            var item = ContentSamples.ItemsByType[modItem.Type];

            var isTile = item.createTile > -1;
            var isWall = item.createWall > -1;
            var isWeapon = item.damage > 0;
            var isEquip = item.accessory || item.defense > 0;

            var sacrificeCount = 25;

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
