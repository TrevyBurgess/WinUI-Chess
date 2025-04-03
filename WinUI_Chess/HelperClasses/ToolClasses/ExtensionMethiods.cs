//
//
namespace TrevyBurgess.Games.TrevyChess.ChessGameUI
{
    using System;
    using System.Windows.Media;

    /// <summary>
    /// Decorators
    /// </summary>
    public static class ExtensionMethiods
    {
        /// <summary>
        /// Extension method for parsing colors from string resources
        /// </summary>
        public static Color ParseColors(this string colorToParse)
        {
            try
            {
                return (Color)ColorConverter.ConvertFromString(colorToParse);
            }
            catch (FormatException)
            {
                throw new FormatException(colorToParse + " isn't a valid color.");
            }
        }
    }
}
