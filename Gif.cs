using System.Drawing.Imaging;
using System.Drawing;

namespace GetOutOfMyDesktop
{
    class Gif
    {
        private FrameDimension dimension;
        private int frameCount;
        private int currentFrame;
        private int passFrame;
        private int actualTimeUnits;
        private bool oneLoop;
        private Image gifImage;

        public Gif(Image gif, int TimeUnits, bool once = false)
        {
            gifImage = gif;
            passFrame = TimeUnits;
            oneLoop = once;

            dimension = new FrameDimension(gif.FrameDimensionsList[0]);

            currentFrame = 0;

            frameCount = gifImage.GetFrameCount(dimension);
        }

        public Image GetNextFrame(bool fliped)
        {
            actualTimeUnits++;

            if (actualTimeUnits == passFrame)
            {
                currentFrame++;
                
                actualTimeUnits = 0;
            }

            if (currentFrame >= frameCount)
            {
                if (oneLoop)
                    currentFrame = frameCount - 1;
                else
                    currentFrame = 0;
            }
           
            return (GetFrame(currentFrame, fliped));
        }

        public Image GetFrame(int index, bool flip)
        {           
            gifImage.SelectActiveFrame(dimension, index);
            Image finalImg = (Image)gifImage.Clone();

            if (!flip)
                finalImg.RotateFlip(RotateFlipType.RotateNoneFlipX);

            return (Image)finalImg.Clone();
        }
    }
}
