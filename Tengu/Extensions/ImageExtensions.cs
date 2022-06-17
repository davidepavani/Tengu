using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Extensions
{
    public static class ImageExtensions
    {
        // This is a Tacconata
        // Because AsyncImageLoader sometimes throw an exception "Value cannot be null"
        // so, i need to use an IControl
        public static Image InitializeImage(this string source, Stretch stretch)
        {
            Image image = new()
            {
                Stretch = stretch
            };
            AsyncImageLoader.ImageLoader.SetSource(image, source);
            return !string.IsNullOrEmpty(source) ? image : null;
        }
    }
}
