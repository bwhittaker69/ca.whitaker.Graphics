﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string sourceDirectory = @"E:\source\repos\ca.whitaker.Graphics\ca.whitaker.Graphics.BulkImageResize";
        string targetDirectory = @"E:\source\repos\ca.whitaker.Graphics\ca.whitaker.Graphics.BulkImageResize\Output";

        ResizeImages(sourceDirectory, targetDirectory, 12);
        ResizeImages(sourceDirectory, targetDirectory, 24);
        ResizeImages(sourceDirectory, targetDirectory, 36);
        ResizeImages(sourceDirectory, targetDirectory, 48);
    }

    static void ResizeImages(string sourceDir, string targetDir, int size)
    {
        if (!Directory.Exists(targetDir))
            Directory.CreateDirectory(targetDir);

        foreach (string file in System.IO.Directory.GetFiles(sourceDir, "*.png"))
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
    }
}