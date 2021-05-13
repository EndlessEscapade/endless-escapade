using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Extensions
{
    public static class TextExtensions
    {
        /// <summary>Formats the text with automatic new lines.</summary>
        /// <param name="breakPoint">The limit of characters in a single line.</param>
        public static string FormatString(this string text, int breakPoint)
        {
            string formattedText = "";
            int lineBreakPoint = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (lineBreakPoint >= breakPoint)
                {
                    if (!char.IsWhiteSpace(text[i]))
                    {
                        int failsafe = 1;
                        string yeaCat = "";
                        while (!char.IsWhiteSpace(text[i - failsafe]))
                        {
                            yeaCat += text[i - failsafe];
                            failsafe++;
                        }
                        lineBreakPoint = 0;
                        formattedText += "\n";
                        string naCat = new string(yeaCat.Reverse().ToArray());
                        formattedText = formattedText.Replace(" " + naCat, "");
                        formattedText += naCat;
                    }
                    else
                    {
                        lineBreakPoint = 0;
                        formattedText += "\n";
                    }
                }
                formattedText += text[i];
                lineBreakPoint++;
            }
            return formattedText;
        }
    }
}