﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;

namespace GetOutOfMyDesktop
{
    class Interaction
    {
        //things about controlling the mouse that I dont know exactly how works
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        private static extern bool setCursosPos(int x, int y);

        Random random = new Random();

        public void Iteraction() {}

        public void Update()
        {
            if (!Comunication.WasLaunched && Comunication.IsAfterMouse && Comunication.IsInside)
            {
                // get a new position to throw the mouse at, and then trow it there
                Point NewRandomMousePos = new Point(random.Next(0, 1366), random.Next(0, 768));
                setCursosPos(NewRandomMousePos.X, NewRandomMousePos.Y);

                // give the mouse some time to recover
                Comunication.stopDetection(1500);
            }
        }
    }
}
