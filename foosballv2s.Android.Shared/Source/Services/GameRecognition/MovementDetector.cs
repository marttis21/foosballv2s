﻿using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace foosballv2s.Droid.Shared.Source.Services.GameRecognition
{
    /// <summary>
    /// Foosball game detection and recognition class
    /// </summary>
    public class MovementDetector
    {
        // Steps:
        // 1) A frame is retrieved from a video
        // 2) The frame is converted from RGB to HSV color scheme (for more accurate detection)
        // 3) All unneeded colors are filtered out, only pixels with given color are left
        // 4) After this step the picture is in grayscale where the ball appears white and all other
        //    pixels are black
        // 5) Circles are detected in the grayscale image
        // 6) The proccessed frame with circles drawn on it is shown in a window
        private Mat frame;
        private Image<Hsv, Byte> hsvFrame;
        private Image<Gray, Byte> thresholded;
        private Hsv minHsv;
        private Hsv maxHsv;
        
        public const int NONE = 0;
        public const int LEFT_SIDE = 1;
        public const int RIGHT_SIDE = 2;
        
        public bool NewGoalDetected { get; set; }
        public CircleF LastBallDetected { get; private set; }
        public DateTime LastTimeBallDetected { get; set; } = DateTime.MinValue;
        public int GoalSide { get; private set; }

        public IVideo Video { get; set; }
        
        public MovementDetector() {}
        
        public MovementDetector(IVideo video) 
        {
            this.Video = video;
        }
        
        /// <summary>
        /// Sets up the detector with frame and ball color values
        /// </summary>
        /// <param name="frameHeight"></param>
        /// <param name="frameWidth"></param>
        /// <param name="hsvBall"></param>
        public void SetupBallDetector(int frameHeight, int frameWidth, Hsv hsvBall)
        {
            //double t1min = 170, t1max = 4, t2min = 120, t2max = 255, t3min = 120, t3max = 255; // color in HSV
            double t1min, t1max, t2min, t2max, t3min, t3max;

            t1min = hsvBall.Hue - 8;
            t1max = hsvBall.Hue + 8;
            t2min = hsvBall.Satuation - 70;
            t2max = hsvBall.Satuation + 100;
            t3min = hsvBall.Value - 70;
            t3max = hsvBall.Value + 100;

            if (t1min < 0)
                t1min = t1min + 180;
            if (t1max > 179)
                t1max = t1max - 180;
            if (t2min < 0)
                t2min = 0;
            if (t2max > 255)
                t2max = 255;
            if (t3min < 0)
                t3min = 0;
            if (t3max > 255)
                t3max = 255;
            
            minHsv = new Hsv(t1min, t2min, t3min); // minimum color that passes
            maxHsv = new Hsv(t1max, t2max, t3max); // maximum color that passes

            hsvFrame = new Image<Hsv, byte>(new Image<Gray, Byte>[] {
                new Image<Gray, Byte>(frameWidth, frameHeight),
                new Image<Gray, Byte>(frameWidth, frameHeight),
                new Image<Gray, Byte>(frameWidth, frameHeight),
                
            }); // frame converted to HSV
            thresholded = new Image<Gray, byte>(frameWidth, frameHeight); // frame with filtered out colors
        }
        
        /// <summary>
        /// Detects a ball in the given frame
        /// </summary>
        /// <param name="inputFrame"></param>
        /// <param name="frameHeight"></param>
        /// <param name="frameWidth"></param>
        /// <returns></returns>
        public CircleF[] DetectBall(Image<Hsv, System.Byte> inputFrame, int frameHeight, int frameWidth,
            int bitmapScaleDown)
        { 
            if (inputFrame == null)
            {
                Console.WriteLine("Error. A frame is empty. Skipping");
                return null;
            }
            hsvFrame = inputFrame;
            // Filter out other colors than specified
            this.FilterHsvImageColor(hsvFrame, minHsv, maxHsv);
            // Make some smoothing for better detection results
            thresholded = thresholded.SmoothGaussian(5);
            // Find circles in grayscale image and draw them on the frame
            CircleF[] circles = this.DetectCirclesInImage(thresholded, frame, bitmapScaleDown);
            if (circles.Length > 0)
            {
                LastBallDetected = circles[0];
                LastTimeBallDetected = DateTime.Now;
            }
            CheckGoal();
            return circles;
        }

        /// <summary>
        /// Filters out colors which are out of the range in the image
        /// </summary>
        /// <param name="hsvImage"></param>
        /// <param name="minHsv"></param>
        /// <param name="maxHsv"></param>
        private void FilterHsvImageColor(Image<Hsv, byte> hsvImage, Hsv minHsv, Hsv maxHsv)
        {
            // Dealing with colors that are close to 0 or 180 by their hue (especially red)
            if (minHsv.Hue > maxHsv.Hue)
            {
                Hsv partialMax = new Hsv(180, maxHsv.Satuation, maxHsv.Value);
                Hsv partialMin = new Hsv(0, minHsv.Satuation, minHsv.Value);
                Image<Gray, Byte> thresholdedFirst = hsvImage.InRange(minHsv, partialMax);
                Image<Gray, Byte> thresholdedSecond = hsvImage.InRange(partialMin, maxHsv);
                CvInvoke.AddWeighted(thresholdedFirst, 1.0, thresholdedSecond, 1.0, 0.0, thresholded);
                thresholdedFirst.Dispose();
                thresholdedSecond.Dispose();

            }
            else // Otherwise just filter out colors that are out of the given range
            {
                thresholded = hsvImage.InRange(minHsv, maxHsv);
            }
        }
        
        /// <summary>
        /// Using Hough transform to detect circles in a black-white image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="outputFrame"></param>
        /// <returns></returns>
        private CircleF[] DetectCirclesInImage(Image<Gray, byte> image, Mat outputFrame, int bitmapScaleDown)
        {
            //CircleF[] circles = CvInvoke.HoughCircles(image, HoughType.Gradient, 1, 
            //    1000, 10, 10, 15, 60);
            return CvInvoke.HoughCircles(image, HoughType.Gradient, 2, image.Height / 4, 50, 40, 15 / bitmapScaleDown, 50 / bitmapScaleDown);
        }

        private void CheckGoal()
        {
            if (NewGoalDetected 
                || LastTimeBallDetected == DateTime.MinValue 
                || DateTime.Now - LastTimeBallDetected < TimeSpan.FromSeconds(3))
            {
                NewGoalDetected = false;
                return;
            }
            if (LastBallDetected.Center.X < hsvFrame.Width / 2)
            {
                GoalSide = LEFT_SIDE;
            }
            else if(LastBallDetected.Center.X > hsvFrame.Width / 2)
            {
                GoalSide = RIGHT_SIDE;
            }
            
            NewGoalDetected = true;
        }

        
    }
}