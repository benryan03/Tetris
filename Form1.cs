using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        Control[] activePiece = { null, null, null, null };
        Control[] activePiece2 = { null, null, null, null };
        Control[] nextPiece = { null, null, null, null };
        Control[] savedPiece = { null, null, null, null };
        int timeElapsed = 0;
        int currentPiece;
        int nextPieceInt;
        int savedPieceInt = -1;
        int rotations = 0;
        Color pieceColor = Color.White;
        Color nextPieceColor = Color.White;
        Color savedPieceColor = Color.White;
        int combo = 0;
        int score = 0;
        int clears = 0;
        int level = 0;

        //I piece, L piece, J piece, S piece, Z piece, O piece, T piece
        readonly Color[] colorList = { Color.Cyan, Color.Orange, Color.Blue, Color.Green, Color.Red, Color.Yellow, Color.Purple };

        public Form1()
        {
            InitializeComponent();
            timer1.Start();
            timer2.Start();

            System.Random random = new System.Random();
            nextPieceInt = random.Next(7);

            DropNewPiece();
        }

        public void DropNewPiece()
        {
            rotations = 0;

            //Move next piece to current piece
            currentPiece = nextPieceInt;
            //currentPiece = 3; //debug

            //Determine next piece
            System.Random random = new System.Random();
            nextPieceInt = random.Next(7);

            //If not first move, clear next piece panel
            if (nextPiece.Contains(null) == false)
            {
                foreach (Control x in nextPiece)
                {
                    x.BackColor = Color.White;
                }
            }

            ///////////////////////////////
            //Layout options for next piece
            Control[,] nextPieceArray = 
            {
                { box203, box207, box211, box215 }, // I piece
                { box202, box206, box210, box211 }, // L piece
                { box203, box207, box211, box210 }, // J piece
                { box206, box207, box203, box204 }, // S piece
                { box202, box203, box207, box208 }, // Z piece
                { box206, box207, box210, box211 }, // O piece
                { box207, box210, box211, box212 }  // T piece
            };

            //Select generated next piece
            for (int x = 0; x < 4; x++)
            {
                nextPiece[x] = nextPieceArray[nextPieceInt,x];
            }

            //Populate next piece panel with correct color
            foreach (Control square in nextPiece)
            {
                square.BackColor = colorList[nextPieceInt];
            }

            //////////////////////////////////
            //Layout options for falling piece
            Control[,] activePieceArray =
            {
                { box6, box16, box26, box36 }, // I piece
                { box4, box14, box24, box25 }, // L piece
                { box5, box15, box25, box24 }, // J piece
                { box14, box15, box5, box6 },  // S piece
                { box5, box6, box16, box17 },  // Z piece
                { box5, box6, box15, box16 },  // O piece
                { box6, box15, box16, box17 }  // T piece
            };

            //Select falling piece
            for (int x = 0; x < 4; x++)
            {
                activePiece[x] = activePieceArray[currentPiece, x];
            }

            //Populate falling piece squares with correct color
            foreach (Control square in activePiece)
            {
                square.BackColor = colorList[currentPiece];
            }
        }
            
        public bool TestMove(string direction)
        {
            int currentHighRow = 21;
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
                else if (direction == "down" & squareRow < 21)
                {
                    newSquare = grid.GetControlFromPosition(squareCol, squareRow + 1);
                    nextSquare = currentLowRow;
                }
                else if (direction == "down" & squareRow == 21)
                {
                    return false;
                }

                if (newSquare.BackColor != Color.White & activePiece.Contains(newSquare) == false & nextSquare > 0)
                {
                    return false;
                }

            }

            return true;
        }

        public void MovePiece(string direction)
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
                square.BackColor = colorList[currentPiece];
                //square.BackColor = pieceColor;
                activePiece[x] = square;
                x++;
            }
        }

        private bool TestOverlap()
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
            if ((e.KeyCode == Keys.Left | e.KeyCode == Keys.A) & TestMove("left") == true)
            {
                MovePiece("left");
            }
            else if ((e.KeyCode == Keys.Right | e.KeyCode == Keys.D) & TestMove("right") == true)
            {
                MovePiece("right");
            }
            else if ((e.KeyCode == Keys.Down | e.KeyCode == Keys.S) & TestMove("down") == true)
            {
                MovePiece("down");
            }
            else if (e.KeyCode == Keys.Up | e.KeyCode == Keys.W)
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
                        if (TestOverlap() == true)
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
                        
                        if (TestOverlap() == true)
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
                        if (TestOverlap() == true)
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
                        if (TestOverlap() == true)
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
                        if (TestOverlap() == true)
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
                        if (TestOverlap() == true)
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
                    if (rotations == 0 & (square1Col == 0 | square1Col == 1))
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
                        if (TestOverlap() == true)
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
                        if (TestOverlap() == true)
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
                        if (TestOverlap() == true)
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
                        if (TestOverlap() == true)
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
                        if (TestOverlap() == true)
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
                        if (TestOverlap() == true)
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
                    if (rotations == 1 & square1Col == 8)
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
                        if (TestOverlap() == true)
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
                        if (TestOverlap() == true)
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
                    if (rotations == 1 & square1Col == 9)
                    {
                        return;
                    }
                    else if (rotations == 3 & square1Col == 0)
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
                        if (TestOverlap() == true)
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
                        if (TestOverlap() == true)
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
                        if (TestOverlap() == true)
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
                        if (TestOverlap() == true)
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
                    square.BackColor = colorList[currentPiece];
                    activePiece[x] = square;
                    x++;
                }
            }
            else if (e.KeyCode == Keys.ShiftKey)
            {
                rotations = 0;

                if (savedPieceInt == -1)
                {
                    foreach (Control x in activePiece)
                    {
                        x.BackColor = Color.White;
                    }

                    savedPieceInt = currentPiece;

                    if (savedPieceInt == 0)
                    {
                        savedPiece[0] = box219;
                        savedPiece[1] = box223;
                        savedPiece[2] = box227;
                        savedPiece[3] = box231;
                        savedPieceColor = Color.Cyan;
                    }
                    else if (savedPieceInt == 1)
                    {
                        savedPiece[0] = box218;
                        savedPiece[1] = box222;
                        savedPiece[2] = box226;
                        savedPiece[3] = box227;
                        savedPieceColor = Color.Orange;
                    }
                    else if (savedPieceInt == 2)
                    {
                        savedPiece[0] = box219;
                        savedPiece[1] = box223;
                        savedPiece[2] = box227;
                        savedPiece[3] = box210;
                        savedPieceColor = Color.Blue;
                    }
                    else if (savedPieceInt == 3)
                    {
                        savedPiece[0] = box222;
                        savedPiece[1] = box223;
                        savedPiece[2] = box219;
                        savedPiece[3] = box220;
                        savedPieceColor = Color.Green;
                    }
                    else if (savedPieceInt == 4)
                    {
                        savedPiece[0] = box218;
                        savedPiece[1] = box219;
                        savedPiece[2] = box222;
                        savedPiece[3] = box223;
                        savedPieceColor = Color.Red;
                    }
                    else if (savedPieceInt == 5)
                    {
                        savedPiece[0] = box222;
                        savedPiece[1] = box223;
                        savedPiece[2] = box226;
                        savedPiece[3] = box227;
                        savedPieceColor = Color.Yellow;
                    }
                    else if (savedPieceInt == 6)
                    {
                        savedPiece[0] = box223;
                        savedPiece[1] = box226;
                        savedPiece[2] = box227;
                        savedPiece[3] = box228;
                        savedPieceColor = Color.Purple;
                    }

                    DropNewPiece();

                    foreach (Control x in savedPiece)
                    {
                        x.BackColor = savedPieceColor;
                    }
                }
                else
                {
                    //Erase falling piece
                    foreach (Control x in activePiece)
                    {
                        x.BackColor = Color.White;
                    }
                    //Erase saved piece
                    foreach (Control x in savedPiece)
                    {
                        x.BackColor = Color.White;
                    }

                    //Swap pieces
                    int savedPieceTemp = currentPiece;
                    currentPiece = savedPieceInt;
                    savedPieceInt = savedPieceTemp;

                    //Populate squares in saved piece panel
                    if (savedPieceInt == 0)
                    {
                        savedPiece[0] = box219;
                        savedPiece[1] = box223;
                        savedPiece[2] = box227;
                        savedPiece[3] = box231;
                        savedPieceColor = Color.Cyan;
                    }
                    else if (savedPieceInt == 1)
                    {
                        savedPiece[0] = box218;
                        savedPiece[1] = box222;
                        savedPiece[2] = box226;
                        savedPiece[3] = box227;
                        savedPieceColor = Color.Orange;
                    }
                    else if (savedPieceInt == 2)
                    {
                        savedPiece[0] = box219;
                        savedPiece[1] = box223;
                        savedPiece[2] = box227;
                        savedPiece[3] = box226;
                        savedPieceColor = Color.Blue;
                    }
                    else if (savedPieceInt == 3)
                    {
                        savedPiece[0] = box222;
                        savedPiece[1] = box223;
                        savedPiece[2] = box219;
                        savedPiece[3] = box220;
                        savedPieceColor = Color.Green;
                    }
                    else if (savedPieceInt == 4)
                    {
                        savedPiece[0] = box218;
                        savedPiece[1] = box219;
                        savedPiece[2] = box223;
                        savedPiece[3] = box224;
                        savedPieceColor = Color.Red;
                    }
                    else if (savedPieceInt == 5)
                    {
                        savedPiece[0] = box222;
                        savedPiece[1] = box223;
                        savedPiece[2] = box226;
                        savedPiece[3] = box227;
                        savedPieceColor = Color.Yellow;
                    }
                    else if (savedPieceInt == 6)
                    {
                        savedPiece[0] = box223;
                        savedPiece[1] = box226;
                        savedPiece[2] = box227;
                        savedPiece[3] = box228;
                        savedPieceColor = Color.Purple;
                    }

                    foreach (Control x in savedPiece)
                    {
                        x.BackColor = savedPieceColor;
                    }




                    //Populate new falling piece
                    if (currentPiece == 0)
                    {
                        activePiece[0] = box6;
                        activePiece[1] = box16;
                        activePiece[2] = box26;
                        activePiece[3] = box36;
                        pieceColor = Color.Cyan;
                    }
                    else if (currentPiece == 1)
                    {
                        activePiece[0] = box4;
                        activePiece[1] = box14;
                        activePiece[2] = box24;
                        activePiece[3] = box25;
                        pieceColor = Color.Orange;
                    }
                    else if (currentPiece == 2)
                    {
                        activePiece[0] = box5;
                        activePiece[1] = box15;
                        activePiece[2] = box25;
                        activePiece[3] = box24;
                        pieceColor = Color.Blue;
                    }
                    else if (currentPiece == 3)
                    {
                        activePiece[0] = box14;
                        activePiece[1] = box15;
                        activePiece[2] = box5;
                        activePiece[3] = box6;
                        pieceColor = Color.Green;
                    }
                    else if (currentPiece == 4)
                    {
                        activePiece[0] = box5;
                        activePiece[1] = box6;
                        activePiece[2] = box16;
                        activePiece[3] = box17;
                        pieceColor = Color.Red;
                    }
                    else if (currentPiece == 5)
                    {
                        activePiece[0] = box5;
                        activePiece[1] = box6;
                        activePiece[2] = box15;
                        activePiece[3] = box16;
                        pieceColor = Color.Yellow;
                    }
                    else if (currentPiece == 6)
                    {
                        activePiece[0] = box6;
                        activePiece[1] = box15;
                        activePiece[2] = box16;
                        activePiece[3] = box17;
                        pieceColor = Color.Purple;
                    }

                    foreach (Control square in activePiece)
                    {
                        square.BackColor = pieceColor;
                    }

                }
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //Move piece down, or drop new piece if it can't move
            if (TestMove("down") == true)
            {
                MovePiece("down");
            }
            else
            {
                if (CheckForCompleteRows() > -1)
                {
                    ClearFullRow();
                }
                DropNewPiece();
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            //Update timer
            timeElapsed++;
            label2.Text = "Time: " + timeElapsed.ToString();
        }

        private void ClearFullRow()
        {
            int completedRow = CheckForCompleteRows();

            //Turn that row white
            for (int x = 0; x <= 9; x++)
            {
                Control z = grid.GetControlFromPosition(x, completedRow);
                z.BackColor = Color.White;
            }

            //Move all other squares down
            //For each above cleared row
            for (int x = completedRow - 1; x >= 0; x--)
            {
                //For each square in row
                for (int y = 0; y <= 9; y++)
                {
                    //the square
                    Control z = grid.GetControlFromPosition(y, x);

                    //the square below it
                    Control zz = grid.GetControlFromPosition(y, x + 1);

                    zz.BackColor = z.BackColor;
                    z.BackColor = Color.White;
                }
            }

            //Update score
            if (CheckForCompleteRows() == -1)
            {
                if (combo % 3 != 0)
                {
                    combo = 0;
                }
            }

            if (combo < 3)
            {
                score = score + 100;
            }
            else if (combo == 3)
            {
                score = score + 500;
            }
            else if (combo > 3 & combo < 6)
            {
                score = score + 100;
            }
            else if (combo >= 6)
            {
                score = score + 900;
            }

            combo++;
            label3.Text = "Score: " + score.ToString();

            clears++;
            label4.Text = "Lines cleared: " + clears;

            if (clears % 10 == 0)
            {
                LevelUp();
            }

            if (CheckForCompleteRows() > -1)
            {
                //combo++;
                ClearFullRow();
            }
            else
            {
                if (combo % 4 != 0)
                {
                    combo = 0;
                }
            }
        }  

        private int CheckForCompleteRows()
        {
            //For each row
            for (int x = 21; x >= 2; x--)
            {


                //For each square in row
                for (int y = 0; y <= 9; y++)
                {
                    Control z = grid.GetControlFromPosition(y /* col */, x /* row */);
                    if (z.BackColor == Color.White)
                    {
                        break;
                    }
                    if (y == 9)
                    {
                        //Return the row that is full
                        return x;
                    }
                }
            }
            return -1; //"null"
        }

        private void LevelUp()
        {
            level++;
            label5.Text = "Level " + level.ToString();

            //Milliseconds per square fall
            //Level 1 = 800 ms per square, level 2 = 716 ms per square, etc.
            int[] levelSpeed =
            {
                800, 716, 633, 555, 466, 383, 300, 216, 133, 100, 083, 083, 083, 066, 066,
                066, 050, 050, 050, 033, 033, 033, 033, 033, 033, 033, 033, 033, 033, 016
            };

            //Level speed does not change after level 29
            if (level <= 29)
            {
                timer1.Interval = levelSpeed[level];
            }
        }
    }   
}