using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Pong
{
    public partial class RacquetBall : Form
    {
        Rectangle player1 = new Rectangle(10, 100, 10, 60);
        Rectangle player2 = new Rectangle(10, 240, 10, 60);
        Rectangle ball = new Rectangle(295, 195, 10, 10);

        int player1Score = 0;
        int player2Score = 0;

        int playerSpeed = 4;
        int ballXSpeed = -5;
        int ballYSpeed = 0;

        int turn = 0;
        int ballSpeedIncrease = 0;

        Random random = new Random();

        bool aDown = false;
        bool dDown = false;
        bool wDown = false;
        bool sDown = false;
        bool leftArrowDown = false;
        bool rightArrowDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;

        SolidBrush redbrush = new SolidBrush(Color.Tomato);
        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush notP1 = new SolidBrush(Color.DarkBlue);
        SolidBrush notP2 = new SolidBrush(Color.DarkRed);
        public RacquetBall()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
            }

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
            }

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (turn == 1)
            {
                e.Graphics.FillRectangle(blueBrush, player1);
                e.Graphics.FillRectangle(notP2, player2);
            }
            else if (turn == 2)
            {
                e.Graphics.FillRectangle(notP1, player1);
                e.Graphics.FillRectangle(redbrush, player2);
            }
            else if (turn == 0)
            {
                e.Graphics.FillRectangle(blueBrush, player1);
                e.Graphics.FillRectangle(redbrush, player2);
            }
            else
            {
                e.Graphics.FillRectangle(notP1, player1);
                e.Graphics.FillRectangle(notP2, player2);
            }

            e.Graphics.FillRectangle(whiteBrush, ball);
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move ball 
            ball.X += ballXSpeed;
            ball.Y += ballYSpeed;

            //move player 1
            
            if (aDown == true && player1.X > 0)
            {
                player1.X -= playerSpeed;
            }

            if (dDown == true && player1.X < this.Width - player1.Width)
            {
                player1.X += playerSpeed;
            }

            if (wDown == true && player1.Y > 0)
            {
                player1.Y -= playerSpeed;
            }

            if (sDown == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += playerSpeed;
            }

            //move player 2

            if (leftArrowDown == true && player2.X > 0)
            {
                player2.X -= playerSpeed;
            }

            if (rightArrowDown == true && player2.X < this.Width - player1.Width)
            {
                player2.X += playerSpeed;
            }

            if (upArrowDown == true && player2.Y > 0)
            {
                player2.Y -= playerSpeed;
            }

            if (downArrowDown == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += playerSpeed;
            }

            //check if ball hit top, bottom, or right wall and change direction if it does 
            if (ball.Y < 0 || ball.Y > this.Height - ball.Height)
            {
                ballYSpeed *= -1;  // or: ballYSpeed = -ballYSpeed;

            }
            if (ball.X > this.Width - ball.Width)
            {
                if (ballSpeedIncrease == 0)
                {
                    ballSpeedIncrease++;
                }
                else
                {
                    ballXSpeed++;
                    ballSpeedIncrease = 0;
                }

                ballXSpeed *= -1;
                turn /= 10;
                ball.X = this.Width - ball.Width;
            }

            //check if ball hits either player. If it does change the direction 
            //and place the ball in front of the player hit 
            if (player1.IntersectsWith(ball) && (turn == 1 || turn == 0))
            {
                ballXSpeed *= -1;
                ballYSpeed += random.Next(-3, 3);
                ball.X = player1.X + ball.Width;
                turn = 20;
            }
            else if (player2.IntersectsWith(ball) && (turn == 2 || turn == 0))
            {
                ballXSpeed *= -1;
                ballYSpeed += random.Next(-3, 3);
                ball.X = player2.X + ball.Width;
                turn = 10;
            }

            //check if a player missed the ball and if true add 1 to score of other player  
            if (ball.X < 0)
            {
                if (turn == 1)
                {
                    player2Score++;
                }
                else if (turn == 2)
                {
                    player1Score++;
                }

                p1ScoreLabel.Text = $"{player1Score}";
                p2ScoreLabel.Text = $"{player2Score}";

                ball.X = this.Width/2 - ball.Width/2;
                ball.Y = this.Height/2 - ball.Height/2;
                
                if (turn != 0)
                {
                    player1.X = 10;
                    player2.X = 10;
                    player1.Y = 100;
                    player2.Y = 240;
                }

                ballSpeedIncrease = 0;
                ballXSpeed = -5;
                ballYSpeed = 0;
            }

            // check score and stop game if either player is at 3 
            if (player1Score == 3)
            {
                gameTimer.Enabled = false;
                winLabel.Visible = true;
                winLabel.ForeColor = Color.DodgerBlue;
                winLabel.BackColor = Color.Black;
                winLabel.Text = "Player 1 Wins!!";
            }
            else if (player2Score == 3)
            {
                gameTimer.Enabled = false;
                winLabel.Visible = true;
                winLabel.ForeColor = Color.Tomato;
                winLabel.BackColor = Color.Black;
                winLabel.Text = "Player 2 Wins!!";
            }

            Refresh();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            gameTimer.Enabled = false;
            
            player1Score = 0;
            player2Score = 0;
            p1ScoreLabel.Text = "0";
            p2ScoreLabel.Text = "0";
            
            ball.X = 295;
            ball.Y = 195;

            player1.X = 10;
            player2.X = 10;
            player1.Y = 100;
            player2.Y = 240;

            turn = 0;

            ballSpeedIncrease = 0;
            ballXSpeed = -5;
            ballYSpeed = 0;
            winLabel.Visible = false;
            Refresh();
            Thread.Sleep(1000);
            gameTimer.Enabled = true;
        }
    }
}
