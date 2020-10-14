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
    [Serializable]
    public class Perceptron : MLObject
    {
        float[,] weights1;
        float[,] weights2;
        float[,] weights3;
        float c;
        int noOfLayers;
        int noOfLayersSecond;
        int noOfInputs;
        int noOfLayersFinal;
        float[] biases1;
        float[] biases2;
        float[] biases3;
        float weightVariation = 0.1f;
        float biasVariation = 0;
        public List<float> firstHiddenLayer = new List<float>();
        public List<float> secondHiddenLayer = new List<float>();
        public float[] finalLayerHold;
        public List<float> outputLayer = new List<float>();
        double e = 2.7182818284590452353602874713527;

        //[end]

        public Perceptron(int noOfInputs, int noOfLayers, int noOfLayersSecond, int noOfLayersFinal, float learningRate)
        {
            weights1 = new float[noOfLayers, noOfInputs];
            weights2 = new float[noOfLayersSecond, noOfLayers];
            weights3 = new float[noOfLayersFinal, noOfLayersSecond];
            biases1 = new float[noOfLayers];
            biases2 = new float[noOfLayersSecond];
            biases3 = new float[noOfLayersFinal];
            this.noOfInputs = noOfInputs;
            this.noOfLayers = noOfLayers;
            this.noOfLayersSecond = noOfLayersSecond;
            this.noOfLayersFinal = noOfLayersFinal;
            finalLayerHold = new float[noOfLayersFinal];
            c = learningRate;
            for (int z = 0; z < noOfLayers; z++)
            {
                for (int x = 0; x < noOfInputs; x++)
                {
                    weights1[z, x] = Main.rand.NextFloat(-weightVariation, weightVariation);
                }
            }
            for (int v = 0; v < noOfLayersSecond; v++)
            {
                for (int n = 0; n < noOfLayers; n++)
                {
                    weights2[v, n] = Main.rand.NextFloat(-weightVariation, weightVariation);
                }
            }
            for (int r = 0; r < noOfLayersFinal; r++)
            {
                for (int t = 0; t < noOfLayersSecond; t++)
                {
                    weights3[r, t] = Main.rand.NextFloat(-weightVariation, weightVariation);
                }
            }
            for (int i = 0; i < biases1.Length; i++)
            {
                biases1[i] = Main.rand.NextFloat(-biasVariation, biasVariation);
            }
            for (int i = 0; i < biases2.Length; i++)
            {
                biases2[i] = Main.rand.NextFloat(-biasVariation, biasVariation);
            }
            for (int i = 0; i < biases3.Length; i++)
            {
                biases3[i] = Main.rand.NextFloat(-biasVariation, biasVariation);
            }

            //[end]
        }
        public List<float> FeedForward(float[] input)
        {
            List<float> activationArray = Sigmoid(input.ToList(), noOfLayers, weights1, biases1);
            List<float> secondLayer = Sigmoid(activationArray, noOfLayersSecond, weights2, biases2);
            float[] finalLayerHolder = new float[noOfLayersFinal];
            List<float> finalLayer = SoftMax(secondLayer, noOfLayersFinal, weights3, biases3);
            firstHiddenLayer = activationArray;
            secondHiddenLayer = secondLayer;
            finalLayerHold = finalLayerHolder;
            outputLayer = finalLayer;
            return finalLayer;
        }

        List<float> Sigmoid(List<float> inputs, int NumberOfNeuronsNextLayer, float[,] weights, float[] biases)
        {
            float[] secondLayerHolder = new float[NumberOfNeuronsNextLayer];
            List<float> secondLayer = new List<float>();
            for (int i = 0; i < NumberOfNeuronsNextLayer; i++)
            {
                for (int j = 0; j < inputs.Count; j++)
                {
                    secondLayerHolder[i] += inputs[j] * weights[i, j];
                }
                secondLayerHolder[i] += biases[i];
                secondLayer.Add(1 / (1 + (float)Math.Pow(e, secondLayerHolder[i] * -1)));
            }
            return secondLayer;
        }
        List<float> SoftMax(List<float> inputs, int NumberOfNeuronsNextLayer, float[,] weights, float[] biases)
        {
            float[] finalLayerHolder = new float[NumberOfNeuronsNextLayer];
            List<float> finalLayer = new List<float>();
            for (int i = 0; i < NumberOfNeuronsNextLayer; i++)
            {
                float sumOfExponents = 0;
                for (int j = 0; j < inputs.Count; j++)
                {
                    finalLayerHolder[i] += inputs[j] * weights[i, j];
                }
                finalLayerHolder[i] += biases[i];
                for (int h = 0; h < noOfLayersFinal; h++)
                {
                    sumOfExponents += (float)Math.Pow(e, finalLayerHolder[h]);//softmax
                }
                finalLayer.Add(((float)Math.Pow(e, finalLayerHolder[i])) / (float)(sumOfExponents));
            }
            return finalLayer;
        }

        public float getError(float[] desired, List<float> input)
        {
            float errors = 0;
            float error = 0;
            for (int i = 0; i < input.Count; i++)
            {
                errors += -(float)((desired[i] * Math.Log(input[i])));//cross entropy
                error = errors / 1f;
            }
            return error;
        }
        public float getIndexOfLargest(float[] array)
        {
            if (array == null || array.Length == 0) return -1; // null or empty

            int largest = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > array[largest]) largest = i;
            }
            return largest; // position of the first largest found
        }
        public void Train(List<float> finalLayer, float[] desired, float[] input, List<float> inputToSecond, List<float> inputToThird)
        {
            float[] error = new float[noOfLayersFinal];
            float[] dActivate20 = new float[noOfLayersSecond];
            float[] dActivate10 = new float[noOfLayers];
            float[] hiddenLayerOutputs2 = new float[noOfLayersSecond];
            float[] hiddenLayerOutputs1 = new float[noOfLayers];
            float[] sigmaDerivative2 = new float[noOfLayersSecond];
            float[] sigmaDerivative1 = new float[noOfLayers];
            float finalSigma = 0;
            for (int i = 0; i < noOfLayersFinal; i++)
            {
                error[i] = finalLayer[i] - desired[i];
            }
            for (int a = 0; a < noOfLayersSecond; a++)
            {
                dActivate20[a] = inputToThird[a] * (1 - inputToThird[a]);
                hiddenLayerOutputs2[a] = inputToThird[a];
                for (int b = 0; b < noOfLayersFinal; b++)
                {
                    sigmaDerivative2[a] += (error[b]) * weights3[b, a];
                }
            }
            for (int a = 0; a < noOfLayersSecond; a++)
            {
                finalSigma += sigmaDerivative2[a];
            }
            for (int a = 0; a < noOfLayers; a++)
            {
                dActivate10[a] = inputToSecond[a] * (1 - inputToSecond[a]);
                hiddenLayerOutputs1[a] = inputToSecond[a];
                for (int c = 0; c < noOfLayersSecond; c++)
                {
                    sigmaDerivative1[a] += weights2[c, a] * dActivate20[c];
                }
            }


            //----------------------------------------------------------------------------
            for (int j = 0; j < noOfLayersFinal; j++)
            {
                biases3[j] -= c * (error[j]);
                for (int i = 0; i < noOfLayersSecond; i++)
                {
                    weights3[j, i] -= c * ((error[j]) * inputToThird[i]);
                }
            }

            for (int j = 0; j < noOfLayersSecond; j++)
            {
                biases2[j] -= c * ((sigmaDerivative2[j] * dActivate20[j]));
                for (int i = 0; i < noOfLayers; i++)
                {
                    weights2[j, i] -= c * ((sigmaDerivative2[j] * dActivate20[j] * hiddenLayerOutputs1[i]));
                }
            }

            for (int j = 0; j < noOfLayers; j++)
            {
                biases1[j] -= c * (finalSigma * dActivate10[j] * sigmaDerivative1[j]);
                for (int i = 0; i < noOfInputs; i++)
                {
                    weights1[j, i] -= c * (finalSigma * input[i] * dActivate10[j] * sigmaDerivative1[j]);
                }
            }
        }
    }
}