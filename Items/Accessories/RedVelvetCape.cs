using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories
{
    public class RedVelvetCape : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Velvet Cape");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 30);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RedVelvetCapePlayer>().isWearingCape = true;
        }
    }

    public class RedVelvetCapePlayer : ModPlayer
    {
        public bool isWearingCape = false;

        public Vector2[] arrayPoints = new Vector2[24];
        private float propagation;

        Vector2 mainPoint => new Vector2(Player.Center.X, Player.position.Y);

        private readonly int displaceX = 2;
        private readonly int displaceY = 4;
        private readonly float[] dis = new float[51];

        public override void Initialize()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < arrayPoints.Length; i++)
                {
                    arrayPoints[i] = new Vector2(mainPoint.X + (i * displaceX), mainPoint.Y + (i * displaceY));
                }
            }
        }

        public override void UpdateEquips()
        {
            if(isWearingCape)
            {
                UpdateArrayPoints();
            }
        }

        public override void ResetEffects()
        {
            isWearingCape = false;
        }

        public void UpdateArrayPoints()
        {
            float acc = arrayPoints.Length;
            float upwardDrag = 0.2f;
            float smoothStepSpeed = 8;
            float yDis = 15;
            float propagtionSpeedWTRdisX = 15;
            float propagtionSpeedWTRvelY = 4;
            float basePosFluncStatic = 5f;
            float basePosFlunc = 3f;
            propagation += (Math.Abs(Player.velocity.X / 2f) * 0.015f) + 0.1f;
            for (int i = 0; i < acc; i++)
            {
                float prop = (float)Math.Sin(propagation + (i * propagtionSpeedWTRdisX / acc));
                Vector2 basePos = new Vector2(mainPoint.X + (i * displaceX) + (Math.Abs(Player.velocity.X / basePosFluncStatic) * i), mainPoint.Y + (i * displaceY) + 20);
                float dist = Player.position.Y + yDis - basePos.Y + prop / acc * Math.Abs(-Math.Abs(Player.velocity.X) - (i / acc));
                float amp = Math.Abs(Player.velocity.X * basePosFlunc) * (i * basePosFlunc / acc) + 1f;
                float goTo = Math.Abs(dist * (Math.Abs(Player.velocity.X) * upwardDrag)) + (Player.velocity.Y / propagtionSpeedWTRvelY * i);
                float disClamp = (goTo - dis[i]) / smoothStepSpeed;
                disClamp = MathHelper.Clamp(disClamp, -1.7f, 15);
                dis[i] += disClamp;
                if (i == 0)
                {
                    arrayPoints[i] = basePos;
                }
                else
                {
                    arrayPoints[i] = new Vector2(basePos.X, basePos.Y + prop / acc * amp - dis[i] + i * 2);
                }

                if (Player.direction == 1)
                {
                    float distX = arrayPoints[i].X - Player.Center.X;
                    arrayPoints[i].X = Player.Center.X - distX;
                }
                int tracker = 0;
                if (i != 0)
                {
                    Tile tile = Framing.GetTileSafely((int)arrayPoints[i].X / 16, (int)arrayPoints[i].Y / 16);
                    while (tile.HasTile &&
                            Main.tileSolid[tile.TileType]
                           || !Collision.CanHit(new Vector2(arrayPoints[i].X, arrayPoints[i].Y), 1, 1, new Vector2(arrayPoints[i - 1].X, arrayPoints[i - 1].Y), 1, 1))
                    {
                        arrayPoints[i].Y--;
                        tracker++;
                        if (tracker >= displaceY * acc)
                        {
                            break;
                        }

                        if (arrayPoints[i].Y <= arrayPoints[i - 1].Y - 4)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}