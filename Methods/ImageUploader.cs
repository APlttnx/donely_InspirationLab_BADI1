using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace donely_Inspilab.Methods
{
    public static class ImageUploader
    {
        public static (BitmapImage image, string fileName)? UploadImage(string targetFolder)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png"
            };

            if (dialog.ShowDialog() != true)
                return null;

            string sourcePath = dialog.FileName;
            string fileName = Path.GetFileName(sourcePath);
            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, targetFolder);

            try
            {
                if (!Directory.Exists(imagesFolder))
                    Directory.CreateDirectory(imagesFolder);

                string destinationPath = Path.Combine(imagesFolder, fileName);
                File.Copy(sourcePath, destinationPath, overwrite: true);

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(destinationPath, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                return (bitmap, fileName);
            }
            catch
            {
                throw;
            }
        }
    }

}
