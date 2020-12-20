using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetOutOfMyDesktop
{
    public partial class Comunication : Form
    {
        // Instances of the classes that control the form 
        private Movement move; // Takes care of movement
        private Animation anim; // Takes care of animation 
        private Interaction interaction; // Takes care of interacting with the 

        // Look how funny this is 
        private Random random;

        // comunication bools 
        public static bool IsMoving = false;
        public static bool IsInside = false;
        public static bool IsAfterMouse = false;
        public static bool WasLaunched = false;
        public static bool IsFacingRight = false;
        public static bool IsAnimating = false;


        private Timer AnimationTimer;
        private IContainer components;
        private Timer InteractionTimer;
        private Timer MovementTimer;
        private Timer RandomEventTimer;
        private Timer SleepTimer;
        private PictureBox Animator;
        private static int _sleepTime = 1;

        //basic rules of the universe;
        public Comunication()
        {
            InitializeComponent();
        }

        private void Comunication_Load(object sender, EventArgs e)
        {
            TopMost = true; //makes sure that It's on top of everything

            // Starting timers that trigger all the update functions
            MovementTimer.Start();
            AnimationTimer.Start();
            RandomEventTimer.Start(); 
            InteractionTimer.Start();

            // look, the funny one
            random = new Random();
        }

        private void Comunication_Shown(object sender, EventArgs e)
        {
            // Innitialize objects ( after the form has shown ) ( The height still bugs )
            move = new Movement(ActiveForm);
            anim = new Animation(Animator);
            Point borderPoint = getBorderPoint();
            interaction = new Interaction(borderPoint.X, borderPoint.Y);
        }
        
        // Update functions 

        // For now just to determine if the forms goes after the mouse or if it just wonders around
        private void RandomEventTimer_Tick(object sender, EventArgs e)
        {
            BringToFront();
            TopMost = true;

            if (random.Next(1, 10) < 9)
            {
                IsAfterMouse = true;
            }
            else IsAfterMouse = false;
        }

        // Down Here are just update functions that that control their respective class
        private void MovementUpdate(object sender, EventArgs e)
        {
            if(IsMoving)
            {
                move.Update();
            }
            else
            {
                SleepTimer.Interval = _sleepTime;
                MovementTimer.Stop();

                SleepTimer.Start();
            }
        }

        private void AnimationUpdate(object sender, EventArgs e)
        {
            if (IsAnimating)
            {
                anim.Update();
            }
            else
            {
                SleepTimer.Interval = _sleepTime;
                AnimationTimer.Stop();

                SleepTimer.Start();
            }
        }

        private void InteractionUpdate(object sender, EventArgs e)
        {
            if (WasLaunched)
            {
                Point borderPoint = getBorderPoint();
                interaction.Launch_To(borderPoint.X, borderPoint.Y);

                SleepTimer.Interval = _sleepTime;
                InteractionTimer.Stop();
                
                SleepTimer.Start();
            }
            else
            {
                interaction.Update();
            }
        }

        // Used to count how much time a function stays sleeping
        private void SleepTick(object sender, EventArgs e)
        {
            if (!IsMoving)
            {
                SleepTimer.Interval = 1;
                MovementTimer.Start();

                IsMoving = true;
                SleepTimer.Stop();
            }

            if (WasLaunched)
            {
                SleepTimer.Interval = 1;
                InteractionTimer.Start();

                WasLaunched = false;
                SleepTimer.Stop();
            }

            if (!IsAnimating)
            {
                SleepTimer.Interval = 1;
                AnimationTimer.Start();

                IsAnimating = true;
                SleepTimer.Stop();
            }
        }
        
        // Static functions to make possible to the classes to stop their update function 
        public static void StopMovement(int miliseconds)
        {
            IsMoving = false;
            _sleepTime = miliseconds;
        }

        public static void stopDetection(int miliseconds)
        {
            WasLaunched = true;
            _sleepTime = miliseconds;
        }

        public static void StopAnimating(int miliseconds)
        {
            _sleepTime = miliseconds;
        }

        // Returns a random point on the left or right border of the screen
        private Point getBorderPoint()
        {
            Point screen = new Point(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Point border = new Point(random.Next(0, screen.X), 0);

            // If is facing right the point should be on the right 

            // Console.WriteLine($"{Cursor.Position.X}, {screen.Y - (screen.Y / 5)}");

            // If too close to the left or the right throw in the oposite direction 
            if (Cursor.Position.X <= screen.Y / 5)
            {
                Console.WriteLine("Direita");
                border.Y = screen.X - 10;
                IsFacingRight = true;
            }
            else if (Cursor.Position.X >= screen.Y - (screen.Y / 5))
            {
                Console.WriteLine("Esquierda");
                border.Y = 10;
                IsFacingRight = false;
            }
            else
            {
                if (IsFacingRight)
                {
                    border.Y = screen.X - 10;
                }
            }

            return border;
        }

        // form related events
        private void Mouse_Leave(object sender, EventArgs e)
        {
            IsInside = false;
        }

        private void Mouse_Enter(object sender, EventArgs e)
        {
            // if mouse touches the form, then it's not inside
            IsInside = true;
        }

        private void Comunication_MouseLeave(object sender, EventArgs e)
        {

        }

        private void Animator_Click(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AnimationTimer = new System.Windows.Forms.Timer(this.components);
            this.InteractionTimer = new System.Windows.Forms.Timer(this.components);
            this.MovementTimer = new System.Windows.Forms.Timer(this.components);
            this.RandomEventTimer = new System.Windows.Forms.Timer(this.components);
            this.SleepTimer = new System.Windows.Forms.Timer(this.components);
            this.Animator = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Animator)).BeginInit();
            this.SuspendLayout();
            // 
            // AnimationTimer
            // 
            this.AnimationTimer.Interval = 1;
            this.AnimationTimer.Tick += new System.EventHandler(this.AnimationUpdate);
            // 
            // InteractionTimer
            // 
            this.InteractionTimer.Interval = 1;
            this.InteractionTimer.Tick += new System.EventHandler(this.InteractionUpdate);
            // 
            // MovementTimer
            // 
            this.MovementTimer.Interval = 1;
            this.MovementTimer.Tick += new System.EventHandler(this.MovementUpdate);
            // 
            // RandomEventTimer
            // 
            this.RandomEventTimer.Interval = 1500;
            this.RandomEventTimer.Tick += new System.EventHandler(this.RandomEventTimer_Tick);
            // 
            // SleepTimer
            // 
            this.SleepTimer.Interval = 1;
            this.SleepTimer.Tick += new System.EventHandler(this.SleepTick);
            // 
            // Animator
            // 
            this.Animator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Animator.Location = new System.Drawing.Point(0, 0);
            this.Animator.Name = "Animator";
            this.Animator.Size = new System.Drawing.Size(76, 94);
            this.Animator.TabIndex = 0;
            this.Animator.TabStop = false;
            this.Animator.MouseEnter += new System.EventHandler(this.Mouse_Enter);
            this.Animator.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            // 
            // Comunication
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(76, 106);
            this.ControlBox = false;
            this.Controls.Add(this.Animator);
            this.Cursor = System.Windows.Forms.Cursors.Help;
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Comunication";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.Load += new System.EventHandler(this.Comunication_Load);
            this.Shown += new System.EventHandler(this.Comunication_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.Animator)).EndInit();
            this.ResumeLayout(false);

        }


    }
}
