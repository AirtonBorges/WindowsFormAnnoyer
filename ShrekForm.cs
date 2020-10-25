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
    public partial class Shrek : Form
    {
        //The multiplier of the wandering of shrek 
        private int _vel = 1;

        //god so damn ramdom 
        Random random = new Random();

        //the variable that stores a point in the screen, where shrek goes to
        private Point _destination = new Point(0, 0);

        //All shrek related images
        private Image _defautPosImg = Properties.Resources.HollowWalk; //defaut animation
        private Image _launchPosImg = Properties.Resources.HollowJab; //when throwing the mouse
        private Image _touchPosImg = Properties.Resources.touchPosition;   //when the mouse touches shrek 
        
        //how many times the Uptate function is called until the cicle resets 
        private int _interval = 1000;
        //how many fixed timestamps throught that function
        private int _timestamps = 10;
        //the amount of time that shrek goes crazy when he touches the mouse 
        private int _crazyTime = 500; //find better nam

        //simple counters for counting stuff
        private int _counter = 0;
        private int _scounter = 0;

        //determined by the sleep func
        private int _sleepTime = -1;

        //method comunication bools 
        private bool _isInsideShrek = false;
        private bool _goAfterMouse = false;
        private bool _wasLaunched = false;
        private bool _isFacingRight = true;

        //sound efects
        private SoundPlayer _dtrSP = new SoundPlayer(Properties.Resources.DTR);
        private SoundPlayer _goomsSP = new SoundPlayer(Properties.Resources.GOOMS);

        //things about controlling the mouse that I dont know exactly how works
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        private static extern bool setCursosPos(int x, int y);

        //basic rules of the universe;
        public Shrek()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TopMost = true; //makes sure that shrek is on top of everything 
            SMoodPicBox.Show();
            ChangeMood(_defautPosImg);
            UpdateTimer.Start(); //starts timer that is the main animator basically
        }

        //where the magic happens 
        //also primary update function
        private void UpdateTimer_Tick_1(object sender, EventArgs e)
        {
            _counter++;
            //Console.WriteLine(_counter);

            Update(_counter);
        }

        //secondary update function
        private void SecondaryUpdateTimer_Tick_1(object sender, EventArgs e)
        {
            //Console.WriteLine(_sCounter);
            _scounter++;
            Console.WriteLine(_scounter);

            //sleep stuff, just happens if sleep func sets sleeptime to a number >= -1
            if (_scounter >= _sleepTime && _sleepTime >= -1)
            {
                SecondaryUpdateTimer.Stop();
                _scounter = 0;
                UpdateTimer.Start();
            }
        }

        //depending on the number of the counter, shrek does something. Also what I call the update function. 
        //to not write a lot of code on the timer. 
        private void Update(int counterValue)
        {
            Console.Write("Counter: " + _counter);
            Console.WriteLine(" Velocity: " + _vel);

            //to change where shrek is going, just change the variable destination 
            GoTo(_destination);

            //timestamps, they need to go from the biggest to the lowest

            //events that happen every update
            //reset
            if (counterValue >= _interval)
            {
                _counter = 0;

                 ChangeMood(_defautPosImg);
            }

            //events that happen every update throught a certain amount of time   
            //if shrek was after the cursor and got it into his hand
            if(_goAfterMouse && counterValue >= _interval - _crazyTime -1)
            {
                if(counterValue == _interval - _crazyTime - 1) { _goomsSP.Play(); }
                //makes shrek go after the mouse if he is not in the mouse position
                if (_isInsideShrek)
                {
                    //shrek goes crazy
                    NewDestination(new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2));
                    setCursosPos(Location.X, Location.Y);

                    if (Location.X == Screen.PrimaryScreen.Bounds.Width / 2 && Location.Y == Screen.PrimaryScreen.Bounds.Height / 2)
                    {
                        setCursosPos(Location.X / 2, Location.Y / 2);
                        _vel = random.Next(8, 14);
                    }
                }
                if(!_isInsideShrek)
                { 
                    NewDestination(MousePosition);
                    _vel = 8;
                }
                else if (counterValue >= _interval - 1 )
                {
                    SecondaryUpdateTimer.Start();
                    _dtrSP.Play();
                    _counter -= _counter;                
                    //throw animation
                    setCursosPos(random.Next(0, 1367), random.Next(0, 769));
                    _isInsideShrek = false;
                    _goAfterMouse = false;
                    _wasLaunched = true; 
                }
            }

            // spin image to look after mouse 
            if (Location.X < _destination.X && !_isFacingRight)
            {
                _isFacingRight = true;
                SMoodPicBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
            if (Location.X > _destination.X && _isFacingRight)
            {
                _isFacingRight = false;
                SMoodPicBox.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }

            if (_wasLaunched == true)
            {
                if (_scounter == 50)
                {
                    _scounter = 0;
                    SecondaryUpdateTimer.Stop();
                    _wasLaunched = false;
                    ChangeMood(_launchPosImg);
                }
            }
            
            //events that happen once per cicle
            if (counterValue == 0)
            {
                ChangeMood(_defautPosImg);
                //Sleep(_interval/3);
            }

            if (counterValue == _interval - _crazyTime - 2)
            {
                _goAfterMouse = random.Next(1, 6) == 5 || random.Next(1, 6) == 4;
                Console.WriteLine(_goAfterMouse);
            }

            //events that happen multiple diferent predetermined timestamps
            //in every multiple of 1/10th of the interval / how many times random events ocour
            if ((float)counterValue % ((float)(_interval/_timestamps)) == 0f)
            {
                int randnum = random.Next(1, 11);
                if (randnum <= 3 && !_goAfterMouse)
                {
                   // Sleep(500);
                }
                else
                {
                    NewDestination(GetNewRandomPoint());
                    _vel = random.Next(1, 7);
                }
            }

            _scounter++;
        }

        //Makes the movement work, basicaly translates shrek semi-smoothly to an point on the screen
        private void GoTo(Point destination)
        {
            //stores if shrek needs to go positively or negatively on the x and/or y axis, to get to his destiny  
            int valX = 0;
            int valY = 0;

            //if destination is on the left of shrek, negative,
            //if right, then positive (also makes the midle of shrek be on the middle)
            if (destination.X > Location.X + Width/3)
            {
                valX = 1;
            }
            if (destination.X < Location.X + Width/3)
            {
                valX = -1;
            }

            //same thing but up and down 
            if (destination.Y > Location.Y + Height - Height/3)
            {
                valY = 1;
            }
            if (destination.Y < Location.Y + Height - Height/3)
            {
                valY = -1;
            }

            //teleports shrek into his new position, and multiplies it by the _vel variable, that can make him look a litle 
            //enraged
            Location = new Point(Location.X + (valX * _vel), Location.Y + (valY * _vel));
        }

        //point stuff

        //update destination more easily 
        private void NewDestination(Point point)
        {
            _destination = point;
        }

        //generates a random point a little bit bigger than the screen 
        private Point GetNewRandomPoint()
        {
            random = new Random();

            int x = random.Next(-100, 1466);
            int y = random.Next(-100, 868);

            return new Point(x, y);
        }

        //

        //changes shrek animation//picture
        private void ChangeMood(Image mood)
        {
            SMoodPicBox.Image = mood;
        }

        //sleep function that pauses the main update function, using a timer
        private void Sleep(int sleepTime)
        {
            _sleepTime = sleepTime;
            SecondaryUpdateTimer.Start();
            UpdateTimer.Stop();
        }

        //things about windows forms events
        private void SMoodPicBox_MouseEnter(object sender, EventArgs e)
        {
            _isInsideShrek = true;
        }

    }
}
