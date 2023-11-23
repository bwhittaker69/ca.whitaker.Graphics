using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string sourceDirectory = @"E:\source\repos\ca.whitaker.Graphics\Solution Items";
        string targetDirectory = @"E:\source\repos\ca.whitaker.Graphics\Solution Items\Social Media";

        ResizeImages(sourceDirectory, targetDirectory, 12);
        ResizeImages(sourceDirectory, targetDirectory, 24);
        ResizeImages(sourceDirectory, targetDirectory, 36);
        ResizeImages(sourceDirectory, targetDirectory, 48);

        // Optional: Force garbage collection after processing
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    static void ResizeImages(string sourceDir, string targetDir, int size)
    {
        if (!Directory.Exists(targetDir))
            Directory.CreateDirectory(targetDir);

        foreach (string file in Directory.GetFiles(sourceDir, "*.png"))
        {
            try
            {
                using (Image image = Image.FromFile(file))
                {
                    using (Bitmap newImage = new Bitmap(image, new Size(size, size)))
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file) + $"_{size}.png";
                        string savePath = Path.Combine(targetDir, fileName);
                        newImage.Save(savePath, ImageFormat.Png);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file {file}: {ex.Message}");
            }
        }
    }
}
