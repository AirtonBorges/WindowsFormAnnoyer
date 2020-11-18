using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetOutOfMyDesktop
{
    public class Movement
    {
        private Form Comun = new Form();
        private Path path;

        //points
        private Point Destination; //where the form should go
        private Point Mouse;
        //god so damn ramdom 
        private Random random = new Random(); 

        private int Height = 0;
        private int Width = 0;

        private int Vel = 10;

        private int count = 0;

        public Movement(Form MainForm)
        {
            Comun = MainForm;
            Height = MainForm.Height;
            Width = MainForm.Width;
            
            Destination = GetNewRandomDestination();

            // get a list of steps to follow to get to the destination
            Mouse = new Point(Cursor.Position.X * -(Comun.Width / 2), Cursor.Position.Y * -(Comun.Height / 2));
            path = new Path(Comun.Location, Mouse, Height, Width);
            path.Generate(Vel);
        }

        public void Update()
        {
            if ( path.Steps.Count == count)
            {
                // make a new step list if gone through all steps
                Destination = Cursor.Position;
                Mouse = new Point(Cursor.Position.X - (Comun.Width / 2), Cursor.Position.Y - (Comun.Height / 2));
                path = new Path(Comun.Location, Mouse, Height, Width);
                path.Generate(Vel / 2);
                count = 0;
            }

            // use a counter to go through the step list
            Console.WriteLine(Comun.Location);
            Comun.Location = path.Steps[count];
            count++;
        }
        
        //generates a random point a little bit bigger than the screen 
        private Point GetNewRandomDestination()
        {
            random = new Random();
            Point randomP;

            int x = random.Next(0, Screen.PrimaryScreen.Bounds.Width);
            int y = random.Next(0, Screen.PrimaryScreen.Bounds.Height);

            randomP = new Point(x, y);
            
            return randomP;
        }
    }
}
