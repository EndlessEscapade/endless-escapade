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
    public class NeuronInterface
    {
        public List<float> finalLayer = new List<float>();

        public List<List<float>> finalLayerHolder = new List<List<float>>();

        public List<float> answerLayer = new List<float>();

        public List<List<float>> answerHolder = new List<List<float>>();

        public List<float> errors = new List<float>();
    }
}