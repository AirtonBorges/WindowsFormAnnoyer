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
    public class Animation
    {
        // sound efects
        private SoundPlayer _gonnaSeekSound = new SoundPlayer(Properties.Resources.DTR);
        private SoundPlayer _gonnaThrowSound = new SoundPlayer(Properties.Resources.GOOMS);

        // all images
        private static Image _defaultImg = Properties.Resources.DefautPosition; // defaut animation
        private static Image _launchImg = Properties.Resources.launchPosition; // when throwing the mouse
        private static Image _touchImg = Properties.Resources.touchPosition; // when the mouse is being touched

        private PictureBox Animate = new PictureBox();

        private bool was_facing_right = true;

        public Animation(PictureBox Animator)
        {
            Animate = Animator;
            Animator.Show();
            Animator.Image = _defaultImg;
        }

        public void Update()
        {
            if (!Comunication.IsFacingRight && !was_facing_right)
            {
                was_facing_right = true;
                Animate.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }

            if (Comunication.IsFacingRight && was_facing_right)
            {
                was_facing_right = false;
                Animate.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }

            if (Comunication.WasLaunched)
            {
                Animate.Image = _launchImg;
                _gonnaThrowSound.Play();
                Comunication.StopAnimating(2000);
            }
            else
            {
                if (!Comunication.IsInside)
                {
                    Animate.Image = _defaultImg;
                }
                else
                {
                    Animate.Image = _touchImg;
                }
            }
        }
    }
}

