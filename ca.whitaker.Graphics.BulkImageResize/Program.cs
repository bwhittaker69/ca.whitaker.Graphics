using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter source file or directory path:");
        string sourcePath = Console.ReadLine();

        // Prompt for target directory
        Console.WriteLine("Enter target directory path:");
        string targetDirectory = Console.ReadLine();

        // Check if target directory exists, create if not
        if (!Directory.Exists(targetDirectory))
        {
            Console.WriteLine("Target directory does not exist. Creating it now...");
            Directory.CreateDirectory(targetDirectory);
        }

        // Prompt for resize size
        Console.WriteLine("Enter the size for resizing (e.g., 12, 24, 36, 48):");
        if (!int.TryParse(Console.ReadLine(), out int size))
        {
            Console.WriteLine("Invalid size. Please enter a valid number.");
            return;
        }

        // Prompt for grayscale option
        Console.WriteLine("Apply grayscale? (yes/no)");
        bool grayscale = Console.ReadLine().Trim().ToLower() == "yes";

        // Determine if the source path is a file or a directory
        if (File.Exists(sourcePath))
        {
            // It's a file, process single file
            ProcessFile(sourcePath, targetDirectory, size, grayscale);
        }
        else if (Directory.Exists(sourcePath))
        {
            // It's a directory, process all files in directory
            ProcessDirectory(sourcePath, targetDirectory, size, grayscale);
        }
        else
        {
            Console.WriteLine("The specified path does not exist as a file or directory.");
            return;
        }
    }

    static void ProcessDirectory(string sourceDir, string targetDir, int size, bool grayscale)
    {
        foreach (string file in Directory.GetFiles(sourceDir, "*.png"))
        {
            ProcessFile(file, targetDir, size, grayscale);
        }
    }

    static void ProcessFile(string filePath, string targetDir, int size, bool grayscale)
    {
        try
        {
            using (Image image = Image.FromFile(filePath))
            {
                using (Bitmap newImage = new Bitmap(image, new Size(size, size)))
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath) + $"_{size}.png";
                    string savePath = Path.Combine(targetDir, fileName);
                    newImage.Save(savePath, ImageFormat.Png);

                    if (grayscale)
                    {
                        using (Bitmap grayscaleImage = ConvertToGrayscale(new Bitmap(newImage)))
                        {
                            fileName = Path.GetFileNameWithoutExtension(fileName) + "_grayscale.png";
                            savePath = Path.Combine(targetDir, fileName);
                            grayscaleImage.Save(savePath, ImageFormat.Png);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
        }
    }

    static Bitmap ConvertToGrayscale(Bitmap original)
    {
        Bitmap grayscale = new Bitmap(original.Width, original.Height);

        using (Graphics g = Graphics.FromImage(grayscale))
        {
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
