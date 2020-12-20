using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GetOutOfMyDesktop
{
    public class Movement
    {
        private Form Comun = new Form();
        private Path path;

        // points
        private Point Destination; // where the form should go
        
        // god so damn ramdom 
        private Random random = new Random(); 

        private int Height = 0;
        private int Width = 0;

        private int Vel = 60;

        private int count = 0;

        public Movement(Form MainForm)
        {
            Comun = MainForm;
            Height = MainForm.Height;
            Width = MainForm.Width;
            
            Destination = GetNewRandomDestination();

            // get a list of steps to follow to get to the destination
            Destination = GetNewRandomDestination();
            path = new Path(Comun.Location, Destination, Height, Width);
            path.Generate(Vel);
        }

        public void Update()
        {
            if (Comunication.IsAfterMouse && Comunication.IsInside)
            {
                Comunication.StopMovement(1000);
            }

            if (path.Steps.Count == count)
            {
                count = 0;

                if (Comunication.IsAfterMouse)
                {
                    GotoMouse();
                }
                else
                {
                    GotoRandom();
                }

                if (Destination.X <= Comun.Location.X)
                {
                    Comunication.IsFacingRight = true;
                }
                else
                {
                    Comunication.IsFacingRight = false;
                }
            }

            // use a counter to go through the step list 
            if (Comun.Location.X != path.Steps[count].X && Comun.Location.Y != path.Steps[count].Y)
            {
                Comun.Location = path.Steps[count];
            }

            count++;
        }

        private void GotoRandom()
        {
            // make a new step list if gone through all steps
            Destination = GetNewRandomDestination();
            path = new Path(Comun.Location, Destination, Height, Width);
            path.Generate(Vel * random.Next(1, 3));
        }

        private void GotoMouse()
        {
            // make a new step list if gone through all steps
            Destination = new Point(Cursor.Position.X - (Comun.Width / 3), Cursor.Position.Y - (Comun.Height / 2)); ;
            path = new Path(Comun.Location, Destination, Height, Width);
            path.Generate(Vel / 2);
        }

        // generates a random point a little bit bigger than the screen 
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
