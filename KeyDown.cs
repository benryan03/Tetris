using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    public partial class MainWindow : Form
    {
        // Handle inputs - triggered on any keypress
        // Cleanup needed
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
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
                    // Erase falling piece
                    foreach (Control x in activePiece)
                    {
                        x.BackColor = Color.White;
                    }

                    savedPieceInt = currentPiece;

                    // Layout options for saved piece
                    Control[,] savedPieceArray =
                    {
                        { box219, box223, box227, box231 }, // I piece
                        { box218, box222, box226, box227 }, // L piece
                        { box219, box223, box227, box226 }, // J piece
                        { box222, box223, box219, box220 }, // S piece
                        { box218, box219, box223, box224 }, // Z piece
                        { box222, box223, box226, box227 }, // O piece
                        { box223, box226, box227, box228 }  // T piece
                    };

                    // Retrieve layout for saved piece
                    for (int x = 0; x < 4; x++)
                    {
                        savedPiece[x] = savedPieceArray[savedPieceInt, x];
                    }

                    // Populate saved piece box
                    savedPieceColor = colorList[savedPieceInt];
                    foreach (Control x in savedPiece)
                    {
                        x.BackColor = savedPieceColor;
                    }

                    DropNewPiece();
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

                    Control[,] savedPieceArray =
                    {
                        { box219, box223, box227, box231 }, // I piece
                        { box218, box222, box226, box227 }, // L piece
                        { box219, box223, box227, box226 }, // J piece
                        { box222, box223, box219, box220 }, // S piece
                        { box218, box219, box223, box224 }, // Z piece
                        { box222, box223, box226, box227 }, // O piece
                        { box223, box226, box227, box228 }  // T piece
                    };

                    for (int x = 0; x < 4; x++)
                    {
                        savedPiece[x] = savedPieceArray[savedPieceInt, x];
                    }

                    savedPieceColor = colorList[savedPieceInt];

                    //Populate squares in saved piece panel
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
    }
}