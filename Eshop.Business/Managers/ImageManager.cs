using Eshop.Business.Classes;
using Eshop.Business.Interfaces;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Business.Managers
{
    public class ImageManager : IImageManager
    {
        public string OutputDirectoryPath {  get; set; }

        private ReadOnlySpan<byte> ResizeImage(SKStream ImageStream, int width = 0,  int height = 0, ImageExtension? newExtension = null)
        {
            using var skData = SKData.Create(ImageStream);
            using var codec = SKCodec.Create(skData);
            using var bitmap = SKBitmap.Decode(codec);

            float newWidth = bitmap.Width;
            float newHeight = bitmap.Height;

            float widthResizeFactor = width / (float)newWidth;
            float heightResizeFactor = height / (float)newHeight;
            widthResizeFactor = widthResizeFactor == 0 ? 1 : widthResizeFactor;
            heightResizeFactor = heightResizeFactor == 0 ? 1 : heightResizeFactor;

            if (width == 0 || height == 0)
            {
                newWidth *= width > 0 ? widthResizeFactor : heightResizeFactor;
                newHeight *= height > 0 ? widthResizeFactor : heightResizeFactor;
            }

            using var newBitmap = new SKBitmap((int)Math.Round(newWidth), (int)Math.Round(newHeight), bitmap.ColorType, bitmap.AlphaType);
            bitmap.ScalePixels(newBitmap, SKFilterQuality.High);

            return newBitmap.Encode(newExtension is null ? codec.EncodedFormat : ExtensionToSKFormat(newExtension.Value), 90).AsSpan();
        }

        private SKEncodedImageFormat ExtensionToSKFormat(ImageExtension extension)
        {
            return extension switch
            {
                ImageExtension.Gif => SKEncodedImageFormat.Gif,
                ImageExtension.Jpeg => SKEncodedImageFormat.Jpeg,
                ImageExtension.Png => SKEncodedImageFormat.Png,
                ImageExtension.Bmp => SKEncodedImageFormat.Bmp,
                _ => throw new NotImplementedException()
            };
        }

        private string ExtensionToString(ImageExtension extension)
        {
            return extension switch
            {
                ImageExtension.Gif => "gif",
                ImageExtension.Jpeg => "jpeg",
                ImageExtension.Png => "png",
                ImageExtension.Bmp => "bmp",
                _ => throw new NotImplementedException()
            };
        }
    }
}
