using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace EdgeDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = args[0], outputFile = args[1];
            Console.WriteLine("Reading image from {0}", filename);
            Bitmap bitmap = new Bitmap(inputFile);
            Console.Write("Image Read. Now processing...", filename);
            var t1 = DateTime.Now;
            Bitmap output = processBitmap(bitmap, Sobel);
            /*
            processBitmap also takes in a lambda expression consisting of two integers (for the index), and the bitmap.
            Bitmap output = processBitmap(bitmap, (i,j,bmp)=>
            {
                var c = bmp.GetPixel(i, j);
                return Color.FromArgb(c.B, 0, 0);
            });
            */
            var t2 = DateTime.Now;
            TimeSpan ts = (t2 - t1);
            Console.Write("processing complete, took {0}ms.\n", ts.TotalMilliseconds);
            Console.WriteLine("Now writing image to {0}", outputFile);
            output.Save(outputFile);
        }

        private static Bitmap processBitmap(Bitmap bitmap,Func<int,int,Bitmap,Color> func)
        {
            int height = bitmap.Height-2, width = bitmap.Width-2;
            Bitmap temp = new Bitmap(width, height);
            for (int j = 1; j < height; j++)
            {
                for (int i = 1; i < width; i++)
                {
                    Color outputColor = func(i, j, bitmap);
                    temp.SetPixel(i,j,outputColor);
                }
            }
            return temp;
        }

        //pixels are ordered in a grid position
        // tl t tr
        // ml m mr
        // bl b br
        private static int kernel(int i, int j, Bitmap bitmap)
        {
            int tl = magnitude(bitmap.GetPixel(i - 1, j + 1));
            int t = magnitude(bitmap.GetPixel(i, j + 1));
            int tr = magnitude(bitmap.GetPixel(i + 1, j + 1));
            int ml = magnitude(bitmap.GetPixel(i - 1, j));
            int mr = magnitude(bitmap.GetPixel(i + 1, j));
            int bl = magnitude(bitmap.GetPixel(i - 1, j - 1));
            int b = magnitude(bitmap.GetPixel(i, j - 1));
            int br = magnitude(bitmap.GetPixel(i + 1, j - 1));
            double Gx = (tr - tl + 2 * (mr - ml) + br - bl);
            double Gy = (tl - bl + 2 * (t - b) + tr - br);
            int G = (int)Math.Sqrt(Gx * Gx + Gy * Gy);
            if (G > 255)
            {
                G = 255;
            }
            return G;
        }
        private static Color Sobel(int i, int j, Bitmap bitmap)
        {
            int G = kernel(i, j, bitmap);
            return Color.FromArgb(G, G, G);
        }
        private static Color CelShaded(int i, int j, Bitmap bitmap)
        {
            int G = kernel(i, j, bitmap);
            Color original = bitmap.GetPixel(i, j);
            int NR = original.R - G, NG = original.G - G, NB = original.B - G;
            if (NR < 0) NR = 0;
            if (NG < 0) NG = 0;
            if (NB < 0) NB = 0;
            return Color.FromArgb(NR, NG, NB);
        }
        private static int magnitude(Color color)
        {
            return (color.R + color.B + color.G) / 3;
        }
    }
}
