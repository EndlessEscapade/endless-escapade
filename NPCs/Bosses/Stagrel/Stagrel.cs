using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Compatibility;
using Microsoft.Xna.Framework.Graphics;
using EEMod.EEWorld;
using EEMod.Items.Weapons.Ranger;
using EEMod.Items.Weapons.Mage;

namespace EEMod.NPCs.Bosses.Stagrel
{
    public class Stagrel : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stagrel, Golem of the Oasis");
        }

        public override void SetDefaults()
        {
            npc.boss = true;
            npc.lavaImmune = true;
            npc.friendly = false;
            npc.noGravity = false;

            npc.aiStyle = -1;
            npc.lifeMax = 50000;
            npc.defense = 25;
            npc.damage = 95;
            npc.value = Item.buyPrice(0, 8, 0, 0);

            npc.width = 155;
            npc.height = 204;

            npc.npcSlots = 36f;
            npc.knockBackResist = 0f;

            musicPriority = MusicPriority.BossMedium;

            music = Compatibilities.EEMusic?.GetSoundSlot(SoundType.Music, "Sounds/Music/Stagrel") ?? MusicID.Boss2;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 75000;
            npc.damage = 110;
            npc.defense = 28;
        }

        public override void NPCLoot()
        {
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.NextBool(2))
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Stalagtite>());
                else
                    Item.NewItem(npc.getRect(), ModContent.ItemType<SandBuster>());
            }

            EEWorld.EEWorld.downedStagrel = true;
            //EEMod.ServBoolUpdate();   Not working for some reason ;-;
        }

        // Phases
        public float Phase { get => npc.ai[0]; set => npc.ai[0] = value; }
        // Attack Timer
        public float AttackTimer { get => npc.ai[1]; set => npc.ai[1] = value; }
        // Attack Phase
        public float SubPhase { get => npc.ai[2]; set => npc.ai[2] = value; }

        private int Timer;

        public override void AI()
        {
            Player player = Main.player[npc.target];

            if (!player.active || player.dead)
            {
                npc.velocity = new Vector2(0f, 10f);
                npc.timeLeft = 10;
                npc.noTileCollide = true;
            }

            /*Behaviour
			  Idle
			  Stagrel slowly moves in the player's direction. DONE

			  Jump Attack
			  Stagrel jumps to over 40 tiles above the player and instantly falls down, basically smashing the player on the ground. TODO

			  Projectiles Attack
			  Stagrel creates over 7 homing projectiles on his back, which all of them passes through over 1 tile over his head and start homing on the player. TODO

			  Fist Attack
			  Stagrel goes insanely fast on the player's direction, punching them. After that, Stagrel will be exaust for 21 / 17 / 10 seconds (Free hit time). TODO

			  Note: Stagrel switches from "Phases" as the following:

			  Idle => Jump => Idle => Projectiles => Idle => Fist => Loop(Idle (0)) (aka restart)

			  The idle phase is 10 seconds long on normal, 6 on expert and 4 on genkai
			  */

            Timer++;

            // Idle 0
            // 0 X 2 X 4
            if (Phase == 0f || Phase == 2f || Phase == 4f)
            {
                npc.TargetClosest(true);
                if (player.Center.X > npc.position.X)
                    npc.velocity.X = 1f;
                if (player.Center.X < npc.position.X)
                    npc.velocity.X = -1f;

                if (Timer >= (EEWorld.EEWorld.GenkaiMode ? 4 * 60 : Main.expertMode ? 60 * 6 : 60 * 10))
                {
                    Phase++;
                }
            }
            // Jump Attack
            else if (Phase >= 1f && Phase < 2f)
            {
                if (Phase == 1f)
                {
                    Phase = 1.1f;
                }
                else if (Phase == 1.1f)
                {

                }
            }
            // Projectiles Attack
            else if (Phase == 3f)
            {

            }
            //Fist Attack
            else if (Phase == 5f)
            {

            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = TextureCache.Stagrel_Glow;
            spriteBatch.Draw
            (
                texture,
                npc.Center,
                Color.White
            );
        }

        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.direction;
        }
    }
}
