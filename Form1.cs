using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        Control[] activePiece = { null, null, null, null };
        Control[] activePiece2 = { null, null, null, null };
        int timeElapsed = 0;

        public Form1()
        {
            InitializeComponent();
            dropNewPiece();
            timer1.Start();
        }

        public void dropNewPiece()
        {
            activePiece[0] = pictureBox5;
            activePiece[1] = pictureBox6;
            activePiece[2] = pictureBox16;
            activePiece[3] = pictureBox26;

            foreach (Control square in activePiece)
            {
                square.BackColor = Color.Red;
            }
        }
            
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            int currentHighRow = 9;
            int currentLowRow = 0;
            int currentLeftCol = 9;
            int currentRightCol = 0;
            foreach (Control square in activePiece)
            {
                if (grid.GetRow(square) < currentHighRow)
                {
                    currentHighRow = grid.GetRow(square);
                }
                if (grid.GetRow(square) > currentLowRow)
                {
                    currentLowRow = grid.GetRow(square);
                }
                if (grid.GetColumn(square) < currentLeftCol)
                {
                    currentLeftCol = grid.GetColumn(square);
                }
                if (grid.GetColumn(square) > currentRightCol)
                {
                    currentRightCol = grid.GetColumn(square);
                }
            }
            
            if (e.KeyCode == Keys.Left)
            {
                bool canMove = true;
                foreach (Control square in activePiece)
                {
                    int squareRow = grid.GetRow(square);
                    int squareCol = grid.GetColumn(square);
                    if (currentLeftCol > 0)
                    {
                        Control leftSquare = grid.GetControlFromPosition(squareCol - 1, squareRow);
                        if (leftSquare.BackColor == Color.Red & activePiece.Contains(leftSquare) == false & currentLeftCol > 0)
                        {
                            canMove = false;
                            break;
                        }
                    }
                    else
                    {
                        canMove = false;
                        break;

                    }
                }

                if (canMove == true)
                {
                    //Move piece left
                    int x = 0;
                    foreach (PictureBox square in activePiece)
                    {
                        square.BackColor = Color.White;
                        int squareRow = grid.GetRow(square);
                        int squareCol = grid.GetColumn(square);
                        activePiece2[x] = grid.GetControlFromPosition(squareCol - 1, squareRow);
                        x++;
                    }
                    x = 0;
                    foreach (PictureBox square in activePiece2)
                    {
                        square.BackColor = Color.Red;
                        activePiece[x] = square;
                        x++;
                    }
                }
            }

            if (e.KeyCode == Keys.Right)
            {
                bool canMove = true;
                foreach (Control square in activePiece)
                {
                    int squareRow = grid.GetRow(square);
                    int squareCol = grid.GetColumn(square);
                    if (currentRightCol < 9)
                    {
                        Control rightSquare = grid.GetControlFromPosition(squareCol + 1, squareRow);
                        if (rightSquare.BackColor == Color.Red & activePiece.Contains(rightSquare) == false & currentRightCol > 0)
                        {
                            canMove = false;
                            break;
                        }
                    }
                    else
                    {
                        canMove = false;
                        break;

                    }
                }

                if (canMove == true)
                {
                    //Move piece right
                    int x = 0;
                    foreach (PictureBox square in activePiece)
                    {
                        square.BackColor = Color.White;
                        int squareRow = grid.GetRow(square);
                        int squareCol = grid.GetColumn(square);
                        activePiece2[x] = grid.GetControlFromPosition(squareCol + 1, squareRow);
                        x++;
                    }
                    x = 0;
                    foreach (PictureBox square in activePiece2)
                    {
                        square.BackColor = Color.Red;
                        activePiece[x] = square;
                        x++;
                    }
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                //Rotate
            }
            if (e.KeyCode == Keys.Down)
            {
                bool canMove = true;
                foreach (Control square in activePiece)
                {
                    int squareRow = grid.GetRow(square);
                    int squareCol = grid.GetColumn(square);
                    if (currentLowRow < 9)
                    {
                        Control lowSquare = grid.GetControlFromPosition(squareCol, squareRow + 1);
                        if (lowSquare.BackColor == Color.Red & activePiece.Contains(lowSquare) == false & currentLowRow > 0)
                        {
                            canMove = false;
                            break;
                        }
                    }
                    else
                    {
                        canMove = false;
                        break;

                    }
                }

                if (canMove == true)
                {
                    //Move piece down
                    int x = 0;
                    foreach (PictureBox square in activePiece)
                    {
                        square.BackColor = Color.White;
                        int squareRow = grid.GetRow(square);
                        int squareCol = grid.GetColumn(square);
                        activePiece2[x] = grid.GetControlFromPosition(squareCol, squareRow + 1);
                        x++;
                    }
                    x = 0;
                    foreach (PictureBox square in activePiece2)
                    {
                        square.BackColor = Color.Red;
                        activePiece[x] = square;
                        x++;
                    }
                }
            }

        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Update timer
            timeElapsed++;
            label2.Text = "Time: " + timeElapsed.ToString();

            int currentRow = 0;
            int currentCol = 0;

            int currentHighRow = 9;
            int currentLowRow = 0;
            int currentLeftCol = 9;
            int currentRightCol = 0;
            foreach (Control square in activePiece)
            {
                if (grid.GetRow(square) < currentHighRow)
                {
                    currentHighRow = grid.GetRow(square);
                }
                if (grid.GetRow(square) > currentLowRow)
                {
                    currentLowRow = grid.GetRow(square);
                }
                if (grid.GetColumn(square) < currentLeftCol)
                {
                    currentLeftCol = grid.GetColumn(square);
                }
                if (grid.GetColumn(square) > currentRightCol)
                {
                    currentRightCol = grid.GetColumn(square);
                }
            }
            bool canMove = true;
            foreach (Control square in activePiece)
            {
                int squareRow = grid.GetRow(square);
                int squareCol = grid.GetColumn(square);
                if (currentLowRow < 9)
                {
                    Control lowSquare = grid.GetControlFromPosition(squareCol, squareRow + 1);
                    if (lowSquare.BackColor == Color.Red & activePiece.Contains(lowSquare) == false & currentLowRow < 9)
                    {
                        canMove = false;
                        break;
                    }
                }
                else
                {
                    canMove = false;
                    break;

                }
            }

            if (canMove == true)
            {
                //Move piece down
                int x = 0;
                foreach (PictureBox square in activePiece)
                {
                    square.BackColor = Color.White;
                    int squareRow = grid.GetRow(square);
                    int squareCol = grid.GetColumn(square);
                    activePiece2[x] = grid.GetControlFromPosition(squareCol, squareRow + 1);
                    x++;
                }
                x = 0;
                foreach (PictureBox square in activePiece2)
                {
                    square.BackColor = Color.Red;
                    activePiece[x] = square;
                    x++;
                }
            }
            else
            {
                dropNewPiece();
            }
        }
    }
}
