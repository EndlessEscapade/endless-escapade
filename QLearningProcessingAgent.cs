using EEMod.Autoloading;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Compatibility;
using EEMod.NPCs.Bosses.Hydros;
using System.Collections.Generic;
using System.Linq;
namespace EEMod
{
        public class QLearningAgent : ModNPC
        {
            int rows = 20;
            int columns = 20;
            float sizeOfSquares = 50;
            float[,] qValues => new float[columns,rows];
            float[,] rewards => new float[columns,rows];
            public string direction;
            float probability = 1;
            bool flag = false;
            int time = 0;
            int[,] positionOfBlocks = new int[120,68];
            int[,] positionOfBadBad = { { 19, 1 } };
            int[,] positionOfGoodGood = { { 19, 0 } };
            float startingPosX;
            float startingPosY;
            float discountValue = .99f;
            float movement = 0;
            string indication;
            int timeBeforeTermination = 2000;
            List<int[]> blocks = new List<int[]>();
            float impatience;
            int speedOfSimulation = 20;
            float fatigue = -0.002f;
            double movementVelSigma => QuadraticSolver(1, -1, -16 * 2);
            double QuadraticSolver(double a, double b, double c)
            {
                double determinant = b * b - 4 * a * c;
                double root1 = (-b + Math.Sqrt(determinant)) / (2 * a);
                double root2 = (-b - Math.Sqrt(determinant)) / (2 * a);
                if (root1 > root2)
                    return root1;
                else
                    return root2;
            }
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("QLearningAgent");
            }

            public override void SetDefaults()
            {
                npc.boss = true;
                npc.lavaImmune = true;
                npc.friendly = false;
                npc.noGravity = true;
                npc.aiStyle = -1;
                npc.lifeMax = 50000;
                npc.defense = 40;
                npc.damage = 0;
                npc.value = Item.buyPrice(0, 8, 0, 0);
                npc.noTileCollide = true;
                npc.width = 568;
                npc.height = 472;
                npc.npcSlots = 24f;
                npc.knockBackResist = 0f;
                musicPriority = MusicPriority.BossMedium;
                npc.alpha = 255;
                impatience = 0;
                startingPosX = sizeOfSquares * 0.5f;
                startingPosY = (rows * sizeOfSquares) - sizeOfSquares * 0.5f;
                //initialise values to zero
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        qValues[i,j] = 0;
                        rewards[i,j] = fatigue;
                    }
                }
                for (int a = 0; a < positionOfGoodGood.GetLength(0); a++)
                {
                    qValues[positionOfGoodGood[a,0],positionOfGoodGood[a,1]] = 1;
                }
                for (int a = 0; a < positionOfBadBad.GetLength(0); a++)
                {
                    qValues[positionOfBadBad[a,0],positionOfBadBad[a,1]] = -1;
                }
            }
            string command;
            string getDirection(int x, int y, float[,] tableHolder)
            {
                List<float> maxDirectionHolder = new List<float>();
                int right = x + 1;
                int left = x - 1;
                int up = y - 1;
                int down = y + 1;
               

                for (int i = 0; i < positionOfBlocks.GetLength(0); i++)
                {
                    if (right == positionOfBlocks[i,0] && y == positionOfBlocks[i,1] && positionOfBlocks[i,0] != 0 && positionOfBlocks[i,0] != columns - 1)
                    {
                        right = positionOfBlocks[i,0] - 1;
                    }
                    if (left == positionOfBlocks[i,0] && y == positionOfBlocks[i,1] && positionOfBlocks[i,0] != 0 && positionOfBlocks[i,0] != columns - 1)
                    {
                        left = positionOfBlocks[i,0] + 1;
                    }
                    if (down == positionOfBlocks[i,1] && x == positionOfBlocks[i,0] && positionOfBlocks[i,1] != 0 && positionOfBlocks[i,0] != rows - 1)
                    {
                        down = positionOfBlocks[i,1] - 1;
                    }
                    if (up == positionOfBlocks[i,1] && x == positionOfBlocks[i,0] && positionOfBlocks[i,1] != 0 && positionOfBlocks[i,0] != rows - 1)
                    {
                        up = positionOfBlocks[i,1] + 1;
                    }
                }
                if (right > columns - 1)
                    right = columns - 1;
                if (down > rows - 1)
                    down = rows - 1;
                if (up < 0)
                    up = 0;
                if (left < 0)
                    left = 0;
                maxDirectionHolder.Add(tableHolder[x,up]);
                maxDirectionHolder.Add(tableHolder[x,down]);
                maxDirectionHolder.Add(tableHolder[left,y]);
                maxDirectionHolder.Add(tableHolder[right,y]);
                float maxVal = maxDirectionHolder.Max(); // should return 7
                int maxIdx = maxDirectionHolder.IndexOf(maxVal);

                if (maxIdx == 0)
                    command = "up";
                if (maxIdx == 1)
                    command = "down";
                if (maxIdx == 2)
                    command = "left";
                if (maxIdx == 3)
                    command = "right";

                return command;
            }
            float getMaxSum(int x, int y, float[,] tableHolder)
            {
                List<float> maxHolder = new List<float>();
                int right = x + 1;
                int left = x - 1;
                int up = y - 1;
                int down = y + 1;
                if (right > columns - 1)
                    right = columns - 1;
                if (down > rows - 1)
                    down = rows - 1;
                if (up < 0)
                    up = 0;
                if (left < 0)
                    left = 0;
                for (int i = 0; i < positionOfBlocks.GetLength(0); i++)
                {
                    if (right == positionOfBlocks[i,0] && y == positionOfBlocks[i,1] && positionOfBlocks[i,0] != 0 && positionOfBlocks[i,0] != columns - 1)
                    {
                        right = positionOfBlocks[i,0] - 1;
                    }
                    if (left == positionOfBlocks[i,0] && y == positionOfBlocks[i,1] && positionOfBlocks[i,0] != 0 && positionOfBlocks[i,0] != columns - 1)
                    {
                        left = positionOfBlocks[i,0] + 1;
                    }
                    if (down == positionOfBlocks[i,1] && x == positionOfBlocks[i,0] && positionOfBlocks[i,1] != 0 && positionOfBlocks[i,1] != rows - 1)
                    {
                        down = positionOfBlocks[i,1] - 1;
                    }
                    if (up == positionOfBlocks[i,1] && x == positionOfBlocks[i,0] && positionOfBlocks[i,1] != 0 && positionOfBlocks[i,1] != rows - 1)
                    {
                        up = positionOfBlocks[i,1] + 1;
                    }
                }
                maxHolder.Add(tableHolder[right,y] * probability + tableHolder[x,up] * ((1 - probability) * 0.5f) + tableHolder[x,down] * ((1 - probability) * 0.5f));
                maxHolder.Add(tableHolder[left,y] * probability + tableHolder[x,up] * ((1 - probability) * 0.5f) + tableHolder[x,down] * ((1 - probability) * 0.5f));
                maxHolder.Add(tableHolder[x,up] * probability + tableHolder[left,y] * ((1 - probability) * 0.5f) + tableHolder[right,y] * ((1 - probability) * 0.5f));
                maxHolder.Add(tableHolder[x,down] * probability + tableHolder[left,y] * ((1 - probability) * 0.5f) + tableHolder[right,y] * ((1 - probability) * 0.5f));
                float maxVal = maxHolder.Max(); // should return 7
                int maxIdx = maxHolder.IndexOf(maxVal);

                if (maxIdx == 0)
                    direction = "right";
                if (maxIdx == 1)
                    direction = "left";
                if (maxIdx == 2)
                    direction = "up";
                if (maxIdx == 3)
                    direction = "down";
                return maxHolder.Max();
            }
            void terminate()
            {
                time = 0;
                impatience = 0;
                startingPosX = sizeOfSquares / 2;
                startingPosY = (rows * sizeOfSquares) - sizeOfSquares / 2;
                for (int i = columns - 1; i >= 0; i--)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        bool canText = true;
                        int[] coords = { i, j };
                        for (int q = 0; q < positionOfBadBad.GetLength(0); q++)
                        {
                            if (coords[0] == positionOfBadBad[q,0] && coords[1] == positionOfBadBad[q,1])
                                canText = false;
                        }
                        for (int e = 0; e < positionOfGoodGood.GetLength(0); e++)
                        {
                            if (coords[0] == positionOfGoodGood[e,0] && coords[1] == positionOfGoodGood[e,1])
                                canText = false;
                        }
                        for (int e = 0; e < positionOfBlocks.GetLength(0); e++)
                        {
                            if (coords[0] == positionOfBlocks[e,0] && coords[1] == positionOfBlocks[e,1])
                                canText = false;
                        }
                        if (canText)
                            qValues[i,j] = rewards[i,j] + (discountValue * (getMaxSum(i, j, qValues)));
                    }
                }
            }
            public override void AI()
            {
                for (int i = 0; i < positionOfBlocks.GetLength(0); i++)
                {
                    if (time % speedOfSimulation == 0 && getDirection((int)Math.Round((startingPosX - (sizeOfSquares * 0.5)) * (1 / sizeOfSquares)), (int)Math.Round((startingPosY - (sizeOfSquares * 0.5)) * (1 / sizeOfSquares)), qValues) == "up" && startingPosY > (sizeOfSquares * 0.5))
                    {
                        if (startingPosY != (positionOfBlocks[i,1] * sizeOfSquares) + ((sizeOfSquares * 0.5) * 3) || startingPosX != (positionOfBlocks[i,0] * sizeOfSquares) + (sizeOfSquares * 0.5))
                        {
                            indication = "up";
                            movement = 0;
                        }
                        else
                        {
                            indication = null;
                            return;
                        }
                    }
                    if (time % speedOfSimulation == 0 && getDirection((int)Math.Round((startingPosX - (sizeOfSquares * 0.5)) * (1 / sizeOfSquares)), (int)Math.Round((startingPosY - (sizeOfSquares * 0.5)) * (1 / sizeOfSquares)), qValues) == "down" && startingPosY < (rows * sizeOfSquares) - (sizeOfSquares * 0.5))
                    {
                        if (startingPosY != (positionOfBlocks[i,1] * sizeOfSquares) - (sizeOfSquares * 0.5) || startingPosX != (positionOfBlocks[i,0] * sizeOfSquares) + (sizeOfSquares * 0.5))
                        {
                            indication = "down";
                            movement = 0;
                        }
                        else
                        {
                            indication = null;
                            return;
                        }
                    }
                    if (time % speedOfSimulation == 0 && getDirection((int)Math.Round((startingPosX - (sizeOfSquares * 0.5)) * (1 / sizeOfSquares)), (int)Math.Round((startingPosY - (sizeOfSquares * 0.5)) * (1 / sizeOfSquares)), qValues) == "right" && startingPosX < (columns * sizeOfSquares) - (sizeOfSquares * 0.5))
                    {
                        if (startingPosY != (positionOfBlocks[i,1] * sizeOfSquares) + (sizeOfSquares * 0.5) || startingPosX != (positionOfBlocks[i,0] * sizeOfSquares) - (sizeOfSquares * 0.5))
                        {
                            indication = "right";
                            movement = 0;
                        }
                        else
                        {
                            indication = null;
                            return;
                        }
                    }
                    if (time % speedOfSimulation == 0 && getDirection((int)Math.Round((startingPosX - (sizeOfSquares * 0.5)) * (1 / sizeOfSquares)), (int)Math.Round((startingPosY - (sizeOfSquares * 0.5)) * (1 / sizeOfSquares)), qValues) == "left" && startingPosX > (sizeOfSquares * 0.5))
                    {
                        if (startingPosY != (positionOfBlocks[i,1] * sizeOfSquares) + (sizeOfSquares * 0.5) || startingPosX != (positionOfBlocks[i,0] * sizeOfSquares) + ((sizeOfSquares * 0.5) * 3))
                        {
                            indication = "left";
                            movement = 0;
                        }
                        else
                        {
                            indication = null;
                            return;
                        }
                    }
                }
                if (indication != null)
                    move(indication);
                if (movement == (float)movementVelSigma || indication == null)
                {
                    impatience++;
                }
                else
                {
                    impatience = 0;
                }
                if (impatience == 5)
                {
                    startingPosX = roundTo(startingPosX - (sizeOfSquares * 0.5f), (int)sizeOfSquares) + (sizeOfSquares * 0.5f);
                    startingPosY = roundTo(startingPosY - (sizeOfSquares * 0.5f), (int)sizeOfSquares) + (sizeOfSquares * 0.5f);
                }
                if (time % timeBeforeTermination == 0 && time != 0 || impatience == 60)
                {
                    for (int i = 0; i < 1; i++)
                        terminate();
                }
            }
            void move(string direction)
            {
                movement++;
                if (movement > movementVelSigma)
                    movement = (float)movementVelSigma;
                switch (direction)
                {
                    case "right":
                        startingPosX += (float)movementVelSigma - movement;
                        break;
                    case "left":
                        startingPosX -= (float)movementVelSigma - movement;
                        break;
                    case "up":
                        startingPosY -= (float)movementVelSigma - movement;
                        break;
                    case "down":
                        startingPosY += (float)movementVelSigma - movement;
                        break;
                }
            }
            int roundTo(double i, int v)
            {
                return (int)(Math.Round(i / v) * v);
            }
        }
    }


