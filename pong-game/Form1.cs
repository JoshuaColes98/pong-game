using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pong_game
{
    public partial class Pong : Form
    {
        int ballXspeed = 4;
        int ballYspeed = 4;
        int speed = 2;
        Random random = new Random();
        bool goDown, goUp;
        int computer_speed_change = 50;
        int playerScore = 0;
        int computerScore = 0;
        int playerSpeed = 8;
        int[] i = { 5, 6, 8, 9 };
        int[] j = { 10, 9, 8, 11, 12 };
        bool isPaused = false; // New variable to track pause state

        public Pong()
        {
            InitializeComponent();
            MakeBallPictureBoxRounded();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            if (!isPaused) // Check if the game is not paused
            {
                ball.Top -= ballYspeed;
                ball.Left -= ballXspeed;

                playerScoreLabel.Text = "Player: " + playerScore;
                computerScoreLabel.Text = "Computer: " + computerScore;

                if (ball.Top < 0 || ball.Bottom > this.ClientSize.Height)
                {
                    ballYspeed = -ballYspeed;
                }
                if (ball.Left < -2)
                {
                    ball.Left = 300;
                    ballXspeed = -ballXspeed;
                    computerScore++;
                }

                if (ball.Right > this.ClientSize.Width + 2)
                {
                    ball.Left = 300;
                    ballXspeed = -ballXspeed;
                    playerScore++;
                }

                if (computer.Top <= 1)
                {
                    computer.Top = 0;
                }
                else if (computer.Bottom >= this.ClientSize.Height)
                {
                    computer.Top = this.ClientSize.Height - computer.Height;
                }

                if (ball.Top < computer.Top + (computer.Height / 2) && ball.Left > 300)
                {
                    computer.Top -= speed;
                }

                if (ball.Top > computer.Top + (computer.Height / 2) && ball.Left > 300)
                {
                    computer.Top += speed;
                }

                computer_speed_change -= 1;

                if (computer_speed_change < 0)
                {
                    speed = i[random.Next(i.Length)];
                    computer_speed_change = 50;
                }

                if (goDown && player.Top + player.Height < this.ClientSize.Height)
                {
                    player.Top += playerSpeed;
                }

                if (goUp && player.Top > 0)
                {
                    player.Top -= playerSpeed;
                }

                CheckCollision(ball, player, player.Right + 5);
                CheckCollision(ball, computer, computer.Left - 35);

                if (computerScore > 5)
                {
                    GameOver("Unlucky, try again?");
                }
                else if (playerScore > 5)
                {
                    GameOver("Congratulations, you won! Want to play again?");
                }
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                goDown = true;
            }
            else if (e.KeyCode == Keys.Space) // Handle spacebar for pause
            {
                isPaused = !isPaused; // Toggle pause state
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            else if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        private void CheckCollision(PictureBox PicOne, PictureBox PicTwo, int offset)
        {
            if (PicOne.Bounds.IntersectsWith(PicTwo.Bounds))
            {
                PicOne.Left = offset;

                int x = j[random.Next(j.Length)];
                int y = j[random.Next(j.Length)];

                if (ballXspeed < 0)
                {
                    ballXspeed = x;
                }
                else
                {
                    ballXspeed = -x;
                }

                if (ballYspeed < 0)
                {
                    ballYspeed = -y;
                }
                else
                {
                    ballYspeed = y;
                }
            }
        }

        private void MakeBallPictureBoxRounded()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(ball.ClientRectangle);
            ball.Region = new Region(path);
        }

        private void GameOver(string message)
        {
            GameTimer.Stop();
            DialogResult result = MessageBox.Show(message, "Game Over! ", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                RestartGame();
            }
            else if (result == DialogResult.No)
            {
                Application.Exit();
            }
        }

        private void RestartGame()
        {
            computerScore = 0;
            playerScore = 0;
            ballXspeed = ballYspeed = 4;
            computer_speed_change = 50;
            GameTimer.Start();
        }
    }
}
