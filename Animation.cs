using Desktop_Hollow.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetOutOfMyDesktop
{
    public class Animation
    {
        // sound efects
        private SoundPlayer _gonnaThrowSound = new SoundPlayer(Resources.Hollow_Stab_Sound);

        // all images
        private static Image _defaultImg = Resources.Hollow_Walk; // defaut animation
        private static Image _launchImg = Resources.Hollow_Jab; // when throwing the mouse

        Gif gifImage;
        private PictureBox Animate = new PictureBox();

        private bool can_default = true;
        private bool was_facing_right = true;
        
        public Animation(PictureBox Animator)
        {
            Animate = Animator;
            Animate.Show();

            gifImage = new Gif(_defaultImg, 5);
        }

        public void Update()
        {
            if (!Comunication.IsFacingRight && !was_facing_right)
            {
                was_facing_right = true;
            }

            if (Comunication.IsFacingRight && was_facing_right)
            {
                was_facing_right = false;
            }

            if (Comunication.WasLaunched && !can_default)
            {
                gifImage = new Gif(_launchImg, 5, true);
                Animate.Invalidate();
                //Comunication.StopAnimating(2000);
                _gonnaThrowSound.Play();
                can_default = true;
            }
            else
            {
                if (!Comunication.IsInside && !Comunication.WasLaunched && can_default)
                {
                    gifImage = new Gif(_defaultImg, 5);
                    Animate.Invalidate();
                    can_default = false;
                }
            }

            Animate.Image = gifImage.GetNextFrame(was_facing_right);
            Animate.Invalidate();
        }
    }
}

