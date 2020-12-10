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
        private Movement move;
        private Animation anim;
        private Random random;
        private Interaction interaction;

        // comunication bools 
        public static bool IsMoving = false;
        public static bool IsInside = false;
        public static bool IsAfterMouse = false;
        public static bool WasLaunched = false;
        public static bool IsFacingRight = true;
        public static bool IsAnimating = false;
        private PictureBox Animator;
        private Timer AnimationTimer;
        private IContainer components;
        private Timer InteractionTimer;
        private Timer MovementTimer;
        private Timer RandomEventTimer;
        private Timer SleepTimer;
        private static int _sleepTime = 1;

        //basic rules of the universe;
        public Comunication()
        {
            InitializeComponent();
        }

        private void Comunication_Load(object sender, EventArgs e)
        {
            TopMost = true; //makes sure that It's on top of everything

            MovementTimer.Start(); //starts the update timer
            AnimationTimer.Start();
            RandomEventTimer.Start();  //starts the random event timer
            InteractionTimer.Start();

            //anim = new Animation(Animator);
        }

        private void Comunication_Shown(object sender, EventArgs e)
        {
            //load controllers
            move = new Movement(ActiveForm);
            anim = new Animation(Animator);
            interaction = new Interaction();

            random = new Random();
        }
        
        // update functions 
        private void RandomEventTimer_Tick(object sender, EventArgs e)
        {
            if (random.Next(1, 10) != 2)
            {
                IsAfterMouse = true;
            }
            else IsAfterMouse = false;
        }

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
                SleepTimer.Interval = _sleepTime;
                InteractionTimer.Stop();

                SleepTimer.Start();
            }
            else
            {
                interaction.Update();
            }
        }

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
            IsAnimating = false;
            _sleepTime = miliseconds;
        }

        // form related events
        private void Animator_MouseLeave(object sender, EventArgs e)
        {
            // if the cursor leaves, then it's not inside
            IsInside = false;
        }

        private void SMoodPicBox_MouseEnter(object sender, EventArgs e)
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
            this.Animator = new System.Windows.Forms.PictureBox();
            this.AnimationTimer = new System.Windows.Forms.Timer(this.components);
            this.InteractionTimer = new System.Windows.Forms.Timer(this.components);
            this.MovementTimer = new System.Windows.Forms.Timer(this.components);
            this.RandomEventTimer = new System.Windows.Forms.Timer(this.components);
            this.SleepTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Animator)).BeginInit();
            this.SuspendLayout();
            // 
            // Animator
            // 
            this.Animator.Location = new System.Drawing.Point(0, 0);
            this.Animator.Name = "Animator";
            this.Animator.Size = new System.Drawing.Size(75, 96);
            this.Animator.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Animator.TabIndex = 0;
            this.Animator.TabStop = false;
            this.Animator.WaitOnLoad = true;
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
            // Comunication
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
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
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.Load += new System.EventHandler(this.Comunication_Load);
            this.Shown += new System.EventHandler(this.Comunication_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.Animator)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
