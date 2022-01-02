using IL.Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.Audio;

namespace EEMod.Systems.EEGame
{
    public class SpaceInvaders : EEGame
    {
        public override Texture2D tex => EEMod.Instance.Assets.Request<Texture2D>("UI/EEGameAssets/ArcadeBG").Value;
        public override Vector2 sizeOfMainCanvas => new Vector2(600, 800);
        public override Vector2 centerOfMainCanvas => Main.LocalPlayer.Center;
        public override Color colourOfMainCanvas => Color.White;
        public override float speedOfStartUp => 16f;
        private float pauseShaderTImer;
        private int player;

        private int[,,] formations = {
            {
                {0, 0, 0, 0, 1, 1, 0 },
                {0, 0, 0, 0, 1, 0, 1 },
                {0, 0, 1, 1, 0, 0, 1 },
                {0, 1, 0, 0, 0, 1, 0 },
                {1, 0, 0, 0, 0, 1, 0 },
                {1, 0, 0, 1, 0, 0, 1 },
                {0, 1, 1, 1, 1, 1, 1 }
            },
            {
                {0, 1, 0, 0, 0, 1, 0 },
                {0, 0, 1, 0, 1, 0, 0 },
                {0, 1, 1, 1, 1, 1, 0 },
                {1, 1, 0, 1, 0, 1, 1 },
                {1, 0, 1, 1, 1, 0, 1 },
                {1, 1, 0, 0, 0, 1, 1 },
                {0, 1, 1, 0, 1, 1, 0 }
            },
            {
                {0, 0, 1, 1, 1, 1, 0 },
                {0, 1, 0, 0, 0, 0, 1 },
                {1, 0, 0, 0, 0, 1, 0 },
                {1, 0, 0, 0, 1, 0, 0 },
                {1, 0, 0, 0, 0, 1, 0 },
                {0, 1, 0, 0, 0, 0, 1 },
                {0, 0, 1, 1, 1, 1, 0 }
            },
            {
                {1, 0, 1, 1, 1, 0, 1 },
                {1, 0, 1, 1, 1, 0, 1 },
                {1, 0, 1, 1, 1, 0, 1 },
                {1, 0, 0, 0, 0, 0, 1 },
                {1, 0, 1, 1, 1, 0, 1 },
                {1, 0, 1, 1, 1, 0, 1 },
                {1, 0, 1, 1, 1, 0, 1 }
            },
        };

        public int lives = 2;
        public int[] lifeImages = new int[3];
        public override void Initialize()
        {
            Texture2D SAPlayer = EEMod.Instance.Assets.Request<Texture2D>("UI/EEGameAssets/SAPlayer").Value;
            player = AddUIElement(new Vector2(30, 48), Color.White, centerOfMainCanvas + new Vector2(0, 300));
            //elementArray[puck].AttatchToMouse(16f, i);
            elementArray[player].BindElementToGame(this);
            elementArray[player].AttachCollisionComponents(false, true, false);
            elementArray[player].BindElementToTexture(SAPlayer);

            SpawnFormation();

            for (int i = 0; i < 3; i++)
            {
                lifeImages[i] = AddUIElement(new Vector2(30, 48), Color.White, centerOfMainCanvas + new Vector2(-266 + (i * 48), -366));

                elementArray[lifeImages[i]].BindElementToGame(this);
                elementArray[lifeImages[i]].AttachCollisionComponents(false, false, false);
                elementArray[lifeImages[i]].BindElementToTexture(SAPlayer);
            }
            time = 0;
            level = 1;
        }

        void SpawnFormation()
        {
            int legoYoda = Main.rand.Next(0, 4);
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (formations[legoYoda, 6 - j, 6 - i] == 1)
                    {
                        Color enemyColor = Color.White;
                        if (level == 2) enemyColor = Color.Blue;
                        if (level == 3) enemyColor = Color.Green;
                        int enemy = AddUIElement(new Vector2(36, 30), enemyColor, centerOfMainCanvas + new Vector2(97 + (-i * 34), -100 + (-j * 30)));

                        elementArray[enemy].BindElementToGame(this);
                        elementArray[enemy].AttachCollisionComponents(false, true, false, 1);
                        elementArray[enemy].BindElementToTexture(EEMod.Instance.Assets.Request<Texture2D>("UI/EEGameAssets/SAEnemy").Value);
                        elementArray[enemy].speedOfStartUp = 8;
                        elementArray[enemy].AttachTag("SAEnemy");
                        elementArray[enemy].velocity.X = 1.5f;
                        elementArray[enemy].friction = 1;
                        elementArray[enemy].ai[1] = 1;
                        elementArray[enemy].ai[0] = 60;
                        elementArray[enemy].ai[2] = 0;
                        elementArray[enemy].ai[3] = level;
                    }
                }
            }
        }

        public override void OnDeactivate()
        {
            if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:Pause"].IsActive())
            {
                Filters.Scene.Deactivate("EEMod:Pause");
            }
        }

        private int shootCooldown;
        private int time;
        public int level;

        public void LoseLife()
        {
            lives--;
            // elementArray[lifeImages[lives + 1]].elementActive = false;
            foreach (GameElement GE in elementArray)
            {
                if (GE != null && GE.tag == "SAEnemy")
                {
                    // GE.elementActive = false;
                    time = 780;
                    SoundEngine.PlaySound(SoundID.NPCDeath56, Main.LocalPlayer.Center);
                }
            }
            if (lives <= -1)
            {
                EndGame();
                // Main.LocalPlayer.GetModPlayer<EEPlayer>().playingGame = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:Pause"].IsActive())
            {
                Filters.Scene.Activate("EEMod:Pause").GetShader().UseOpacity(pauseShaderTImer);
            }
            Filters.Scene["EEMod:Pause"].GetShader().UseOpacity(pauseShaderTImer);
            pauseShaderTImer += 50;
            if (pauseShaderTImer > 1000)
            {
                pauseShaderTImer = 1000;
            }

            if (gameActive)
            {
                #region Player controls
                if (Main.LocalPlayer.controlLeft)
                {
                    elementArray[player].velocity = new Vector2(-3, 0);
                }
                if (Main.LocalPlayer.controlRight)
                {
                    elementArray[player].velocity = new Vector2(3, 0);
                }

                if (shootCooldown > 0) shootCooldown--;
                if (Main.LocalPlayer.controlUseItem && shootCooldown <= 0)
                {
                    int bolt = AddUIElement(new Vector2(14, 32), Color.White, elementArray[player].centerOfElement);

                    elementArray[bolt].BindElementToGame(this);
                    elementArray[bolt].AttachCollisionComponents(false, true, false);
                    elementArray[bolt].BindElementToTexture(EEMod.Instance.Assets.Request<Texture2D>("UI/EEGameAssets/SABolt").Value);
                    elementArray[bolt].velocity = new Vector2(0, -16);
                    elementArray[bolt].lifetime = 45;
                    elementArray[bolt].friction = 1;
                    elementArray[bolt].AttachTag("SABolt");
                    elementArray[bolt].speedOfStartUp = 1;
                    shootCooldown = 20;
                    SoundEngine.PlaySound(SoundID.Item93);
                }
                #endregion

                time += level;

                if (time % 840 == 0 && time != 0)
                {
                    SpawnFormation();
                }

                if (time > 1800)
                {
                    level++;
                    if (level >= 3)
                    {
                        EndGame();
                        Main.LocalPlayer.QuickSpawnItem(ItemID.SilverCoin, 50);
                    }
                    time = 0;
                }
            }

            base.Update(gameTime);
        }
    }
}