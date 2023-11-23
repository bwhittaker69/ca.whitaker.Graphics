using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string sourceDirectory = @"E:\source\repos\ca.whitaker.Graphics\Solution Items";
        string targetDirectory = @"E:\source\repos\ca.whitaker.Graphics\Solution Items\Output";

        ResizeImages(sourceDirectory, targetDirectory, 12, true);
        ResizeImages(sourceDirectory, targetDirectory, 24, true);
        ResizeImages(sourceDirectory, targetDirectory, 36, true);
        ResizeImages(sourceDirectory, targetDirectory, 48, true);

    }

    static void ResizeImages(string sourceDir, string targetDir, int size, bool grayscale)
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
                        using (Bitmap grayscaleImage = ConvertToGrayscale(new Bitmap(newImage)))
                        {
                            fileName = Path.GetFileNameWithoutExtension(fileName) + "_grayscale.png";
                            savePath = Path.Combine(targetDir, fileName);
                            grayscaleImage.Save(savePath, ImageFormat.Png);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file {file}: {ex.Message}");
            }
        }
    }
    static Bitmap ConvertToGrayscale(Bitmap original)
    {
        Bitmap grayscale = new Bitmap(original.Width, original.Height);

        using (Graphics g = Graphics.FromImage(grayscale))
        {
            // Create grayscale color matrix
            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[] { .3f, .3f, .3f, 0, 0 },
                    new float[] { .59f, .59f, .59f, 0, 0 },
                    new float[] { .11f, .11f, .11f, 0, 0 },
                    new float[] { 0, 0, 0, 1, 0 },
                    new float[] { 0, 0, 0, 0, 1 }
                });

            using (ImageAttributes attributes = new ImageAttributes())
            {
                attributes.SetColorMatrix(colorMatrix);
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                            0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }
        }

        return grayscale;
    }

}
