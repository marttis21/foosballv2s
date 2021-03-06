﻿using Android.Graphics;
using Emgu.CV.Structure;

namespace foosballv2s.Droid.Shared.Source.Services.GameRecognition
{
    /// <summary>
    /// A class for working with image files and getting a ball color from it
    /// </summary>
    public class BallImage
    {
        private Bitmap image;
        
        public BallImage(Bitmap bitmapImage)
        {
            this.image = bitmapImage;
        }

        /// <summary>
        /// Gets a ball color from a picture
        /// </summary>
        /// <returns></returns>
        public Hsv getColor()
        {
            Bitmap bitmap = this.image;
           
            int a = 0, r = 0, g = 0, b = 0;
            int pixelCount = 0;
            for (int i = (bitmap.Width / 2) - 20; i < (bitmap.Width / 2) + 20; i += 4)
            {
                for (int j = (bitmap.Height / 2) - 20; j < (bitmap.Height / 2) + 20; j += 4)
                {
                    int pixel = bitmap.GetPixel(i, j);
                    a += Color.GetAlphaComponent(pixel);
                    r += Color.GetRedComponent(pixel);
                    g += Color.GetGreenComponent(pixel);
                    b += Color.GetBlueComponent(pixel);
                    pixelCount++;
                }
            }

            // calculate averages
            a /= pixelCount;
            r /= pixelCount;
            g /= pixelCount;
            b /= pixelCount;

            Color colorAvg = Color.Argb(a, r, g, b);

            Hsv hsv = new Hsv(colorAvg.GetHue() / 2,    //color.GetHue() [0;360] -> Hue [0;180]
                colorAvg.GetSaturation() * 255,         //color.GetSaturation() [0;1] -> Saturation [0;255]
                colorAvg.GetBrightness() * 255);        //color.GetBrightness() [0;1] -> Brightness [0;255]
            return hsv;
        }
    }
}
