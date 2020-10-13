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
        public override int SIZEOFINPUTS => objTrainer[0].kerneledInputs.Length;
        public override int NumberOfKernels => 2;
        public override int NumberOfClassifications => 10;
        public override float LearningRate => 0.01f;

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

        public override void OnInitialize()
        {
            MainPerceptron = new Perceptron(SIZEOFINPUTS*NumberOfKernels, SIZEOFINPUTS, SIZEOFINPUTS/2, NumberOfClassifications, 0.01f);
        }

        public void CreateNewDataSet(string TrainingInputsExcel)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(TrainingInputsExcel);
            Excel._Worksheet xlWorksheet = (Excel._Worksheet)xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            objTrainer = new Trainer[sizeOfData];
            CurrentData = 0;
            for (int i = 0; i < objTrainer.Length; i++)
            {
                //currentRow = (int)random(csvReader.getRowCount());
                //csvRow = csvReader.getRow(currentRow);
                double[] inputs = new double[SIZEOFINPUTS];
                for (int a = 0; a < SIZEOFINPUTS; a++)
                {
                   // inputs[a] = (csvRow.getInt(a + 1) / (double)255);
                }
                float[] answers = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
               // answers[csvRow.getInt(0)] = 1;
                //training[i] = new Trainer(inputs, answers, currentRow);

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
        }
    }
}