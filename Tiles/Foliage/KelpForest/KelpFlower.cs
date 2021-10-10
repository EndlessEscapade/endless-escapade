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
using EEMod.Items.Weapons.Summon.Whips;
using EEMod.Items.Accessories.InterstellarKelpBud;
using EEMod.Items.Weapons.Melee.Yoyos;
using Terraria.Audio;

namespace EEMod.Tiles.Foliage.KelpForest
{
    public class KelpFlower : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);

            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 18 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<KelpFlowerTE>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            // TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Kelpblossom");
            AddMapEntry(Color.DarkMagenta, name);
            DustType = DustID.PurpleTorch;
            DisableSmartCursor = true;
            AnimationFrameHeight = 66;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            int x = i - tile.frameX / 16 % 5;
            int y = j - tile.frameY / 16 % 4;

            int targetTe = ModContent.GetInstance<KelpFlowerTE>().Find(x, y);
            if (targetTe > -1 && TileEntity.ByID[targetTe] is KelpFlowerTE TE)
            {
                frameYOffset = (AnimationFrameHeight * TE._frame);
            }

            if (targetTe > -1 && TileEntity.ByID[targetTe] is KelpFlowerTE kelpFlowerEntity)
            {
                if (kelpFlowerEntity._frame > 6 && !kelpFlowerEntity.isOpen)
                {
                    Color chosen = Color.Lerp(Color.Gold, Color.LightYellow, Main.rand.NextFloat(1f));
                    EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.25f));
                    EEMod.MainParticles.SpawnParticles(new Vector2(i * 16 + Main.rand.Next(0, 16), j * 16 + Main.rand.Next(0, 16)), new Vector2(Main.rand.NextFloat(-0.075f, 0.075f), Main.rand.NextFloat(-1f, -3f)), Mod.Assets.Request<Texture2D>("Particles/SmallCircle").Value, 30, 1, chosen, new SlowDown(0.99f), new RotateTexture(0.02f), new SetMask(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/RadialGradient").Value, 0.6f), new AfterImageTrail(0.98f), new RotateVelocity(Main.rand.NextFloat(-0.01f, 0.01f)), new SetLighting(chosen.ToVector3(), 0.3f));
                }

                if (kelpFlowerEntity.isOpen)
                {
                    Color chosen = Color.Lerp(Color.Gold, Color.LightYellow, Main.rand.NextFloat(1f));
                    EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.0075f));
                    EEMod.MainParticles.SpawnParticles(new Vector2(i * 16 + Main.rand.Next(0, 16), j * 16 + Main.rand.Next(0, 16)), new Vector2(Main.rand.NextFloat(-0.05f, 0.05f), Main.rand.NextFloat(-0.5f, -1f)), Mod.Assets.Request<Texture2D>("Particles/SmallCircle").Value, 30, 1, chosen, new SlowDown(0.99f), new RotateTexture(0.02f), new SetMask(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/RadialGradient").Value, 0.6f), new AfterImageTrail(0.98f), new RotateVelocity(Main.rand.NextFloat(-0.01f, 0.01f)), new SetLighting(chosen.ToVector3(), 0.3f));
                }
            }
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<KelpFlowerTE>().Kill(i, j);
        }

        public override bool RightClick(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            int x = i - tile.frameX / 16 % 4;
            int y = j - tile.frameY / 16 % 4;

            int targetTe = ModContent.GetInstance<KelpFlowerTE>().Find(x, y);
            if (targetTe > -1 && TileEntity.ByID[targetTe] is KelpFlowerTE TE)
            {
                if(TE._frame == 0)
                    TE.isOpening = true;
            }

            return true;
        }
    }

    public class KelpFlowerTE : EETileEntity
    {
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Framing.GetTileSafely(x, y);
            return tile.IsActive;
        }

        public Projectile myItem = null;
        public bool isOpen;
        public bool isOpening;
        public int cooldown;

        private bool itemDeployed = false;

        public int _frame = 0;
        public int _frameCounter;

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
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
            if(isOpening && (myItem == null || myItem.active == false) && !itemDeployed && _frame > 5)
            {
                //SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/KelpFlowerOpen"));
                myItem = Projectile.NewProjectileDirect(new ProjectileSource_TileInteraction(Main.LocalPlayer, Position.X, Position.Y), new Vector2((Position.X * 16) + 32, (Position.Y * 16) + 24), new Vector2(0, -2.5f), ModContent.ProjectileType<KelpFlowerItem>(), 0, 0f, default, ChooseItem());
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
                if (_frame < 10 && _frameCounter > 4)
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

            if (_frame == 10)
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

            if (choice == 0) return ModContent.ItemType<InterstellarKelpBud>();
            else if (choice == 1) return ModContent.ItemType<KelpWhip>();
            else if (choice == 2) return ModContent.ItemType<KelpThrow>();
            else return ItemID.DirtBlock;
        }
    }
}