using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Prompt for source directory
        Console.WriteLine("Enter source directory path:");
        string sourceDirectory = Console.ReadLine();

        // Prompt for target directory
        Console.WriteLine("Enter target directory path:");
        string targetDirectory = Console.ReadLine();

        // Check if source directory exists
        if (!Directory.Exists(sourceDirectory))
        {
            Console.WriteLine("Source directory does not exist.");
            return;
        }

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

        // Process images
        ResizeImages(sourceDirectory, targetDirectory, size, grayscale);
    }

    static void ResizeImages(string sourceDir, string targetDir, int size, bool grayscale)
    {
        // Iterate over each file in the source directory
        foreach (string file in Directory.GetFiles(sourceDir, "*.png"))
        {
            try
            {
                // Load the image
                using (Image image = Image.FromFile(file))
                {
                    // Resize the image
                    using (Bitmap newImage = new Bitmap(image, new Size(size, size)))
                    {
                        // Save the resized image
                        string fileName = Path.GetFileNameWithoutExtension(file) + $"_{size}.png";
                        string savePath = Path.Combine(targetDir, fileName);
                        newImage.Save(savePath, ImageFormat.Png);

                        // If grayscale is applied, convert and save the grayscale image
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
                // Handle any errors during processing
                Console.WriteLine($"Error processing file {file}: {ex.Message}");
            }
        }
    }

    static Bitmap ConvertToGrayscale(Bitmap original)
    {
        // Create a new empty bitmap for the grayscale image
        Bitmap grayscale = new Bitmap(original.Width, original.Height);

        // Use graphics to edit the bitmap
        using (Graphics g = Graphics.FromImage(grayscale))
        {
            // Set up the color matrix for grayscale conversion
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
                // Apply the color matrix to the original image
                attributes.SetColorMatrix(colorMatrix);
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                            0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }
        }

        // Return the grayscale image
        return grayscale;
    }
}
