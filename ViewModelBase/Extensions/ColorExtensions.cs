//
//
namespace CyberFeedForward.ViewModelBase.Extensions;

using System;
using System.Globalization;
using Windows.UI;

/// <summary>
/// Decorators
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Extension method for parsing colors from string resources
    /// </summary>
    public static Color Parse(this string colorToParse)
    {
        try
        {
            if (colorToParse.StartsWith('#'))
            {
                // Parse hex color string
                byte a = 255; // Default alpha
                int startIndex = 1;

                if (colorToParse.Length == 9) // #AARRGGBB
                {
                    a = byte.Parse(colorToParse.Substring(1, 2), NumberStyles.HexNumber);
                    startIndex = 3;
                }

                byte r = byte.Parse(colorToParse.Substring(startIndex, 2), NumberStyles.HexNumber);
                byte g = byte.Parse(colorToParse.Substring(startIndex + 2, 2), NumberStyles.HexNumber);
                byte b = byte.Parse(colorToParse.Substring(startIndex + 4, 2), NumberStyles.HexNumber);

                return Color.FromArgb(a, r, g, b);
            }
        }
        catch (Exception ex) when (ex is FormatException || ex is ArgumentException)
        {
            throw new FormatException(colorToParse + " isn't a valid color.");
        }

        throw new FormatException(colorToParse + " isn't a valid color.");
    }
}
