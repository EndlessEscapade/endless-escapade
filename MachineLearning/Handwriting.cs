using EEMod.Extensions;
using EEMod.Items.Placeables.Furniture;
using IL.Terraria.GameContent.UI.Elements;
using log4net;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using Terraria;
using Terraria.ModLoader;
using Excel = Microsoft.Office.Interop.Excel;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace EEMod.MachineLearning
{
    public class Handwriting : NeuralNetwork
    {
        public override int sizeOfData => 5000;
        public override int SIZEOFINPUTS => 169;
        public int SIZEOFUNPROCESSEDINPUTS => 784;
        public override int NumberOfKernels => 2;
        public override int NumberOfClassifications => 10;
        public override float LearningRate => 0.01f;
        public override int IMAGEHEIGHT => 28;
        public override int IMAGEWIDTH => 28;
        public override string PerceptronSavePath => $@"{Main.SavePath}\Mod Sources\EEMod\MachineLearning\PerceptronData.TrivBadMod";

        public int percDoneWithLoading;

        public float Prediction;
        public float Answer;
        double[] UIInputs = new double[784];
        public List<float> Vals = new List<float>();
        public float[] GotoVals = new float[10];
        public void Clear()
        {
            for(int i = 0; i<UIInputs.Length; i++)
            {
                UIInputs[i] = 0;
            }
        }
        public void Draw()
        {
            int PixelSize = 13;
            Vector2 StartPoint = Main.screenPosition.ForDraw() + new Vector2(0,100);
            Rectangle MouseSquare = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 2, 2);
            if(Main.LocalPlayer.controlHook)
            {
                SerliazeCurrentPerceptron();
                Main.NewText("ObjectSaved!");
            }
            if(EEMod.Train.JustPressed)
            {
                Clear();
            }
            for (int i = 0; i < UIInputs.Length; i++)
            {
                Rectangle box = new Rectangle((int)StartPoint.X + (i % IMAGEWIDTH) * PixelSize, (int)StartPoint.Y + (i / IMAGEHEIGHT) * PixelSize, PixelSize, PixelSize);
                if (MouseSquare.Intersects(box) && Main.LocalPlayer.controlUseItem)
                {
                    UIInputs[i] = 1;
                    if (i - IMAGEWIDTH > 0)
                    {
                        UIInputs[i - IMAGEWIDTH] += 0.01f;
                    }
                    if (i - 1 > 0)
                    {
                        UIInputs[i - 1] += 0.01f;
                    }
                    if (i + 1 < SIZEOFUNPROCESSEDINPUTS)
                    {
                        UIInputs[i + 1] += 0.01f;
                    }
                    if (i + IMAGEWIDTH < SIZEOFUNPROCESSEDINPUTS)
                    {
                        UIInputs[i + IMAGEWIDTH] += 0.01f;
                    }
                }
                UIInputs[i] = Math.Min(UIInputs[i], 1);
                Main.spriteBatch.Draw(Main.magicPixel, box, Color.Lerp(Color.Black,Color.White, (float)UIInputs[i]));
            }
            for(int i = 0; i <Vals.Count; i++)
            {
                GotoVals[i] += (Vals[i] - GotoVals[i]) / 64f;
                int Seperation = 20;
                int Length = 500;
                Rectangle box = new Rectangle((int)StartPoint.X + IMAGEWIDTH * PixelSize, (int)StartPoint.Y + i* Seperation, (int)(Length* GotoVals[i]), 20);
                EEMod.UIText(i.ToString(), Color.White, new Vector2((int)StartPoint.X + IMAGEWIDTH * PixelSize - 10, (int)StartPoint.Y + i * Seperation), 1);
                Main.spriteBatch.Draw(Main.magicPixel, box, Color.Lerp(Color.Red,Color.Green, GotoVals[i]));
            }
            try
            {
                if (File.Exists(PerceptronSavePath))
                {
                    Main.NewText(PredictPicture(UIInputs, DeserializeSavedPerceptron())[3]);
                    Vals = PredictPicture(UIInputs, DeserializeSavedPerceptron());
                }
            }
            catch
            {
                if(Main.rand.Next(40) == 1)
                Main.NewText("Failed");
            }
        }
        int[] convolutionalFilter1 =
        {
        -1, 0, 1,
        -1, 0, 1,
        -1, 0, 1
        };

        int[] convolutionalFilter2 =
        {
         -1,-1,-1,
          0, 0, 0,
          1, 1, 1
        };
        public override void OnUpdate()
        {
            if (isActive)
            {
                MoveToNext();
                FeedForward();
                Error();
                Train();
                Prediction = MainPerceptron.firstHiddenLayer.IndexOf(MainPerceptron.firstHiddenLayer.Max());
                float maxValue = objTrainer[CurrentData].answer.Max();
                Answer = objTrainer[CurrentData].answer.ToList().IndexOf(maxValue);
                Layers.errors.Add(ERROR);
            }
        }
        List<float> PredictPicture(double[] inputs, Perceptron ptron)
        {
            float[] KerneledInputs;
            float[][] Kernels =
               {
                 KernelMultidimensionalArray3x3<float>(inputs, IMAGEWIDTH, IMAGEHEIGHT, convolutionalFilter1,1),
                 KernelMultidimensionalArray3x3<float>(inputs, IMAGEWIDTH, IMAGEHEIGHT, convolutionalFilter2,1)
               };

            float[][] Pools =
              {
                 MaxPoolMultiDimensionalArray<float>(Kernels[0], IMAGEWIDTH - 2,IMAGEHEIGHT - 2,2,2),
                 MaxPoolMultiDimensionalArray<float>(Kernels[1], IMAGEWIDTH - 2,IMAGEHEIGHT - 2,2,2)
              };

            KerneledInputs = Flatten<float>(Pools);
            return ptron.FeedForward(KerneledInputs);
        }
        public override void OnInitialize()
        {
            //isActive = true;
           // MainPerceptron = new Perceptron(SIZEOFINPUTS * NumberOfKernels, SIZEOFINPUTS, SIZEOFINPUTS / 2, NumberOfClassifications, 0.01f);
            //CreateNewDataSet($@"C:\Users\tafid\Desktop\processing-3.5.4-windows64\processing-3.5.4\Handwriting\mnist_test");
        }
            public void CreateNewDataSet(string TrainingInputsExcel)
            {
                Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(TrainingInputsExcel);
            Excel._Worksheet xlWorksheet = (Excel._Worksheet)xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;
                  dynamic[,] excelArray = xlRange.Value2 as dynamic[,];
                  objTrainer = new Trainer[sizeOfData];
                  CurrentData = 0;
                  for (int i = 0; i < objTrainer.Length; i++)
                  {
                      percDoneWithLoading = i;
                      int currentRow = Main.rand.Next(2, xlRange.Rows.Count - 1);
                      double[] inputs = new double[SIZEOFUNPROCESSEDINPUTS];
                      for (int a = 0; a < SIZEOFUNPROCESSEDINPUTS; a++)
                      {
                          inputs[a] = excelArray[currentRow, a + 1] / (double)255;
                      }
                      float[] answers = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                      answers[(int)excelArray[currentRow, 1]] = 1;
                      objTrainer[i] = new Trainer(inputs, answers, currentRow);

                      float[][] Kernels =
                      {
                       KernelMultidimensionalArray3x3<float>(inputs, IMAGEWIDTH, IMAGEHEIGHT, convolutionalFilter1,1),
                       KernelMultidimensionalArray3x3<float>(inputs, IMAGEWIDTH, IMAGEHEIGHT, convolutionalFilter2,1)
                     };

                      float[][] Pools =
                      {
                       MaxPoolMultiDimensionalArray<float>(Kernels[0], IMAGEWIDTH - 2,IMAGEHEIGHT - 2,2,2),
                       MaxPoolMultiDimensionalArray<float>(Kernels[1], IMAGEWIDTH - 2,IMAGEHEIGHT - 2,2,2)
                      };

                      objTrainer[i].kerneledInputs = Flatten<float>(Pools);
                  }
                  Console.WriteLine("Done!");
                  xlWorkbook.Close(true, null, null);
                  xlApp.Quit();

                  Marshal.ReleaseComObject(xlWorksheet);
                  Marshal.ReleaseComObject(xlWorkbook);
                  Marshal.ReleaseComObject(xlApp);
              }
            }
    }


