using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Items;

public sealed class ItemResearches : ModSystem
{
    public override void PostSetupContent() {
        foreach (var modItem in ModContent.GetContent<ModItem>()) {
            var item = ContentSamples.ItemsByType[modItem.Type];

            var tile = item.createTile > -1;
            var wall = item.createWall > -1;
            var weapon = item.damage > 0;
            var equip = item.accessory || item.defense > 0;

            var count = 25;

            if (equip) {
                count = 1;
            }
            else if (tile) {
                count = 100;
            }
            else if (wall) {
                count = 400;
            }
            else if (weapon) {
                count = item.consumable ? 100 : 1;
            }

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[item.type] = count;
        }
    }
}
