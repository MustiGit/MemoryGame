using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Harjoitus_2
{
    public partial class memoryGameMain : Form
    {
        bool allowClick = false;
        PictureBox firstGuess;
        Random rnd = new Random();
        int guessAmount;
        string myString;
        Timer clickTimer = new Timer();


        public memoryGameMain()
        {
            InitializeComponent();

            //Hide restart button when game starts.
            restartButton.Hide();
        }

        // Adds all pictureBoxes to array
        private PictureBox[] pictureBoxes
        {
            get { return Controls.OfType<PictureBox>().ToArray(); }
        }

        // Link between images and resources, creating array of images.
        private static IEnumerable<Image> Images
        {
            get
            {
                return new Image[]
                {
                    Properties.Resources.pic_01,
                    Properties.Resources.pic_02,
                    Properties.Resources.pic_03,
                    Properties.Resources.pic_04,
                    Properties.Resources.pic_05,
                    Properties.Resources.pic_06,
                    Properties.Resources.pic_07,
                    Properties.Resources.pic_08
                };
            }
        }

        // ResetImages function resets pictureBoxes.
        private void ResetImages()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Tag = null;
                pic.Visible = true;
            }

            HideImages();
            setRandomImages();

            guessCounter.Visible = true;
            guessAmount = 0;
            myString = guessAmount.ToString();
            guessCounter.Text = myString;

        }

        // HideImages function will turn all cards around so their backside is up. 
        private void HideImages()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Image = Properties.Resources.pic_00;
            }
        }

        // getFreeSlot function will loop through all pictureBoxes randomly and returns value back to program.
        private PictureBox getFreeSlot()
        {
            int num;

            do
            {
                num = rnd.Next(0, pictureBoxes.Count());
            }
            while (pictureBoxes[num].Tag != null);
            return pictureBoxes[num];
        }

        //setRandomImages function loops through images and tries to find matches.
        private void setRandomImages()
        {
            foreach (var image in Images)
            {
                getFreeSlot().Tag = image;
                getFreeSlot().Tag = image;
            }
        }

        private void clickTimer_delay(object sender, EventArgs e)
        {
            HideImages();

            allowClick = true;
            clickTimer.Stop();

        }

        private void setWin()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Tag = null;
                pic.Visible = true;
                pic.Image = Properties.Resources.win;
            }
            allowClick = false;
        }

        // When START button is clicked, allow clicking and run setRandomImages and HideImages functions.
        private void startButton_Click(object sender, EventArgs e)
        {
            allowClick = true;
            setRandomImages();
            HideImages();
            clickTimer.Interval = 1000;
            clickTimer.Tick += clickTimer_delay;

            // Sets GUESSES amount to zero and convert int to string.
            guessCounter.Show();
            guessAmount = 0;
            myString = guessAmount.ToString();
            guessCounter.Text = myString;

            // Hides START button and shows RESTART button.
            startButton.Hide();
            restartButton.Show();

        }
        //When RESTART button is clicked, reset images and allow clicking.
        private void restartButton_Click(object sender, EventArgs e)
        {
            ResetImages();
            firstGuess = null;
            allowClick = true;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void image_click(object sender, EventArgs e)
        {
            // If false, return to program
            if (!allowClick) return;

            // Local variable to register which pictureBoxes were clicked.
            var pic = (PictureBox)sender;

            // If first guess
            if (firstGuess == null)
            {
                firstGuess = pic;
                pic.Image = (Image)pic.Tag;

                return;
            }
            // When images are clicked, set matching tag to picture box.
            pic.Image = (Image)pic.Tag;

            // If pic is same as firstGuess (if players clicks same picture twice in a row)
            if (pic == firstGuess) return;

            // If match found, add +1 to GUESSES counter
            if (pic.Image == firstGuess.Image)
            {

                guessAmount++;
                myString = guessAmount.ToString();
                guessCounter.Text = myString;

                // Hide (Visible = false) matching images for rest of the game.
                pic.Hide();
                firstGuess.Hide();

                HideImages();
            }
            else
            {   // Add delay

                guessAmount++;
                myString = guessAmount.ToString();
                guessCounter.Text = myString;

                allowClick = false;
                clickTimer.Start();
            }
            // Make firstGuess null, preparing the game for next round.
            firstGuess = null;

            // Checks if there's any visible picture boxes left on the screen. Return if true.
            if (pictureBoxes.Any(p => p.Visible)) return;

            //If not, give the user victory screen.
            setWin();

        }

        private void showButton_Click(object sender, EventArgs e)
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Image = (Image)pic.Tag;
                allowClick = false;
            }
        }
    }
}
