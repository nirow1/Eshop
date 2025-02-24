using Eshop.Business.Classes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Business.Interfaces
{
    public interface IImageManager
    {
        IImageManager ConfigureOutputPath(string outputPath);
        void ResizeImage(string oldPath, string newPath, int width = 0, int height = 0);
        void SaveImage(IFormFile file, string fileName, ImageExtension extention, int width = 0, int height = 0);
    }
}
