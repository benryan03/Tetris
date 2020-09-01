using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Tetris
{
    public partial class Form1 : Form
    {
        Control[] activePiece = { null, null, null, null };
        Control[] activePiece2 = { null, null, null, null };
        Control[] nextPiece = { null, null, null, null };
        Control[] savedPiece = { null, null, null, null };
        Control[] Ghost = { null, null, null, null };
        List<int> PieceSequence = new List<int>();
        int timeElapsed = 0;
        int currentPiece;
        int nextPieceInt;
        int savedPieceInt = -1;
        int rotations = 0;
        Color pieceColor = Color.White;
        Color savedPieceColor = Color.White;
        int combo = 0;
        int score = 0;
        int clears = 0;
        int level = 0;
        bool gameOver = false;
        int PieceSequenceIteration = 0;

        readonly Color[] colorList = 
        {  
            Color.Cyan,     // I piece
            Color.Orange,   // L piece
            Color.Blue,     // J piece
            Color.Green,    // S piece
            Color.Red,      // Z piece
            Color.Yellow,   // O piece
            Color.Purple    // T piece
        };

        public Form1()      
        {
            InitializeComponent();

            label8.Text = "";
            timer1.Start();
            timer2.Start();

            // Initialize ghost piece
            activePiece2[0] = box1;
            activePiece2[1] = box2;
            activePiece2[2] = box3;
            activePiece2[3] = box4;

            // Generate piece sequence
            System.Random random = new System.Random();
            while (PieceSequence.Count < 7)
            {
                int x = random.Next(7);
                if (!PieceSequence.Contains(x))
                {
                    PieceSequence.Add(x);
                }
            }

            nextPieceInt = PieceSequence[0];
            PieceSequenceIteration++;

            DropNewPiece();
        }

        public void DropNewPiece()
        {
            // Reset number of times current piece has been rotated
            rotations = 0;

            // Move next piece to current piece
            currentPiece = nextPieceInt;

            // If last piece of Bag1 set, generate new set
            if (PieceSequenceIteration == 7)
            {
                PieceSequenceIteration = 0;

                // Scramble Bag1
                PieceSequence.Clear();
                System.Random random = new System.Random();
                while (PieceSequence.Count < 7)
                {
                    int x = random.Next(7);
                    if (!PieceSequence.Contains(x))
                    {
                        PieceSequence.Add(x);
                    }
                }
            }

            // Select next piece from Bag1 set
            nextPieceInt = PieceSequence[PieceSequenceIteration];
            PieceSequenceIteration++;

            // If not first move, clear next piece panel
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

            // Retrieve layout for next piece
            for (int x = 0; x < 4; x++)
            {
                nextPiece[x] = nextPieceArray[nextPieceInt,x];
            }

            // Populate next piece panel with correct color
            foreach (Control square in nextPiece)
            {
                square.BackColor = colorList[nextPieceInt];
            }

            ///////////////////////////////////
            // Layout options for falling piece
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

            //This is needed for DrawGhost
            for (int x = 0; x < 4; x++)
            {
                activePiece2[x] = activePieceArray[currentPiece, x];
            }

            //Check for game over
            foreach (Control box in activePiece)
            {
                if (box.BackColor != Color.White & box.BackColor != Color.LightGray)
                {
                    //Game over!
                    timer1.Stop();
                    timer2.Stop();
                    gameOver = true;
                    MessageBox.Show("Game over!");
                    return;
                }
            }

            DrawGhost();

            //Populate falling piece squares with correct color
            foreach (Control square in activePiece)
            {
                square.BackColor = colorList[currentPiece];
            }
        }

        // Test if a potential move (left/right/down) would be outside the board or inside another piece
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

                if ((newSquare.BackColor != Color.White & newSquare.BackColor != Color.LightGray) & activePiece.Contains(newSquare) == false & nextSquare > 0)
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

                activePiece[x] = square;
                x++;
            }

            DrawGhost();


            x = 0;
            foreach (PictureBox square in activePiece2)
            {
                square.BackColor = colorList[currentPiece];
                x++;
            }

        }

        // Test if a potential rotation would be inside another piece
        private bool TestOverlap()
        {
            foreach (PictureBox square in activePiece2)
            {
                if ((square.BackColor != Color.White & square.BackColor != Color.LightGray) & activePiece.Contains(square) == false)
                {
                    return false;
                }
            }
            return true;
        }
        
        // Detect inputs
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {   
            if (!CheckGameOver() & ((e.KeyCode == Keys.Left | e.KeyCode == Keys.A) & TestMove("left") == true))
            {
                MovePiece("left");
            }
            else if (!CheckGameOver() & ((e.KeyCode == Keys.Right | e.KeyCode == Keys.D) & TestMove("right") == true))
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

                DrawGhost();


                //Set new position of piece to that piece's color
                int x = 0;
                foreach (PictureBox square in activePiece2)
                {
                    square.BackColor = colorList[currentPiece];
                    activePiece[x] = square;
                    x++;
                }





            }
            else if (!CheckGameOver() & e.KeyCode == Keys.ShiftKey)
            {
                rotations = 0;

                //If no piece has been saved yet
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
                        activePiece2[0] = box6;
                        activePiece2[1] = box16;
                        activePiece2[2] = box26;
                        activePiece2[3] = box36;
                        pieceColor = Color.Cyan;
                    }
                    else if (currentPiece == 1)
                    {
                        activePiece2[0] = box4;
                        activePiece2[1] = box14;
                        activePiece2[2] = box24;
                        activePiece2[3] = box25;
                        pieceColor = Color.Orange;
                    }
                    else if (currentPiece == 2)
                    {
                        activePiece2[0] = box5;
                        activePiece2[1] = box15;
                        activePiece2[2] = box25;
                        activePiece2[3] = box24;
                        pieceColor = Color.Blue;
                    }
                    else if (currentPiece == 3)
                    {
                        activePiece2[0] = box14;
                        activePiece2[1] = box15;
                        activePiece2[2] = box5;
                        activePiece2[3] = box6;
                        pieceColor = Color.Green;
                    }
                    else if (currentPiece == 4)
                    {
                        activePiece2[0] = box5;
                        activePiece2[1] = box6;
                        activePiece2[2] = box16;
                        activePiece2[3] = box17;
                        pieceColor = Color.Red;
                    }
                    else if (currentPiece == 5)
                    {
                        activePiece2[0] = box5;
                        activePiece2[1] = box6;
                        activePiece2[2] = box15;
                        activePiece2[3] = box16;
                        pieceColor = Color.Yellow;
                    }
                    else if (currentPiece == 6)
                    {
                        activePiece2[0] = box6;
                        activePiece2[1] = box15;
                        activePiece2[2] = box16;
                        activePiece2[3] = box17;
                        pieceColor = Color.Purple;
                    }

                    foreach (Control square in activePiece2)
                    {
                        square.BackColor = pieceColor;
                    }

                    DrawGhost();

                    for (int x = 0; x < 4; x++)
                    {
                        activePiece[x] = activePiece2[x];
                    }

                }



            }
            else if (!CheckGameOver() & e.KeyCode == Keys.Space)
            {
                // Hard drop
                for (int x = 0; x < 4; x++)
                {
                    Ghost[x].BackColor = colorList[currentPiece];
                    activePiece[x].BackColor = Color.White;
                }
                if (CheckForCompleteRows() > -1)
                {
                    ClearFullRow();
                }
                DropNewPiece();
            }
        }

        // Timer for piece movement speed - increases with game level
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (CheckGameOver() == true)
            {
                timer1.Stop();
                timer2.Stop();
                MessageBox.Show("Game over!");
            }

            else
            {
                //Move piece down, or drop new piece if it can't move
                if (TestMove("down") == true)
                {
                    MovePiece("down");
                }
                else
                {
                    if (CheckGameOver() == true)
                    {
                        timer1.Stop();
                        timer2.Stop();
                        MessageBox.Show("Game over!");
                    }
                    if (CheckForCompleteRows() > -1)
                    {
                        ClearFullRow();
                    }
                    DropNewPiece();
                }
            }
        }

        // Game time (seconds elapsed)
        private void Timer2_Tick(object sender, EventArgs e)
        {
            timeElapsed++;
            label2.Text = "Time: " + timeElapsed.ToString();
        }

        // Clear lowest full row
        // This is very messy :(
        private void ClearFullRow()
        {
            int completedRow = CheckForCompleteRows();
            bool resetCombo = false;

            //Turn that row white
            for (int x = 0; x <= 9; x++)
            {
                Control z = grid.GetControlFromPosition(x, completedRow);
                z.BackColor = Color.White;
            }

            //Move all other squares down
            for (int x = completedRow - 1; x >= 0; x--)             //For each row above cleared row
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
            if (combo == 0)
            {
                score = score + 100;
                label8.Text = "+100";
                timer3.Start();
                if (CheckForCompleteRows() == -1)
                {
                    combo = -1;
                }
            }

            else if (combo == 1)
            {
                score = score + 100;
                label8.Text = "+200";
                timer3.Start();
                if (CheckForCompleteRows() == -1)
                {
                    combo = -1;
                }
            }

            else if (combo == 2)
            {
                score = score + 100;
                label8.Text = "+300";
                timer3.Start();
                if (CheckForCompleteRows() == -1)
                {
                    combo = -1;
                }
            }

            else if (combo == 3)
            {
                score = score + 500;
                label8.Text = "+800";
                timer3.Start();
            }

            else if (combo > 3)
            {
                if (combo % 4 == 0)
                {
                    score = score + 100;
                    label8.Text = "+100";
                    timer3.Start();
                    if (CheckForCompleteRows() == -1)
                    {
                        combo = -1;
                    }
                }
                else if ((combo - 1) % 4 == 0)
                {
                    score = score + 100;
                    label8.Text = "+200";
                    timer3.Start();
                    if (CheckForCompleteRows() == -1)
                    {
                        combo = -1;
                    }
                }
                else if ((combo - 2) % 4 == 0)
                {
                    score = score + 100;
                    label8.Text = "+300";
                    timer3.Start();
                    if (CheckForCompleteRows() == -1)
                    {
                        combo = -1;
                    }
                }
                else if ((combo - 3) % 4 == 0)
                {
                    score = score + 900;
                    label8.Text = "+1200";
                    timer3.Start();
                }

            }


            combo++;


            if (resetCombo == true)
            {
                combo = 0;
            }



            label3.Text = "Score: " + score.ToString();

            clears++;
            label4.Text = "Clears: " + clears;

            if (clears % 10 == 0)
            {
                LevelUp();
            }

            if (CheckForCompleteRows() > -1)
            {
                ClearFullRow();
        
            }

            //label1.Text = combo.ToString(); //debug


        }

        // Return row number of lowest full row
        // If no full rows, return -1
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
            label5.Text = "Level: " + level.ToString();

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

        private bool CheckGameOver()
        {
            Control[] topRow = { box1, box2, box3, box4, box5, box6, box7, box8, box9, box10 };

            foreach (Control box in topRow)
            {
                if ((box.BackColor != Color.White & box.BackColor != Color.LightGray) & !activePiece.Contains(box))
                {
                    //Game over!
                    return true;
                }
            }

            if (gameOver == true)
            {
                return true;
            }

            return false;
        }

        // Display gray preview of hard drop position
        private void DrawGhost()
        {

            Control[] Ghost2 = { null, null, null, null };
            bool ghostFound = false;

            // Erase previous Ghost
            foreach (Control x in Ghost)
            {
                if (x != null )
                {
                    if (x.BackColor == Color.LightGray)
                    {
                        x.BackColor = Color.White;

                    }
                }
            }

            // Copy activePiece to Ghost2
            for (int x = 0; x < 4; x++)
            {
                Ghost2[x] = activePiece2[x];
            }

            // Test Ghost2 in each row
            for (int x = 21; x > 1; x--)
            {
                // Get position of test Ghost2, starting at bottom row
                if (currentPiece == 0) //I piece
                {


                    if (rotations == 0)
                    {
                        if (x == 2)
                        {
                            Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                        }
                        else
                        {
                            Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                            Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x - 2);
                            Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 3);
                        }
                    }
                    else if (rotations == 1)
                    {
                        if (x == 2) //ignore
                        {
                            Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                        }

                        else //problem
                        {
                            Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x);
                            Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                            Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                        }
                    }   
                }
                else if (currentPiece == 1) // L piece
                {
                    if (rotations == 0)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 2);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                    }
                    else if (rotations == 1)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 1);
                    }
                    else if (rotations == 2)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 2);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 2);
                    }
                    else if (rotations == 3)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x - 1);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                    }
                }
                else if (currentPiece == 2) // J piece
                {
                    if (rotations == 0)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 2);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                    }
                    else if (rotations == 1)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x - 1);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                    }
                    else if (rotations == 2)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 2);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 2);
                    }
                    else if (rotations == 3)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 1);
                    }
                }
                else if (currentPiece == 3) // S piece
                {
                    if (rotations == 0)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x - 1);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 1);
                    }
                    else if (rotations == 1)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 2);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 1);
                    }
                }
                else if (currentPiece == 4) // Z piece
                {
                    if (rotations == 0)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                    }
                    else if (rotations == 1)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x - 1);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 2);
                    }
                }
                else if (currentPiece == 5) // O piece
                {
                    Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                    Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                    Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                    Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                }
                else if (currentPiece == 6) //T piece
                {
                    if (rotations == 0)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                    }
                    else if (rotations == 1)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 2);
                    }
                    else if (rotations == 2)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 1);
                    }
                    else if (rotations == 3)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x - 2);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                    }
                }

                //If no valid ghost stored
                if (ghostFound == false)
                {
                    // If all squares in test Ghost2 are white,
                    if (
                        (Ghost2[0].BackColor == Color.White | activePiece.Contains(Ghost2[0])) &
                        (Ghost2[1].BackColor == Color.White | activePiece.Contains(Ghost2[1])) &
                        (Ghost2[2].BackColor == Color.White | activePiece.Contains(Ghost2[2])) &
                        (Ghost2[3].BackColor == Color.White | activePiece.Contains(Ghost2[3]))
                        )
                    {
                     
                        // Store Ghost
                        ghostFound = true;
                        for (int y = 0; y < 4; y++)
                        {
                            Ghost[y] = Ghost2[y];
                        }
                    }

                    // If not all white (and nothing stored) check the next row up
                    else
                    {
                        continue;
                    }
                }

                //valid ghost already stored
                else if (ghostFound == true) 
                {

                    //Not all squares white
                    if (Ghost2[0].BackColor != Color.White | Ghost2[1].BackColor != Color.White | Ghost2[2].BackColor != Color.White |Ghost2[3].BackColor != Color.White)
                    {

                        //Is falling piece below x?
                        if (grid.GetRow(activePiece[0]) >= x | grid.GetRow(activePiece[1]) >= x | grid.GetRow(activePiece[2]) >= x | grid.GetRow(activePiece[3]) >= x)
                        {
                            continue;
                        }


                        //Reset
                        ghostFound = false;
                        for (int y = 0; y < 4; y++)
                        {
                            Ghost[y] = null;
                        }
                        continue;
                    }
                }
            }

            //Draw ghost
            if (ghostFound == true)
            {
                for (int x = 0; x < 4; x++)
                {
                    Ghost[x].BackColor = Color.LightGray;
                }
            }
        }

        // Timer for score notification
        private void timer3_Tick(object sender, EventArgs e)
        {
                label8.Text = "";
                timer3.Stop();
        }







    }   
}