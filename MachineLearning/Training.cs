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

namespace EEMod
{
    public class Trainer
    {
        public double[] inputs;
        public float[] kerneledInputs;
        public float[] answer;
        public List<float> term = new List<float>();
        public double[] input;
        public int column;
        public Trainer(double[] input, float[] a, int column)
        {
            this.input = input;
            this.column = column;
            answer = a;
        }
    }
}