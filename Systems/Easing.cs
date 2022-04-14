using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod
{
    public static class Easing
    {
        public static float EasingValue(float time, EasingFunc[] easers)
        {
            float returnVal = 0;

            for(int i = 0; i < easers.Length; i++)
            {
                returnVal += easers[i].GetVal(time);
            }

            return returnVal;
        }
    }

    public struct EasingFunc
    {
        public float startThresh;
        public float endThresh;

        public float firstVal;
        public float secondVal;

        public EasingID type;

        public EasingFunc(float startThresh, float endThresh, float firstVal, float secondVal, EasingID type)
        {
            this.startThresh = startThresh;
            this.endThresh = endThresh;

            this.firstVal = firstVal;
            this.secondVal = secondVal;

            this.type = type;
        }

        public float GetVal(float time)
        {
            if (time <= startThresh || time > endThresh) return 0;

            switch (type)
            {
                case EasingID.Linear:
                    return (secondVal - firstVal) / (endThresh - startThresh);
                    break;
                //case EasingID.SineIn:
                //    return Math.Sin();
                //    break;
                default:
                    return 0;

                    break;
            }
        }
    }

    public enum EasingID
    {
        Linear,
        SineIn,
        SineOut,
        QuadIn,
        QuadOut,
    }
}