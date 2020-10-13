using log4net;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Terraria;
using Terraria.ModLoader;
using Excel = Microsoft.Office.Interop.Excel;
namespace EEMod
{
    public class Handwriting : NeuralNetwork
    {
        int[] convolutionalFilter1 =
        {
        -1, 0, 1,
        -1, 0, 1,
        -1, 0, 1
        };

        int[] convolutionalFilter2 =
        {
         -1, -1, -1,
          0, 0, 0,
          1, 1, 1
        };
        public override void OnUpdate()
        {
            if(isActive)
            {
                CurrentData += TrainStride;
                FeedForward();
                Error();
                Train();
                Layers.errors.Add(ERROR);
            }
        }
        void CreateNewDataSet()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"sandbox_test.xlsx");
            Excel._Worksheet xlWorksheet = (Excel._Worksheet)xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            objTrainer = new Trainer[sizeOfData];
            CurrentData = 0;
            for (int i = 0; i < objTrainer.Length; i++)
            {
                //currentRow = (int)random(csvReader.getRowCount());
                //csvRow = csvReader.getRow(currentRow);
                double[] inputs = new double[784];
                for (int a = 0; a < 784; a++)
                {
                   // inputs[a] = (csvRow.getInt(a + 1) / (double)255);
                }
                float[] answers = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
               // answers[csvRow.getInt(0)] = 1;
                //training[i] = new Trainer(inputs, answers, currentRow);


                float[][] Kernels =
                {
                 KernelMultidimensionalArray3x3<float>(inputs, 28, 28, convolutionalFilter1,1),
                 KernelMultidimensionalArray3x3<float>(inputs, 28, 28, convolutionalFilter2,1)
               };

                float[][] Pools =
                {
                 MaxPoolMultiDimensionalArray<float>(Kernels[0], 26,26,2,2),
                 MaxPoolMultiDimensionalArray<float>(Kernels[1], 26,26,2,2)
                };

                objTrainer[i].kerneledInputs = Flatten<float>(Pools);
            }
        }
    }
}