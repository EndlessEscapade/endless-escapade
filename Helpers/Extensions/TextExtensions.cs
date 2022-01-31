using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Extensions
{
    public static class TextExtensions
    {
        public static bool IsHandlingTag;
        /// <summary>Formats the text with automatic new lines.</summary>
        /// <param name="breakPoint">The limit of characters in a single line.</param>
        public static string FormatString(this string text, int breakPoint)
        {
            StringBuilder formattedText = new StringBuilder();
            //StringBuilder naCat = new StringBuilder();
            formattedText.Append(text[0]);
            int lineBreakPoint = 1;
            int tagHandler = 0;
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsWhiteSpace(text[i - 1]) && !char.IsWhiteSpace(text[i]))
                {
                    if (text[i] == '[')
                    {
                        tagHandler += text.IndexOf(':', i) - i;
                    }
                    if ((text.IndexOf(" ", i) == -1 ? text.Length : text.IndexOf(" ", i)) - i + lineBreakPoint + 1 - tagHandler > breakPoint && text[i + 1] != 'i')
                    {
                        formattedText.Append("\n");
                        lineBreakPoint = 0;
                    }
                    if (tagHandler > 0)
                    {
                        lineBreakPoint -= tagHandler;
                        tagHandler = 0;
                    }
                }
                formattedText.Append(text[i]);
                lineBreakPoint++;
                /*
                if (lineBreakPoint >= breakPoint)
                {
                    if (!char.IsWhiteSpace(text[i]) && i != text.Length - 1 && !char.IsWhiteSpace(text[i + 1]))
                    {
                        int charsBehind = 1;
                        naCat.Clear(); // reset the string builder
                        while (!char.IsWhiteSpace(text[i - charsBehind]) && i - charsBehind > -1)
                        {
                            naCat.Append(text[i - charsBehind]);
                            charsBehind++;
                        }
                        string maybCat = new string(naCat.ToString().Reverse().ToArray());
                        lineBreakPoint = maybCat.Length;
                        formattedText.Append("\n");
                        formattedText.Remove(formattedText.Length - maybCat.Length - 1, maybCat.Length);
                        formattedText.Append(maybCat);
                    }
                    else
                    {
                        lineBreakPoint = 0;
                        formattedText.Append("\n");
                    }
                }
                formattedText.Append(text[i]);
                if (text[i] == '[')
                {
                    IsHandlingTag = true;
                }
                if (text[i] == ':')
                {
                    IsHandlingTag = false;
                }
                if (!IsHandlingTag && text[i] != ']')
                {
                    lineBreakPoint++;
                }
                */
            }
            return formattedText.ToString().Replace('§', ' ');
        }
    }
}