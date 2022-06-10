using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Systems
{
    //Credit to IbanPlay / Calamity
    public class Easing
    {
        public enum EasingType
        {
            Linear,
            SineIn, SineOut, SineInOut, SineBump,
            PolyIn, PolyOut, PolyInOut,
            ExpIn, ExpOut, ExpInOut,
            CircIn, CircOut, CircInOut
        }

        /// <summary>
        /// This represents a part of a piecewise function.
        /// </summary>
        public struct CurveSegment
        {
            /// <summary>
            /// This is the type of easing used in the segment
            /// </summary>
            public EasingType mode;
            /// <summary>
            /// This indicates when the segment starts on the animation
            /// </summary>
            public float originX;
            /// <summary>
            /// This indicates what the starting height of the segment is
            /// </summary>
            public float originY;
            /// <summary>
            /// This represents the elevation shift that will happen during the segment. Set this to 0 to turn the segment into a flat line.
            /// Usually this elevation shift is fully applied at the end of a segment, but the sinebump easing type makes it be reached at the apex of its curve.
            /// </summary>
            public float displacement;
            /// <summary>
            /// This is the degree of the polynomial, if the easing mode chosen is a polynomial one
            /// </summary>
            public int degree;

            public CurveSegment(EasingType MODE, float ORGX, float ORGY, float DISP, int DEG = 1)
            {
                mode = MODE;
                originX = ORGX;
                originY = ORGY;
                displacement = DISP;
                degree = DEG;
            }
        }

        /// <summary>
        /// This gives you the height of a custom piecewise function for any given X value, so you may create your own complex animation curves easily.
        /// The X value is automatically clamped between 0 and 1, but the height of the function may go beyond the 0 - 1 range
        /// </summary>
        /// <param name="progress">How far along the curve you are. Automatically clamped between 0 and 1</param>
        /// <param name="segments">An array of curve segments making up the full animation curve</param>
        /// <returns></returns>
        public static float PiecewiseAnimation(float progress, CurveSegment[] segments)
        {
            if (segments.Length == 0)
                return 0f;

            if (segments[0].originX != 0) //If for whatever reason the user forgets to play by the rules, fix that
                segments[0].originX = 0;

            progress = MathHelper.Clamp(progress, 0f, 1f); //Clamp the progress
            float ratio = 0f;

            for (int i = 0; i <= segments.Length - 1; i++)
            {
                CurveSegment segment = segments[i];
                float startPoint = segment.originX;
                float endPoint = 1f;

                if (progress < segment.originX) //Too early. This should never get reached, since by the time you'd have gotten there you'd have found the appropriate segment and broken out of the for loop
                    continue;

                if (i < segments.Length - 1)
                {
                    if (segments[i + 1].originX <= progress) //Too late
                        continue;
                    endPoint = segments[i + 1].originX;
                }

                float segmentLenght = endPoint - startPoint;
                float segmentProgress = (progress - segment.originX) / segmentLenght; //How far along the specific segment
                ratio = segment.originY;

                switch (segment.mode)
                {
                    case EasingType.Linear:
                        ratio += segmentProgress * segment.displacement;
                        break;
                    //Sines
                    case EasingType.SineIn:
                        ratio += (1f - (float)(Math.Cos(segmentProgress * MathHelper.Pi / 2f))) * segment.displacement;
                        break;
                    case EasingType.SineOut:
                        ratio += (float)Math.Sin(segmentProgress * MathHelper.Pi / 2f) * segment.displacement;
                        break;
                    case EasingType.SineInOut:
                        ratio += (-((float)Math.Cos(segmentProgress * MathHelper.Pi) - 1) / 2f) * segment.displacement;
                        break;
                    case EasingType.SineBump:
                        ratio += ((float)Math.Sin(segmentProgress * MathHelper.Pi)) * segment.displacement;
                        break;
                    //Polynomials
                    case EasingType.PolyIn:
                        ratio += (float)Math.Pow(segmentProgress, segment.degree) * segment.displacement;
                        break;
                    case EasingType.PolyOut:
                        ratio += (1f - (float)Math.Pow(1f - segmentProgress, segment.degree)) * segment.displacement;
                        break;
                    case EasingType.PolyInOut:
                        ratio += (segmentProgress < 0.5f ? (float)Math.Pow(2, segment.degree - 1) * (float)Math.Pow(segmentProgress, segment.degree) : 1f - (float)Math.Pow(-2 * segmentProgress + 2, segment.degree) / 2f) * segment.displacement;
                        break;
                    case EasingType.ExpIn:
                        ratio += (segmentProgress == 0f ? 0f : (float)Math.Pow(2, 10f * segmentProgress - 10f)) * segment.displacement;
                        break;
                    case EasingType.ExpOut:
                        ratio += (segmentProgress == 1f ? 1f : 1f - (float)Math.Pow(2, -10f * segmentProgress)) * segment.displacement;
                        break;
                    case EasingType.ExpInOut:
                        ratio += (segmentProgress == 0f ? 0f : segmentProgress == 1f ? 1f : segmentProgress < 0.5f ? (float)Math.Pow(2, 20f * segmentProgress - 10f) / 2f : (2f - (float)Math.Pow(2, -20f * segmentProgress - 10f)) / 2f) * segment.displacement;
                        break;
                    case EasingType.CircIn:
                        ratio += (1f - (float)Math.Sqrt(1 - Math.Pow(segmentProgress, 2f))) * segment.displacement;
                        break;
                    case EasingType.CircOut:
                        ratio += ((float)Math.Sqrt(1 - Math.Pow(segmentProgress - 1f, 2f))) * segment.displacement;
                        break;
                    case EasingType.CircInOut:
                        ratio += (segmentProgress < 0.5 ? (1f - (float)Math.Sqrt(1 - Math.Pow(2 * segmentProgress, 2f))) / 2f : ((float)Math.Sqrt(1 - Math.Pow(-2f * segmentProgress - 2f, 2f)) + 1f) / 2f) * segment.displacement;
                        break;
                }
                break;
            }
            return ratio;
        }
    }
}