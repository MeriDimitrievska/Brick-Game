using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrickGame
{
    public partial class Form1 : Form
    {

        bool goLeft;
        bool goRight;
        bool isGameOver;

        int score;
        int ballx;
        int bally;
        int playerSpeed;

        Random rnd = new Random();

        PictureBox[] blocksArray;

        public Form1()
        {
            InitializeComponent();
            //setupGame();
            placeBlocks();
        }

        private void setupGame()
        {
            isGameOver=false;
            score = 0;
            ballx = 5;
            bally = 5;
            playerSpeed = 12;
            txtScore.Text = "Score: " + score;


            ball.Left=428;
            ball.Top = 454;
            player.Left = 389;
            player.Top = 475;


            gameTimer.Start();

            foreach(Control x in this.Controls)
            {
                if(x is PictureBox && (string)x.Tag == "blocks")
                {
                    x.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                }
            }

        }

        private void gameOver(String message)
        {
            isGameOver = true;
            gameTimer.Stop();

            txtScore.Text = "Score: " + score + " " + message;
        }

        private void placeBlocks()
        {
            blocksArray = new PictureBox[36];  
            int a = 0;
            int top = 50;
            int left = 100;
        
            for(int i=0; i<blocksArray.Length; i++)
            {
                blocksArray[i] = new PictureBox();
                blocksArray[i].Height = 32;
                blocksArray[i].Width = 100;
                blocksArray[i].Tag = "blocks";
                blocksArray[i].BackColor = Color.White;

                if(a==6)
                {
                    top = top + 50;
                    left = 100;
                    a = 0;
                }

                if(a<6)
                {
                    a++;
                    blocksArray[i].Left = left;
                    blocksArray[i].Top = top;
                    this.Controls.Add(blocksArray[i]);
                    left += 130;
                }
            }

            setupGame();
        }


        private void removeBlocks()
        {
            foreach(PictureBox p in blocksArray)
            {
                this.Controls.Remove(p);
            }
        }

        private void mainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score; 
            if(goLeft == true && player.Left > 0)
            {
                player.Left -= playerSpeed;
            }

            if(goRight == true && player.Left < 832)
            {
                player.Left += playerSpeed;
            }

            ball.Left += ballx;
            ball.Top += bally;

            if(ball.Left < 0 || ball.Left > 920) 
            {
                ballx = -ballx;
            }

            if(ball.Top < 0)
            {
                bally = -bally;
            }

            if(ball.Bounds.IntersectsWith(player.Bounds))
            {
                //ball.Top = 698;

                bally *= -1; // bally = rnd.Next(5, 12) * -1;

                if (ballx<0)
                {
                    ballx = rnd.Next(5, 12) * -1;
                }
                else
                {
                    ballx = rnd.Next(5, 12);
                }
            }

            foreach(Control control in this.Controls)
            {
                if(control is PictureBox && (string)control.Tag=="blocks")
                {
                    if(ball.Bounds.IntersectsWith(control.Bounds))
                    {
                        score += 1;
                        bally = - bally;
                        this.Controls.Remove(control);
                    }
                }
            }

            if(score == 36)
            {
                gameOver("You win! Press Enter to Play Again.");
            }

            if(ball.Top >580)
            {
                gameOver("You lose! Press Enter to try again.");
            }

        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }

            if(e.KeyCode == Keys.Right)
            {
                goRight = true;
            }



        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }

            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }

            if(e.KeyCode==Keys.Enter && isGameOver == true)
            {
                removeBlocks();
                placeBlocks();
            }

        }

    }
}
