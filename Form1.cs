using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
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
        int currentPiece;
        int rotations = 0;
        Color pieceColor = Color.White;

        public Form1()
        {
            InitializeComponent();
            dropNewPiece();
            timer1.Start();
        }

        public void dropNewPiece()
        {
            rotations = 0;
            System.Random random = new System.Random();
           
            currentPiece = random.Next(7);
            //currentPiece = 0;

            if (currentPiece == 0)
            {
                activePiece[0] = pictureBox6;
                activePiece[1] = pictureBox16;
                activePiece[2] = pictureBox26;
                activePiece[3] = pictureBox36;
                pieceColor = Color.Red;
            }
            else if (currentPiece == 1)
            {
                activePiece[0] = pictureBox4;
                activePiece[1] = pictureBox14;
                activePiece[2] = pictureBox24;
                activePiece[3] = pictureBox25;
                pieceColor = Color.Blue;
            }
            else if (currentPiece == 2)
            {
                activePiece[0] = pictureBox5;
                activePiece[1] = pictureBox15;
                activePiece[2] = pictureBox25;
                activePiece[3] = pictureBox24;
                pieceColor = Color.Yellow;
            }
            else if (currentPiece == 3)
            {
                activePiece[0] = pictureBox14;
                activePiece[1] = pictureBox15;
                activePiece[2] = pictureBox5;
                activePiece[3] = pictureBox6;
                pieceColor = Color.Orange;
            }
            else if (currentPiece == 4)
            {
                activePiece[0] = pictureBox5;
                activePiece[1] = pictureBox6;
                activePiece[2] = pictureBox16;
                activePiece[3] = pictureBox17;
                pieceColor = Color.Purple;
            }
            else if (currentPiece == 5)
            {
                activePiece[0] = pictureBox5;
                activePiece[1] = pictureBox6;
                activePiece[2] = pictureBox15;
                activePiece[3] = pictureBox16;
                pieceColor = Color.Green;
            }
            else if (currentPiece == 6)
            {
                activePiece[0] = pictureBox6;
                activePiece[1] = pictureBox15;
                activePiece[2] = pictureBox16;
                activePiece[3] = pictureBox17;
                pieceColor = Color.Brown;
            }

            foreach (Control square in activePiece)
            {
                square.BackColor = pieceColor;
            }
        }
            
        public bool testMove(string direction)
        {
            int currentHighRow = 19;
            int currentLowRow = 0;
            int currentLeftCol = 9;
            int currentRightCol = 0;

            int nextSquare = 0;

            Control newSquare = new Control();

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

            foreach (Control square in activePiece)
            {
                int squareRow = grid.GetRow(square);
                int squareCol = grid.GetColumn(square);

                //Left
                if (direction == "left" & squareCol > 0)
                {
                    newSquare = grid.GetControlFromPosition(squareCol - 1, squareRow);
                    nextSquare = currentLeftCol;
                }
                else if (direction == "left" & squareCol == 0)
                {
                    return false;
                }

                //Right
                else if (direction == "right" & squareCol < 9)
                {
                    newSquare = grid.GetControlFromPosition(squareCol + 1, squareRow);
                    nextSquare = currentRightCol;
                }
                else if (direction == "right" & squareCol == 9)
                {
                    return false;
                }

                //Down
                else if (direction == "down" & squareRow < 19)
                {
                    newSquare = grid.GetControlFromPosition(squareCol, squareRow + 1);
                    nextSquare = currentLowRow;
                }
                else if (direction == "down" & squareRow == 19)
                {
                    return false;
                }

                if (newSquare.BackColor != Color.White & activePiece.Contains(newSquare) == false & nextSquare > 0)
                {
                    //MessageBox.Show("test2");
                    return false;
                }

            }

            return true;
        }

        public void movePiece(string direction)
        {
            int x = 0;
            foreach (PictureBox square in activePiece)
            {
                square.BackColor = Color.White;
                int squareRow = grid.GetRow(square);
                int squareCol = grid.GetColumn(square);
                int newSquareRow = 0;
                int newSquareCol = 0;
                if (direction == "left")
                {
                    newSquareCol = squareCol - 1;
                    newSquareRow = squareRow;
                }
                else if (direction == "right")
                {
                    newSquareCol = squareCol + 1;
                    newSquareRow = squareRow;
                }
                else if (direction == "down")
                {
                    newSquareCol = squareCol;
                    newSquareRow = squareRow + 1;
                }

                activePiece2[x] = grid.GetControlFromPosition(newSquareCol, newSquareRow);
                x++;
            }
            x = 0;
            foreach (PictureBox square in activePiece2)
            {
                square.BackColor = pieceColor;
                activePiece[x] = square;
                x++;
            }
        }

        private bool testOverlap()
        {
            foreach (PictureBox square in activePiece2)
            {
                if (square.BackColor != Color.White & activePiece.Contains(square) == false)
                {
                    return false;
                }
            }
            return true;
        }
                    

    private void Form1_KeyDown(object sender, KeyEventArgs e)
        {   
            if (e.KeyCode == Keys.Left & testMove("left") == true)
            {
                movePiece("left");
            }
            else if (e.KeyCode == Keys.Right & testMove("right") == true)
            {
                movePiece("right");
            }
            else if (e.KeyCode == Keys.Down & testMove("down") == true)
            {
                movePiece("down");
            }
            else if (e.KeyCode == Keys.Up)
            {
                //Rotate

                int square1Col = grid.GetColumn(activePiece[0]);
                int square1Row = grid.GetRow(activePiece[0]);

                int square2Col = grid.GetColumn(activePiece[1]);
                int square2Row = grid.GetRow(activePiece[1]);

                int square3Col = grid.GetColumn(activePiece[2]);
                int square3Row = grid.GetRow(activePiece[2]);

                int square4Col = grid.GetColumn(activePiece[3]);
                int square4Row = grid.GetRow(activePiece[3]);

                if (currentPiece == 0) //The line piece
                {
                    //Test if piece is too close to edge of board
                    if (rotations == 0 & (square1Col == 0 | square1Col == 1 | square1Col == 9))
                    {
                        return;
                    }
                    else if (rotations == 1 & (square3Col == 0 | square3Col == 1 | square3Col == 9))
                    {
                        return;
                    }

                    //If test passes, rotate piece
                    if (rotations == 0)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col - 2, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col - 1, square2Row - 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col, square3Row - 2);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col + 1, square4Row - 3);
                        
                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }  
                    }
                    else if (rotations == 1)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col + 2, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col + 1, square2Row + 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col, square3Row + 2);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col - 1, square4Row + 3);
                        
                        if (testOverlap() == true)
                        {
                            rotations = 0;
                        }
                        else
                        {
                            return;
                        }
                    }


                }        
                else if (currentPiece == 1) //The normal L
                {
                    //Test if piece is too close to edge of board
                    if (rotations == 0 & (square1Col == 8 | square1Col == 9))
                    {
                        return;
                    }
                    else if (rotations == 2 & (square1Col == 9))
                    {
                        return;
                    }

                    //If test passes, rotate piece
                    if (rotations == 0)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row + 2);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col + 1, square2Row + 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col + 2, square3Row);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col + 1, square4Row - 1);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 1)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col + 1, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row - 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col - 1, square3Row - 2);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col - 2, square4Row - 1);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 2)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col + 1, square1Row - 1);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col - 1, square3Row + 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col, square4Row + 2);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 3)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col - 2, square1Row - 1);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col - 1, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col, square3Row + 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col + 1, square4Row);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations = 0;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else if (currentPiece == 2) //The backwards L
                {
                    //Test if piece is too close to edge of board
                    if (rotations == 0 & square1Col == 0)
                    {
                        return;
                    }
                    else if (rotations == 2 & square1Col == 0)
                    {
                        return;
                    }

                    //If test passes, rotate piece
                    if (rotations == 0)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col - 2, square1Row + 1);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col - 1, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col, square3Row - 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col + 1, square4Row);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 1)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col + 1, square1Row + 1);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col - 1, square3Row - 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col, square4Row - 2);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 2)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col + 1, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row + 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col - 1, square3Row + 2);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col - 2, square4Row + 1);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 3)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row - 2);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col + 1, square2Row - 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col + 2, square3Row);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col + 1, square4Row + 1);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations = 0;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else if (currentPiece == 3) //The normal S
                {
                    //Test if piece is too close to edge of board
                    if (rotations == 0 & (square1Row == 1 | square1Col == 9))
                    {
                        return;
                    }
                    else if (rotations == 1 & square1Col == 0)
                    {
                        return;
                    }

                    //If test passes, rotate piece
                    if (rotations == 0)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col + 1, square1Row - 2);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row - 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col + 1, square3Row);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col, square4Row + 1);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 1)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col - 1, square1Row + 2);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row + 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col - 1, square3Row);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col, square4Row - 1);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations = 0;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else if (currentPiece == 4) //The backwards S
                {
                    //Test if piece is too close to edge of board
                    if (rotations == 0 & square1Row == 0)
                    {
                        return;
                    }
                    else if (rotations == 1 & (square1Row == 0 | square1Col == 8))
                    {
                        return;
                    }

                    //If test passes, rotate piece
                    if (rotations == 0)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row + 1);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col - 1, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col, square3Row - 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col - 1, square4Row - 2);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 1)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row - 1);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col + 1, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col, square3Row + 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col + 1, square4Row + 2);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations = 0;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else if (currentPiece == 5) //The square
                {
                    //The square cannot rotate
                    return;
                }
                else if (currentPiece == 6) //The pyramid
                {
                    //Test if piece is too close to edge of board
                    if (rotations == 0 & square1Row == 0)
                    {
                        return;
                    }
                    else if (rotations == 1 & (square1Row == 0 | square1Col == 9))
                    {
                        return;
                    }
                    else if (rotations == 2 & square1Row == 0)
                    {
                        return;
                    }
                    else if (rotations == 3 & (square1Row == 0 | square1Col == 0))
                    {
                        return;
                    }

                    //If test passes, rotate piece
                    if (rotations == 0)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row - 2);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col - 1, square3Row - 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col - 2, square4Row);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 1)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col + 2, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col + 1, square3Row - 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col, square4Row - 2);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 2)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row + 2);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col + 1, square3Row + 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col + 2, square4Row);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 3)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col - 2, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col - 1, square3Row + 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col, square4Row + 2);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        if (testOverlap() == true)
                        {
                            rotations = 0;
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                //Set old position of piece to white
                foreach (PictureBox square in activePiece)
                {
                    square.BackColor = Color.White;
                }

                //Set new position of piece to that piece's color
                int x = 0;
                foreach (PictureBox square in activePiece2)
                {
                    square.BackColor = pieceColor;
                    activePiece[x] = square;
                    x++;
                }
            }

        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Update timer
            timeElapsed++;
            label2.Text = "Time: " + timeElapsed.ToString();

            //Move piece down, or drop new piece if it can't move
            if (testMove("down") == true)
            {
                movePiece("down");
            }
            else
            {
                dropNewPiece();
            }
        }
    }
}