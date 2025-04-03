//
//
namespace TrevyBurgess.Games.TrevyChess.ChessGameUI
{
    using Microsoft.UI.Xaml.Media;
    using Microsoft.UI.Xaml.Media.Imaging;
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;

    public static class HelperMethods
    {
        /// <summary>
        /// Convert a Bitmat into an ImageSource
        /// </summary>
        public static ImageSource GetImageSource(Bitmap theImage)
        {
            Contract.Requires<ArgumentNullException>(theImage != null);

            MemoryStream ms = new MemoryStream();

            theImage.Save(ms, theImage.RawFormat);

            BitmapImage bImg = new BitmapImage();

            bImg.BeginInit();
            bImg.StreamSource = new MemoryStream(ms.ToArray());
            bImg.EndInit();

            return bImg;
        }

        /// <summary>
        /// Convert a Bitmat into an ImageSource
        /// </summary>
        public static ImageSource GetImageSource(System.Drawing.Icon theImage)
        {
            Contract.Requires<ArgumentNullException>(theImage != null);

            return GetImageSource(theImage.ToBitmap());
        }

        /// <summary>
        /// Convert a Bitmat into an ImageSource
        /// </summary>
        public static System.Windows.Controls.Image GetImage(System.Drawing.Bitmap theImage, double size)
        {
            Contract.Requires<ArgumentNullException>(theImage != null);
            Contract.Requires<ArgumentException>(size > 0);

            return new System.Windows.Controls.Image { Source = GetImageSource(theImage), Width = size, Height = size };
        }

        /// <summary>
        /// Convert a Bitmat into an ImageSource
        /// </summary>
        public static System.Windows.Controls.Image GetImage(System.Drawing.Icon theImage, double size)
        {
            Contract.Requires<ArgumentNullException>(theImage != null);
            Contract.Requires<ArgumentException>(size > 0);

            return new System.Windows.Controls.Image { Source = GetImageSource(theImage), Width = size, Height = size };
        }
    }
}
