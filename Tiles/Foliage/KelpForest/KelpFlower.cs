using EEMod.Items.Materials;
using EEMod.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using EEMod.Items.Placeables.Furniture;
using EEMod.EEWorld;
using EEMod.UI.States;
using EEMod;
using EEMod.Items;
using EEMod.Projectiles.CoralReefs;
using EEMod.Items.Weapons.Melee;
using EEMod.Items.Weapons.Summon;

namespace EEMod.Tiles.Foliage.KelpForest
{
    public class KelpFlower : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);

            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<KelpFlowerTE>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Kelpblossom");
            AddMapEntry(new Color(255, 168, 28), name);
            dustType = 11;
            disableSmartCursor = true;
            animationFrameHeight = 72;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            int x = i - tile.frameX / 18 % 4;
            int y = j - tile.frameY / 18 % 4;

            int targetTe = ModContent.GetInstance<KelpFlowerTE>().Find(x, y);
            if (targetTe > -1 && TileEntity.ByID[targetTe] is KelpFlowerTE TE)
            {
                frameYOffset = (animationFrameHeight * TE._frame);
            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<KelpFlowerTE>().Kill(i, j);
        }

        public override bool NewRightClick(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            int x = i - tile.frameX / 18 % 4;
            int y = j - tile.frameY / 18 % 4;

            int targetTe = ModContent.GetInstance<KelpFlowerTE>().Find(x, y);
            if (targetTe > -1 && TileEntity.ByID[targetTe] is KelpFlowerTE TE)
            {
                TE.isOpening = true;
            }

            return true;
        }
    }

    public class KelpFlowerTE : ModTileEntity
    {
        public Projectile myItem = null;
        public bool isOpen;
        public bool isOpening;
        public int cooldown;

        private bool itemDeployed = false;

        public int _frame = 0;
        public int _frameCounter;

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            return tile.active();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i - 1, j - 1, 3);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
                return -1;
            }

            return Place(i, j);
        }

        public override void Update()
        {
            if(isOpen && (myItem == null || myItem.active == false) && !itemDeployed && isOpening)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/KelpFlowerOpen"));
                myItem = Projectile.NewProjectileDirect(new Vector2((Position.X * 16) + 32, Position.Y * 16), new Vector2(0, -2.5f), ModContent.ProjectileType<KelpFlowerItem>(), 0, 0f, default, ChooseItem());
                itemDeployed = true;
            }

            if(isOpen && myItem.active == false && itemDeployed)
            {
                isOpening = false;
                itemDeployed = false;
            }

            if (isOpening && !isOpen)
            {
                _frameCounter++;
                if (_frame < 3 && _frameCounter > 4)
                {
                    _frame++;
                    _frameCounter = 0;
                }
            }

            if (!isOpening && isOpen)
            {
                _frameCounter++;
                if (_frame > 0 && _frameCounter > 4)
                {
                    _frame--;
                    _frameCounter = 0;
                }
            }

            if (_frame == 3)
            {
               isOpen = true;
            }

            if (_frame == 0)
            {
                isOpen = false;
            }
        }

        private int ChooseItem()
        {
            int choice = Main.rand.Next(3);

            if (choice == 0) return ModContent.ItemType<KelpvineCannon>();
            else if (choice == 1) return ModContent.ItemType<KelpFlail>();
            else if (choice == 2) return ModContent.ItemType<KelpWhip>();
            else return ItemID.DirtBlock;
        }
    }
}