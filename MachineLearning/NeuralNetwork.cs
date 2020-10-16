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

namespace EEMod.MachineLearning
{
    public class NeuralNetwork : MLObject
    {
        public bool isActive;
        internal int EpochNo;
        public virtual string PerceptronSavePath { get; set; }
        internal Serializers Serializer = new Serializers();
        public virtual int TrainStride => 1;
        public Perceptron MainPerceptron = new Perceptron(169 * 2, 169, 80, 10, 0.01f);
        public Trainer[] objTrainer;
        public NeuronInterface Layers = new NeuronInterface();
        public int CurrentData;
        public void MoveToNext()
        {
            CurrentData += TrainStride;
            if (CurrentData > SIZEOFINPUTS - 1)
            {
                CurrentData = 0;
                EpochNo++;
            }
        }
        public void SerliazeCurrentPerceptron() => Serializer.Serialize(MainPerceptron, PerceptronSavePath);
        public Perceptron DeserializeSavedPerceptron() =>  Serializer.Deserialize<Perceptron>(PerceptronSavePath);

        public void FeedForward()
        {
            Layers.finalLayer = MainPerceptron.FeedForward(objTrainer[CurrentData].kerneledInputs);
        }
        public float Error()
        {
            ERROR = MainPerceptron.getError(objTrainer[CurrentData].answer, Layers.finalLayer);
            return ERROR;
        }
        public void Train()
        {
            MainPerceptron.Train(Layers.finalLayer, objTrainer[CurrentData].answer, objTrainer[CurrentData].kerneledInputs, MainPerceptron.firstHiddenLayer, MainPerceptron.secondHiddenLayer);
        }
        internal List<int[,]> ConvolutionalFilters = new List<int[,]>();
        public virtual int IMAGEWIDTH
        {
            get;
            set;
        }
        public float ERROR
        {
            get;
            set;
        }
        public virtual int IMAGEHEIGHT
        {
            get;
            set;
        }

        public string filePathForBackPropInfo
        {
            get;
            set;
        }
        public virtual int UpdateSpeed { get; set; }
        public virtual int SIZEOFINPUTS { get; set; }
        public virtual int NumberOfClassifications { get; set; }
        public virtual int NumberOfKernels { get; set; }
        public virtual float LearningRate { get; set; }
        public virtual int sizeOfData
        {
            get;
            set;
        }
        internal int NoOfEpochs
        {
            get;
            private set;
        }
        public int BatchSize
        {
            get;
            private set;
        }
        public int BatchCount
        {
            get;
            set;
        }
        public float[] MaxPoolMultiDimensionalArray<T>(float[] array, int imageWidth, int imageHeight, int poolWidth, int poolHeight)
        {
            List<float> returningArray = new List<float>();
            for (int k = 0; k < array.Length - imageWidth * (poolHeight - 1); k += poolWidth)
            {
                if (k % imageWidth <= imageWidth - poolWidth && (int)(k / imageWidth) % poolHeight == 0)
                {
                    float startingSum = -10000;
                    for (int i = 0; i < poolHeight; i++)
                    {
                        for (int j = 0; j < poolWidth; j++)
                        {
                            float val = array[k + j + i * imageWidth];
                            if (val > startingSum)
                            {
                                startingSum = val;
                            }
                        }
                    }
                    returningArray.Add(startingSum);
                }
            }
            return returningArray.ToArray();
        }
        public T[] Flatten<T>(float[][] MultiArray)
        {
            List<float> bufferArray = new List<float>();
            foreach (float[] var in MultiArray)
            {
                foreach (float var2 in var)
                {
                    bufferArray.Add(var2);
                }
            }
            return bufferArray.ToArray() as T[];
        }
        public float[] KernelMultidimensionalArray3x3<T>(double[] array, int imageWidth, int imageHeight, int[] convolutionalFilter, int scalar) where T : struct
        {
            List<float> returningArray = new List<float>();
            for (int k = 0; k < array.Length; k++)
            {
                if (k % imageWidth != 0 && k % imageWidth != imageWidth - 1)
                {
                    if (k > imageWidth && k < array.Length - imageWidth - 1)
                    {
                        double innerSum = 0;
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                innerSum += array[k + j + i * imageWidth] * convolutionalFilter[i + 1 + ((j + 1) * 3)] * scalar;
                            }
                        }
                        returningArray.Add((float)Math.Max(innerSum, 0));
                    }
                }
            }
            return returningArray.ToArray();
        }
    }
}