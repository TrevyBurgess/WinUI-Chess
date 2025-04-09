//
//
namespace CyberFeedForward.ViewModelBase.Helpers;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;

public static class ResourceHelper
{
    /// <summary>
    /// Retrieves an image based on the provided byte array and size parameter.
    /// </summary>
    /// <param name="imageData">The byte array contains the raw data of the image to be processed.</param>
    /// <param name="size">The size parameter determines the dimensions of the resulting image.</param>
    /// <returns>Returns an Image object created from the specified byte array and size.</returns>
    public static Image GetImage(byte[] imageData, double size)
    {
        if (imageData == null || imageData.Length == 0)
        {
            throw new ArgumentException("Image data cannot be null or empty.", nameof(imageData));
        }

        using var stream = new MemoryStream(imageData);
        var bitmapImage = new BitmapImage();
        stream.Position = 0;
        bitmapImage.SetSource(stream.AsRandomAccessStream());

        var image = new Image
        {
            Source = bitmapImage,
            Width = size,
            Height = size
        };

        return image;
    }

    /// <summary>
    /// Retrieves an image source from a byte array containing image data.
    /// </summary>
    /// <param name="imageData">The byte array contains the raw data of the image to be converted.</param>
    /// <returns>An image source that can be used for displaying the image.</returns>
    public static ImageSource GetImageSource(byte[] imageData)
    {
        if (imageData == null || imageData.Length == 0)
        {
            throw new ArgumentException("Image data cannot be null or empty.", nameof(imageData));
        }

        using var stream = new MemoryStream(imageData);
        var bitmapImage = new BitmapImage();
        stream.Position = 0;
        bitmapImage.SetSource(stream.AsRandomAccessStream());

        return bitmapImage;
    }
}
