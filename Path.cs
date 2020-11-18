using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetOutOfMyDesktop
{
    // A class that stores and generates a list of movements from a point to another
    public class Path
    {
        private Point _from;
        private Point _to;

        public List<Point> Steps;

        private int _step_size = 1;
        private int _width = 0;
        private int _height = 0;

        public Path(Point from, Point to, int height, int width)
        {
            _from = from;
            _to = to;

            _height = height;
            _width = width;
            
            Steps = new List<Point>();
        }
        
        public void Generate(int step_size)
        {
            //bresenham algorithm to get all points betwen the from and to point
            //props to Ryan Steffer on stack overflow
            _step_size = step_size;
            int ydiff = _to.Y - _from.Y, xdiff = _to.X - _from.X;
            double slope = (double)(_to.Y - _from.Y) / (_to.X - _from.X);
            double x, y;

            --_step_size;

            for (double i = 0; i < _step_size; i++)
            {
                y = slope == 0 ? 0 : ydiff * (i / _step_size);
                x = slope == 0 ? xdiff * (i / _step_size) : y / slope;


                Steps.Add(new Point((int)Math.Round(x) + _from.X, (int)Math.Round(y) + _from.Y));
            }

            Steps.Add(_to);
        }
    }
}
